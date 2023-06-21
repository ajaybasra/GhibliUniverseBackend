using System.Net;
using System.Text;
using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.API.Mapper;
using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace GhibliUniverse.Core.Tests.IntegrationTests;

public class FilmTests
{
    private readonly MappingProfiles _mappingProfiles;
     private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;
    private readonly GhibliUniverseWebApplicationFactory<Program> _ghibliUniverseWebApplicationFactory;
    private readonly HttpClient _client;

    public FilmTests()
    {
        _mappingProfiles = new MappingProfiles();
        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(_mappingProfiles));
        _mapper = new Mapper(_mapperConfiguration);
        _ghibliUniverseWebApplicationFactory = new GhibliUniverseWebApplicationFactory<Program>();
        _client = _ghibliUniverseWebApplicationFactory.CreateClient();
    }

    [Fact]
    public void GetAllFilmsEndpoint_ReturnsListOfFilmResponseDTOAnd200StatusCode_WhenCalled()
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var films = context.Films
            .Select(film => film).ToList();
        var filmResponseDTOS = _mapper.Map<List<FilmResponseDTO>>(films);
        var expectedResult = JsonConvert.SerializeObject(filmResponseDTOS,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = _client.GetAsync("api/Film").Result;
        var result = response.Content.ReadAsStringAsync().Result;
        
        Assert.Equal(HttpStatusCode.OK ,response.StatusCode);
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void GetFilmByIdEndpoint_ReturnsFilmResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var films = context.Films
            .Select(film => film).ToList();
        var filmResponseDTO = _mapper.Map<FilmResponseDTO>(films[1]);
        var expectedResult = JsonConvert.SerializeObject(filmResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = _client.GetAsync("api/Film/00000000-0000-0000-0000-000000000001").Result;
        var result = response.Content.ReadAsStringAsync().Result;
        
        Assert.Equal(HttpStatusCode.OK ,response.StatusCode);
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void GetFilmByIdEndpoint_Returns404StatusCodeWithGuid_WhenGivenNonExistentId()
    {
        var response = _client.GetAsync("api/Film/00000040-0000-5000-0000-000000000001").Result;
        
        Assert.Equal(HttpStatusCode.NotFound ,response.StatusCode);
    }

    [Fact]
    public void GetVoiceActorsByFilmEndpoint_ReturnsListOfVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var films = context.Films
            .Include(f => f.VoiceActors)
            .Select(film => film).ToList();
        var voiceActorResponseDTOS = _mapper.Map<List<VoiceActorResponseDTO>>(films[1].VoiceActors);
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDTOS,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = _client.GetAsync($"api/Film/00000000-0000-0000-0000-000000000001/voiceActors").Result;
        var result = response.Content.ReadAsStringAsync().Result;
        
        Assert.Equal(HttpStatusCode.OK ,response.StatusCode);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetVoiceActorsByFilmEndpoint_Returns404StatusCode_WhenGivenNonExistentId()
    {
        var response = _client.GetAsync($"api/Film/00005000-0000-0000-0000-000000000001/voiceActors").Result;
        
        Assert.Equal(HttpStatusCode.NotFound ,response.StatusCode);
        
    }

    [Fact]
    public void CreateFilmEndpoint_ReturnsFilmResponseDTOAnd200StatusCode_WhenGivenValidInput() //hard
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var film = new Film()
        {
            Id = Guid.Parse("30000000-0040-0000-0000-000000000001"),
            Title = ValidatedString.From("Not Spirited Awayz"),
            Description =
                ValidatedString.From(
                    "This is a description..z"),
            Director = ValidatedString.From("Lebronz"),
            Composer = ValidatedString.From("MJz"),
            ReleaseYear = ReleaseYear.From(1996),
        };
        var filmRequestDTOAsJson = JsonConvert.SerializeObject(
            _mapper.Map<FilmRequestDTO>(film),
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        // var expectedFilm = JsonConvert.SerializeObject(
        //     _mapper.Map<FilmResponseDTO>(film),
        //     new JsonSerializerSettings() {ContractResolver = new CamelCasePropertyNamesContractResolver()});
        
        var response = _client.PostAsync("/api/Film",
            new StringContent(filmRequestDTOAsJson, Encoding.UTF8, "application/json")).Result;
        var films = context.Films
            .Select(film => film).ToList();
        var filmResponseDTO = _mapper.Map<FilmResponseDTO>(films[2]);
        var expectedResult = JsonConvert.SerializeObject(filmResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var actualHttpStatusCode = response.StatusCode;
        var actualFilm = response.Content.ReadAsStringAsync().Result;

        
        Assert.Equal(HttpStatusCode.OK, actualHttpStatusCode);
        Assert.Equal(expectedResult, actualFilm);
        
    }
    
    [Fact]
    public void CreateFilmEndpoint_Returns400StatusCode_WhenGivenInvalidInput()
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
         
         var response = _client.PostAsync("/api/Film",
             new StringContent(filmRequestDTOAsJson, Encoding.UTF8, "application/json")).Result;
         
         Assert.Equal(HttpStatusCode.BadRequest ,response.StatusCode);
     }
     
     [Fact]
     public void LinkVoiceActorEndpoint_Returns200StatusCode_WhenGivenValidId()
     {
         using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
         var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
         var voiceActors = context.VoiceActors
             .Select(voiceActor => voiceActor).ToList();
         var voiceActorIdAsJson = JsonConvert.SerializeObject(voiceActors[1].Id);
         
         var response = _client.PostAsync($"/api/Film/00000000-0000-0000-0000-000000000001/LinkVoiceActor", new StringContent(voiceActorIdAsJson, Encoding.UTF8, "application/json")).Result;
         var films = context.Films
             .Include(f => f.VoiceActors)
             .Select(film => film).ToList();
         var voiceActorResponseDTOS = _mapper.Map<List<VoiceActorResponseDTO>>(films[1].VoiceActors);
         var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDTOS,
             new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
         var responseTwo = _client.GetAsync($"api/Film/00000000-0000-0000-0000-000000000001/voiceActors").Result;
         var result = responseTwo.Content.ReadAsStringAsync().Result;
         
         Assert.Equal(HttpStatusCode.OK, response.StatusCode);
         Assert.Equal(expectedResult, result);
     }
     
     [Fact]
     public void UnlinkVoiceActorEndpoint_Returns200StatusCode_WhenGivenValidId()
     {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var voiceActors = context.VoiceActors
            .Select(voiceActor => voiceActor).ToList();
        var voiceActorIdAsJson = JsonConvert.SerializeObject(voiceActors[0].Id);
        
        var response = _client.PostAsync($"/api/Film/00000000-0000-0000-0000-000000000001/UnlinkVoiceActor", new StringContent(voiceActorIdAsJson, Encoding.UTF8, "application/json")).Result;
        var films = context.Films
            .Include(f => f.VoiceActors)
            .Select(film => film).ToList();
        var voiceActorResponseDTOS = _mapper.Map<List<VoiceActorResponseDTO>>(films[1].VoiceActors);
        var expectedResult = JsonConvert.SerializeObject(voiceActorResponseDTOS,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var responseTwo = _client.GetAsync($"api/Film/00000000-0000-0000-0000-000000000001/voiceActors").Result;
        var result = responseTwo.Content.ReadAsStringAsync().Result;
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
     }
     
     [Fact]
     public void UpdateFilmEndpoint_ReturnsFilmResponseDTOAnd200StatusCode_WhenGivenValidInput()
     {
         using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
         var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
         var filmRequestDto = new FilmRequestDTO()
             { Title = "test", Description = "test", Director = "test", Composer = "test", ReleaseYear = 2000 };
         var filmRequestDtoAsJson = JsonConvert.SerializeObject(filmRequestDto, 
             new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
         
         var response = _client.PutAsync($"/api/Film/00000000-0000-0000-0000-000000000001", new StringContent(filmRequestDtoAsJson, Encoding.UTF8, "application/json")).Result;
         var films = context.Films
             .Select(film => film).ToList();
         var filmResponseDTO = _mapper.Map<FilmResponseDTO>(films[1]);
         var expectedResult = JsonConvert.SerializeObject(filmResponseDTO,
             new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
         var actualUpdatedFilm = response.Content.ReadAsStringAsync().Result;
         
         Assert.Equal(HttpStatusCode.OK, response.StatusCode);
         Assert.Equal(expectedResult, actualUpdatedFilm);
     }
     
     [Fact]
     public void DeleteFilmEndpoint_Returns200StatusCode_WhenGivenValidId()
     {
         var filmToBeRemoved = new FilmResponseDTO
         {
             Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
             Title = ValidatedString.From("Not Spirited Away"),
             Description =
                 ValidatedString.From(
                     "This is a description.."),
             Director = ValidatedString.From("Lebron"),
             Composer = ValidatedString.From("MJ"),
             ReleaseYear = ReleaseYear.From(1995),
         };
         using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
         var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
         
         var response = _client.DeleteAsync($"api/Film/00000000-0000-0000-0000-000000000001").Result;
         var actualHttpStatusCode = response.StatusCode;
         var films = context.Films
             .Select(film => film).ToList();
         
         Assert.Equal(HttpStatusCode.OK, actualHttpStatusCode);
         Assert.DoesNotContain(films, film => film.Equals(filmToBeRemoved));
     }
     
}
