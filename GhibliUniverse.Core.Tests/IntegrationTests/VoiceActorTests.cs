using System.Net;
using System.Text;
using AutoMapper;
using GhibliUniverse.API.Controllers;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.API.Mapper;
using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GhibliUniverse.Core.Tests.IntegrationTests;

public class VoiceActorTests
{
    private readonly MappingProfiles _mappingProfiles;
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;
    private readonly GhibliUniverseWebApplicationFactory<Program> _ghibliUniverseWebApplicationFactory;
    private readonly HttpClient _client;

    public VoiceActorTests()
    {
        _mappingProfiles = new MappingProfiles();
        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(_mappingProfiles));
        _mapper = new Mapper(_mapperConfiguration);
        _ghibliUniverseWebApplicationFactory = new GhibliUniverseWebApplicationFactory<Program>();
        _client = _ghibliUniverseWebApplicationFactory.CreateClient();
    }
    
    [Fact]
    public void GetAllVoiceActorsEndpoint_ReturnsListOfVoiceActorResponseDTOAnd200StatusCode_WhenCalled()
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var voiceActors = context.VoiceActors
            .Select(voiceActor => voiceActor).ToList();
        var voiceActorResponseDTOS = _mapper.Map<List<VoiceActorResponseDTO>>(voiceActors);
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDTOS,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = _client.GetAsync("api/VoiceActor").Result;
        var result = response.Content.ReadAsStringAsync().Result;
        
        Assert.Equal(HttpStatusCode.OK ,response.StatusCode);
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void GetVoiceActorByIdEndpoint_ReturnsVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var voiceActors = context.VoiceActors
            .Select(voiceActor => voiceActor).ToList();
        var voiceActorResponseDto = _mapper.Map<VoiceActorResponseDTO>(voiceActors[1]);
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDto,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = _client.GetAsync("api/VoiceActor/00000000-0000-0000-0000-000000000002").Result;
        var result = response.Content.ReadAsStringAsync().Result;
        
        Assert.Equal(HttpStatusCode.OK ,response.StatusCode);
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void GetVoiceActorByIdEndpoint_Returns404StatusCodeWithGuid_WhenGivenNonExistentId()
    {
        var response = _client.GetAsync("api/VoiceActor/00050040-0000-5000-0000-000000000001").Result;
        
        Assert.Equal(HttpStatusCode.NotFound ,response.StatusCode);
    }
    
    [Fact]
    public void GetFilmsByVoiceActorEndpoint_ReturnsListOfFilmResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var voiceActors = context.VoiceActors
            .Include(v => v.Films)
            .Select(voiceActor => voiceActor).ToList();
        var filmResponseDTOS = _mapper.Map<List<FilmResponseDTO>>(voiceActors[0].Films);
        var expectedResult = JsonConvert.SerializeObject(filmResponseDTOS,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = _client.GetAsync($"api/VoiceActor/10000000-0000-0000-0000-000000000000/Films").Result;
        var result = response.Content.ReadAsStringAsync().Result;
        
        Assert.Equal(HttpStatusCode.OK ,response.StatusCode);
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void GetFilmsByVoiceActorEndpoint_Returns404StatusCode_WhenGivenNonExistentId()
    {
        var response = _client.GetAsync($"api/VoiceActor/00005000-0000-0000-0000-000000000001/Films").Result;
        
        Assert.Equal(HttpStatusCode.NotFound ,response.StatusCode);
    }
    
    [Fact]
    public void CreateVoiceActorEndpoint_ReturnsVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidInput() 
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var voiceActor = new VoiceActor()
        {
            Id = Guid.Parse("30000000-0040-0000-0000-000000000001"),
            Name = ValidatedString.From("Ajay Basra")
        };
        var voiceActorRequestDTOAsJson = JsonConvert.SerializeObject(
            _mapper.Map<VoiceActorRequestDTO>(voiceActor),
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = _client.PostAsync("/api/VoiceActor",
            new StringContent(voiceActorRequestDTOAsJson, Encoding.UTF8, "application/json")).Result;
        var voiceActors = context.VoiceActors
            .Select(voiceActor => voiceActor).ToList();
        var voiceActorResponseDTO = _mapper.Map<VoiceActorResponseDTO>(voiceActors[2]);
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var actualHttpStatusCode = response.StatusCode;
        var actualVoiceActor = response.Content.ReadAsStringAsync().Result;

        
        Assert.Equal(HttpStatusCode.OK, actualHttpStatusCode);
        Assert.Equal(expectedResult, actualVoiceActor);
        
    }

    [Fact]
    public void CreateFilmEndpoint_Returns400StatusCode_WhenGivenInvalidInput()
    {
        var voiceActorRequestDTO = new VoiceActorRequestDTO()
        {
            Name = ""
        };
        var voiceActorRequestDTOAsJson = JsonConvert.SerializeObject(
            voiceActorRequestDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = _client.PostAsync("/api/VoiceActor",
            new StringContent(voiceActorRequestDTOAsJson, Encoding.UTF8, "application/json")).Result;

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public void UpdateVoiceActorEndpoint_ReturnsVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidInput()
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var voiceActorRequestDTO = new VoiceActorRequestDTO()
            { Name = "test name" };
        var filmRequestDtoAsJson = JsonConvert.SerializeObject(voiceActorRequestDTO, 
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
         
        var response = _client.PutAsync($"/api/VoiceActor/00000000-0000-0000-0000-000000000002", new StringContent(filmRequestDtoAsJson, Encoding.UTF8, "application/json")).Result;
        var voiceActors = context.VoiceActors
            .Select(voiceActor => voiceActor).ToList();
        var voiceActorResponseDTO = _mapper.Map<VoiceActorResponseDTO>(voiceActors[1]);
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var actualUpdatedVoiceActor = response.Content.ReadAsStringAsync().Result;
         
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, actualUpdatedVoiceActor);
    }
    
    [Fact]
    public void DeleteVoiceActorEndpoint_Returns200StatusCode_WhenGivenValidId()
    {
        var voiceActorToBeRemoved = new VoiceActorResponseDTO()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name = ValidatedString.From("Test Actor"),
        };
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
         
        var response = _client.DeleteAsync($"api/VoiceActor/00000000-0000-0000-0000-000000000002").Result;
        var actualHttpStatusCode = response.StatusCode;
        var voiceActors = context.VoiceActors
            .Select(voiceActor => voiceActor).ToList();
         
        Assert.Equal(HttpStatusCode.OK, actualHttpStatusCode);
        Assert.DoesNotContain(voiceActors, voiceActor => voiceActor.Equals(voiceActorToBeRemoved));
    }
}