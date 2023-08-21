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

public class VoiceActorControllerTests
{
    private readonly MappingProfiles _apiMappingProfiles;
    private readonly Core.Repository.MappingProfiles.MappingProfiles _coreMappingProfiles;
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;
    private readonly GhibliUniverseContext _context;
    private readonly HttpClient _httpClient;

    public VoiceActorControllerTests()
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
    public async Task GetAllVoiceActorsEndpoint_ReturnsListOfVoiceActorResponseDTOAnd200StatusCode_WhenCalled()
    {
        var voiceActors = await _context.VoiceActors
            .Select(voiceActor => voiceActor).ToListAsync();
        var voiceActorValueObjects = _mapper.Map<List<VoiceActor>>(voiceActors);
        var voiceActorResponseDTOS = _mapper.Map<List<VoiceActorResponseDTO>>(voiceActorValueObjects);
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDTOS,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.GetAsync("api/VoiceActor");
        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
    }


    [Fact]
    public async Task GetVoiceActorByIdEndpoint_ReturnsVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        var voiceActors = await _context.VoiceActors
            .Select(voiceActor => voiceActor).ToListAsync();
        var voiceActorValueObject = _mapper.Map<VoiceActor>(voiceActors[1]);
        var voiceActorResponseDto = _mapper.Map<VoiceActorResponseDTO>(voiceActorValueObject);
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDto,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.GetAsync("api/VoiceActor/00000000-0000-0000-0000-000000000002");
        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public async Task GetVoiceActorByIdEndpoint_Returns404StatusCodeWithGuid_WhenGivenNonExistentId()
    {
        var response = await _httpClient.GetAsync("api/VoiceActor/00050040-0000-5000-0000-000000000001");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetFilmsByVoiceActorEndpoint_ReturnsListOfFilmResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        var voiceActors = await _context.VoiceActors
            .Include(v => v.Films)
            .Select(voiceActor => voiceActor)
            .ToListAsync();

        var filmValueObjects = _mapper.Map<List<VoiceActorFilm>>(voiceActors[0].Films);
        var filmResponseDTOS = _mapper.Map<List<VoiceActorFilmResponseDTO>>(filmValueObjects);

        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = { new AverageRatingConverter() }
        };
        var expectedResult = JsonConvert.SerializeObject(filmResponseDTOS, serializerSettings);

        var response = await _httpClient.GetAsync($"api/VoiceActor/10000000-0000-0000-0000-000000000000/Films");
        var result = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
    }

    
    [Fact]
    public async Task GetFilmsByVoiceActorEndpoint_Returns404StatusCode_WhenGivenNonExistentId()
    {
        var response = await _httpClient.GetAsync($"api/VoiceActor/00005000-0000-0000-0000-000000000001/Films");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateVoiceActorEndpoint_ReturnsVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidInput()
    {
        var voiceActor = new VoiceActor()
        {
            Id = Guid.Parse("30000000-0040-0000-0000-000000000001"),
            Name = ValidatedString.From("Ajay Basra")
        };
        var voiceActorRequestDTOAsJson = JsonConvert.SerializeObject(
            _mapper.Map<VoiceActorRequestDTO>(voiceActor),
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.PostAsync("/api/VoiceActor",
            new StringContent(voiceActorRequestDTOAsJson, Encoding.UTF8, "application/json"));
        var voiceActors = await _context.VoiceActors
            .Select(voiceActor => voiceActor).ToListAsync();
        var voiceActorValueObject = _mapper.Map<VoiceActor>(voiceActors[2]);
        var voiceActorResponseDTO = _mapper.Map<VoiceActorResponseDTO>(voiceActorValueObject);
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var actualHttpStatusCode = response.StatusCode;
        var actualVoiceActor = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, actualHttpStatusCode);
        Assert.Equal(expectedResult, actualVoiceActor);
    }

    [Fact]
    public async Task CreatVoiceActorEndpoint_Returns400StatusCode_WhenGivenInvalidInput()
    {
        var voiceActorRequestDTO = new VoiceActorRequestDTO()
        {
            Name = ""
        };
        var voiceActorRequestDTOAsJson = JsonConvert.SerializeObject(
            voiceActorRequestDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.PostAsync("/api/VoiceActor",
            new StringContent(voiceActorRequestDTOAsJson, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateVoiceActorEndpoint_ReturnsVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidInput()
    {
        var voiceActorRequestDTO = new VoiceActorRequestDTO()
        {
            Name = "test name"
        };
        var filmRequestDtoAsJson = JsonConvert.SerializeObject(voiceActorRequestDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.PutAsync($"/api/VoiceActor/00000000-0000-0000-0000-000000000002", new StringContent(filmRequestDtoAsJson, Encoding.UTF8, "application/json"));
        var expectedVoiceActorResponseDTO = new VoiceActorResponseDTO()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name = "test name"
        };
        var expectedResult = JsonConvert.SerializeObject(expectedVoiceActorResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var actualUpdatedVoiceActor = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, actualUpdatedVoiceActor);
    }
    
    [Fact]
    public async Task UpdateVoiceActorEndpoint_Returns400StatusCode_WhenGivenInvalidInput()
    {
        var voiceActorRequestDTO = new VoiceActorRequestDTO()
        {
            Name = ""
        };
        var filmRequestDtoAsJson = JsonConvert.SerializeObject(voiceActorRequestDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _httpClient.PutAsync($"/api/VoiceActor/00000000-0000-0000-0000-000000000002", new StringContent(filmRequestDtoAsJson, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    
    [Fact]
    public async Task DeleteVoiceActorEndpoint_Returns200StatusCode_WhenGivenValidId()
    {
        var voiceActorToBeRemoved = new VoiceActorResponseDTO()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name = "Test Actor"
        };

        var response = await _httpClient.DeleteAsync($"api/VoiceActor/00000000-0000-0000-0000-000000000002");
        var actualHttpStatusCode = response.StatusCode;
        var voiceActors = await _context.VoiceActors
            .Select(voiceActor => voiceActor).ToListAsync();

        Assert.Equal(HttpStatusCode.OK, actualHttpStatusCode);
        Assert.DoesNotContain(voiceActors, voiceActor => voiceActor.Equals(voiceActorToBeRemoved));
    }
    
    [Fact]
    public async Task DeleteVoiceActorEndpoint_Returns400StatusCode_WhenGivenInvalidId()
    {
        var response = await _httpClient.DeleteAsync($"api/VoiceActor/08000000-0000-0000-0000-000000000002");

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