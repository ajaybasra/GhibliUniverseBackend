using System.Net;
using System.Text;
using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.API.Mapper;
using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.IntegrationTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GhibliUniverse.IntegrationTests.Controllers;

public class FilmControllerTests
{
    private readonly MappingProfiles _mappingProfiles;
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;
    private readonly GhibliUniverseContext _context;
    private readonly HttpClient _httpClient;

    public FilmControllerTests()
    {
        _mappingProfiles = new MappingProfiles();
        
        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(_mappingProfiles));
        
        _mapper = new Mapper(_mapperConfiguration);

        var dbContextOptions = new DbContextOptionsBuilder<GhibliUniverseContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options;
        
        _httpClient = CustomHttpClientFactory.Create(dbContextOptions);
        
        _context = new GhibliUniverseContext(dbContextOptions);

        TestDatabaseSeeder.SeedContext(_context);
    }
    
    [Fact]
    public async Task GetAllFilmsEndpoint_ReturnsListOfFilmResponseDTOAnd200StatusCode_WhenCalled()
    {
        var films = await _context.Films
            .Select(film => film)
            .ToListAsync();
        var filmResponseDTOS = _mapper.Map<List<FilmResponseDTO>>(films);
        var expectedResult = JsonConvert.SerializeObject(filmResponseDTOS,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.GetAsync("api/Film");
        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task GetFilmByIdEndpoint_ReturnsFilmResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        var films = await _context.Films
            .Select(film => film)
            .ToListAsync();
        var filmResponseDTO = _mapper.Map<FilmResponseDTO>(films[1]);
        var expectedResult = JsonConvert.SerializeObject(filmResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.GetAsync("api/Film/00000000-0000-0000-0000-000000000001");
        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public async Task GetFilmByIdEndpoint_Returns404StatusCodeWithGuid_WhenGivenNonExistentId()
    {
        var response = await _httpClient.GetAsync("api/Film/00000040-0000-5000-0000-000000000001");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetVoiceActorsByFilmEndpoint_ReturnsListOfVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        var films = await _context.Films
            .Include(f => f.VoiceActors)
            .Select(film => film)
            .ToListAsync();
        var voiceActorResponseDTOS = _mapper.Map<List<VoiceActorResponseDTO>>(films[1].VoiceActors);
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDTOS,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.GetAsync($"api/Film/00000000-0000-0000-0000-000000000001/voiceActors");
        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task GetVoiceActorsByFilmEndpoint_Returns404StatusCode_WhenGivenNonExistentId()
    {
        var response = await _httpClient.GetAsync($"api/Film/00005000-0000-0000-0000-000000000001/voiceActors");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateFilmEndpoint_ReturnsFilmResponseDTOAnd200StatusCode_WhenGivenValidInput()
    {
        var film = new Film()
        {
            Id = Guid.Parse("30000000-0040-0000-0000-000000000001"),
            Title = ValidatedString.From("Not Spirited Awayz"),
            Description = ValidatedString.From("This is a description..z"),
            Director = ValidatedString.From("Lebronz"),
            Composer = ValidatedString.From("MJz"),
            ReleaseYear = ReleaseYear.From(1996),
        };
        var filmRequestDTOAsJson = JsonConvert.SerializeObject(
            _mapper.Map<FilmRequestDTO>(film),
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.PostAsync("/api/Film",
            new StringContent(filmRequestDTOAsJson, Encoding.UTF8, "application/json"));
        var films = await _context.Films
            .Select(film => film)
            .ToListAsync();
        var filmResponseDTO = _mapper.Map<FilmResponseDTO>(films[2]);
        var expectedResult = JsonConvert.SerializeObject(filmResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var actualHttpStatusCode = response.StatusCode;
        var actualFilm = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, actualHttpStatusCode);
        Assert.Equal(expectedResult, actualFilm);
    }
    
    [Fact]
    public async Task CreateFilmEndpoint_Returns400StatusCode_WhenGivenEmptyInput()
    {
        var filmRequestDTO = new FilmRequestDTO()
        {
            Title = "",
            Description = "",
            Director = "",
            Composer = "",
            ReleaseYear = 2000
        };
        var filmRequestDTOAsJson = JsonConvert.SerializeObject(
            filmRequestDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.PostAsync("/api/Film",
            new StringContent(filmRequestDTOAsJson, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateFilmEndpoint_Returns400StatusCode_WhenGivenInvalidReleaseYear()
    {
        var filmRequestDTO = new FilmRequestDTO()
        {
            Title = "a",
            Description = "a",
            Director = "a",
            Composer = "a",
            ReleaseYear = -2000
        };
        var filmRequestDTOAsJson = JsonConvert.SerializeObject(
            filmRequestDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.PostAsync("/api/Film",
            new StringContent(filmRequestDTOAsJson, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
     
    [Fact]
    public async Task LinkVoiceActorEndpoint_Returns200StatusCode_WhenGivenValidId()
    {
        var voiceActors = await _context.VoiceActors
            .Select(voiceActor => voiceActor)
            .ToListAsync();
        var voiceActorIdAsJson = JsonConvert.SerializeObject(voiceActors[1].Id);

        var response = await _httpClient.PostAsync($"/api/Film/00000000-0000-0000-0000-000000000001/LinkVoiceActor",
            new StringContent(voiceActorIdAsJson, Encoding.UTF8, "application/json"));
    
        var films = await _context.Films
            .Include(f => f.VoiceActors)
            .Select(film => film)
            .ToListAsync();
        var voiceActorResponseDTOS = _mapper.Map<List<VoiceActorResponseDTO>>(films[1].VoiceActors);
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDTOS,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
    
        var responseTwo = await _httpClient.GetAsync($"api/Film/00000000-0000-0000-0000-000000000001/voiceActors");
        var result = await responseTwo.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task LinkVoiceActorEndpoint_Returns400StatusCode_WhenGivenInvalidId()
    {
        var voiceActors = await _context.VoiceActors
            .Select(voiceActor => voiceActor)
            .ToListAsync();
        var voiceActorIdAsJson = JsonConvert.SerializeObject(voiceActors[1].Id);

        var response = await _httpClient.PostAsync($"/api/Film/00000000-0000-0000-0000-400000000001/LinkVoiceActor",
            new StringContent(voiceActorIdAsJson, Encoding.UTF8, "application/json"));
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task UnlinkVoiceActorEndpoint_Returns200StatusCode_WhenGivenValidId()
    {
        var voiceActors = _context.VoiceActors
            .Select(voiceActor => voiceActor).ToList();
        var voiceActorIdAsJson = JsonConvert.SerializeObject(voiceActors[0].Id);

        var response = await _httpClient.PostAsync($"/api/Film/00000000-0000-0000-0000-000000000001/UnlinkVoiceActor", new StringContent(voiceActorIdAsJson, Encoding.UTF8, "application/json"));
        var voiceActorResponseDTOS = new List<VoiceActorResponseDTO>();
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDTOS,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var responseTwo = await _httpClient.GetAsync($"api/Film/00000000-0000-0000-0000-000000000001/voiceActors");
        var result = await responseTwo.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public async Task UnlinkVoiceActorEndpoint_Returns400StatusCode_WhenGivenInvalidId()
    {
        var voiceActors = _context.VoiceActors
            .Select(voiceActor => voiceActor).ToList();
        var voiceActorIdAsJson = JsonConvert.SerializeObject(voiceActors[0].Id);

        var response = await _httpClient.PostAsync($"/api/Film/00000000-0000-5000-0000-000000000001/UnlinkVoiceActor", new StringContent(voiceActorIdAsJson, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
     
    [Fact]
    public async Task UpdateFilmEndpoint_ReturnsFilmResponseDTOAnd200StatusCode_WhenGivenValidInput()
    {
        var filmRequestDto = new FilmRequestDTO()
        {
            Title = "test",
            Description = "test",
            Director = "test",
            Composer = "test",
            ReleaseYear = 2000
        };
        var filmRequestDtoAsJson = JsonConvert.SerializeObject(filmRequestDto,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.PutAsync($"/api/Film/00000000-0000-0000-0000-000000000001", new StringContent(filmRequestDtoAsJson, Encoding.UTF8, "application/json"));
        var expectedFilmResponseDTO = new FilmResponseDTO()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Title = ValidatedString.From("test"),
            Description = ValidatedString.From("test"),
            Director = ValidatedString.From("test"),
            Composer = ValidatedString.From("test"),
            ReleaseYear = ReleaseYear.From(2000),
        };
        var expectedResult = JsonConvert.SerializeObject(expectedFilmResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var actualUpdatedFilm = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, actualUpdatedFilm);
    }
    
    [Fact]
    public async Task UpdateFilmEndpoint_Returns400StatusCode_WhenTitleIsEmpty()
    {
        var filmRequestDto = new FilmRequestDTO()
        {
            Title = "", 
            Description = "Updated film description",
            Director = "Updated film director",
            Composer = "Updated film composer",
            ReleaseYear = 2023
        };
        var filmRequestDtoAsJson = JsonConvert.SerializeObject(filmRequestDto,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.PutAsync($"/api/Film/00000000-0000-0000-0000-000000000001", new StringContent(filmRequestDtoAsJson, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateFilmEndpoint_Returns400StatusCode_WhenReleaseYearIsInvalid()
    {
        var filmRequestDto = new FilmRequestDTO()
        {
            Title = "A.", 
            Description = "Updated film description",
            Director = "Updated film director",
            Composer = "Updated film composer",
            ReleaseYear = -2023
        };
        var filmRequestDtoAsJson = JsonConvert.SerializeObject(filmRequestDto,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.PutAsync($"/api/Film/00000000-0000-0000-0000-000000000001", new StringContent(filmRequestDtoAsJson, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
     
    [Fact]
    public async Task DeleteFilmEndpoint_Returns200StatusCode_WhenGivenValidId()
    {
        var filmToBeRemoved = new FilmResponseDTO
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Title = ValidatedString.From("Not Spirited Away"),
            Description = ValidatedString.From("This is a description.."),
            Director = ValidatedString.From("Lebron"),
            Composer = ValidatedString.From("MJ"),
            ReleaseYear = ReleaseYear.From(1995),
        };

        var response = await _httpClient.DeleteAsync($"api/Film/00000000-0000-0000-0000-000000000001");
        var actualHttpStatusCode = response.StatusCode;
        var films = await _context.Films.Select(film => film).ToListAsync();

        Assert.Equal(HttpStatusCode.OK, actualHttpStatusCode);
        Assert.DoesNotContain(films, film => film.Id == filmToBeRemoved.Id);
    }
    
    [Fact]
    public async Task DeleteFilmEndpoint_Returns400StatusCode_WhenGivenInvalidId()
    {
        var response = await _httpClient.DeleteAsync($"api/Film/80000000-0000-0000-0000-000000000001");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}