using System.Globalization;
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
    private readonly MappingProfiles _apiMappingProfiles;
    private readonly Core.Repository.MappingProfiles.MappingProfiles _coreMappingProfiles;
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;
    private readonly GhibliUniverseContext _context;
    private readonly HttpClient _httpClient;

    public FilmControllerTests()
    {
        _apiMappingProfiles = new MappingProfiles();
        
        _coreMappingProfiles = new Core.Repository.MappingProfiles.MappingProfiles();
        
        _mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(_apiMappingProfiles);
            cfg.AddProfile(_coreMappingProfiles);
        });
        
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
        var filmResponseOneDTO = new FilmResponseDTO()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
            Title = "Spirited Away",
            Description = "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.",
            Director = "Hayao Miyazaki",
            Composer = "Joe Hisaishi",
            ReleaseYear = 2001,
            FilmReviewInfo =
            {
                AverageRating = 0,
                NumberOfRatings = 0
            }
        };
        var expectedResultOne = JsonConvert.SerializeObject(filmResponseOneDTO,
            new JsonSerializerSettings() 
            { ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new AverageRatingConverter() }
            });
        var filmResponseTwoDTO = new FilmResponseDTO()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Title = "Not Spirited Away",
            Description = "This is a description..",
            Director = "Lebron",
            Composer = "MJ",
            ReleaseYear = 1995,
            FilmReviewInfo =
            {
                AverageRating = 0,
                NumberOfRatings = 0
            }
        };
        var expectedResultTwo = JsonConvert.SerializeObject(filmResponseTwoDTO,
            new JsonSerializerSettings() 
            { ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new AverageRatingConverter() }
            });
        var expectedResult = $"[{expectedResultOne},{expectedResultTwo}]";

        var response = await _httpClient.GetAsync("api/Film");
        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task GetFilmByIdEndpoint_ReturnsFilmResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        var filmResponseDTO = new FilmResponseDTO()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Title = "Not Spirited Away",
            Description = "This is a description..",
            Director = "Lebron",
            Composer = "MJ",
            ReleaseYear = 1995,
            FilmReviewInfo =
            {
                AverageRating = 0,
                NumberOfRatings = 0
            }
        };
        var expectedResult = JsonConvert.SerializeObject(filmResponseDTO,
            new JsonSerializerSettings() 
            { ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new AverageRatingConverter() }
            });

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
        var voiceActorValueObjects = _mapper.Map<List<VoiceActor>>(films[1].VoiceActors);
        var voiceActorResponseDTOS = _mapper.Map<List<VoiceActorResponseDTO>>(voiceActorValueObjects);
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
    public async Task CreateFilmEndpoint_Returns200StatusCode_WhenGivenValidInput()
    {
        var filmRequestDTO = new FilmRequestDTO()
        {
            Title = "Not Spirited Awayz",
            Description = "This is a description..z",
            Director = "Lebronz",
            Composer = "MJz",
            ReleaseYear = 1996,
        };
        
        var filmRequestDTOAsJson = JsonConvert.SerializeObject(
            filmRequestDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.PostAsync("/api/Film",
            new StringContent(filmRequestDTOAsJson, Encoding.UTF8, "application/json"));
        var actualHttpStatusCode = response.StatusCode;

        Assert.Equal(HttpStatusCode.OK, actualHttpStatusCode);
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
        var voiceActorValueObjects = _mapper.Map<List<VoiceActor>>(films[1].VoiceActors);
        var voiceActorResponseDTOS = _mapper.Map<List<VoiceActorResponseDTO>>(voiceActorValueObjects);
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
            Title = "test",
            Description = "test",
            Director = "test",
            Composer = "test",
            ReleaseYear = 2000,
        };
        var expectedResult = JsonConvert.SerializeObject(expectedFilmResponseDTO,
            new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new AverageRatingConverter() }

            });
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
            Title = "Not Spirited Away",
            Description = "This is a description..",
            Director = "Lebron",
            Composer = "MJ",
            ReleaseYear = 1995,
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
    
    // Custom converter for AverageRating serialization so that trailing digit not shown, i.e. you get 0 and not 0.0
    public class AverageRatingConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            double rating = (double)value;

            if (rating == Math.Floor(rating))
            {
                writer.WriteRawValue(((int)rating).ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteValue(rating);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => false;
        public override bool CanConvert(Type objectType) => objectType == typeof(double);
    }
}