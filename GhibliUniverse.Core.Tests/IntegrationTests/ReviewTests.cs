using System.Net;
using System.Text;
using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.API.Mapper;
using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GhibliUniverse.Core.Tests.IntegrationTests;

public class ReviewTests
{
    private readonly MappingProfiles _mappingProfiles;
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly IMapper _mapper;
    private readonly GhibliUniverseWebApplicationFactory<Program> _ghibliUniverseWebApplicationFactory;
    private readonly HttpClient _client;

    public ReviewTests()
    {
        _mappingProfiles = new MappingProfiles();
        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(_mappingProfiles));
        _mapper = new Mapper(_mapperConfiguration);
        _ghibliUniverseWebApplicationFactory = new GhibliUniverseWebApplicationFactory<Program>();
        _client = _ghibliUniverseWebApplicationFactory.CreateClient();
    }

    [Fact]
    public void GetAllReviewsEndpoint_ReturnsListOfReviewResponseDTOAnd200StatusCode_WhenCalled()
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var reviews = context.Reviews
            .Select(review => review).ToList();
        var reviewResponseDTOS = _mapper.Map<List<ReviewResponseDTO>>(reviews);
        var expectedResult = JsonConvert.SerializeObject(reviewResponseDTOS,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = _client.GetAsync("api/Review").Result;
        var result = response.Content.ReadAsStringAsync().Result;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetReviewByIdEndpoint_ReturnsReviewResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var reviews = context.Reviews
            .Select(review => review).ToList();
        var reviewResponseDTO = _mapper.Map<ReviewResponseDTO>(reviews[1]);
        var expectedResult = JsonConvert.SerializeObject(reviewResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = _client.GetAsync("api/Review/00000000-0000-0000-0000-000000000002").Result;
        var result = response.Content.ReadAsStringAsync().Result;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void GetReviewByIdEndpoint_Returns404StatusCodeWithGuid_WhenGivenNonExistentId()
    {
        var response = _client.GetAsync("api/Review/00050040-0000-5000-0000-000000000001").Result;
        
        Assert.Equal(HttpStatusCode.NotFound ,response.StatusCode);
    }
    
    [Fact]
    public void CreateReviewEndpoint_ReturnsVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidInput() 
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var reviewRequestDTO = new ReviewRequestDTO()
        {
            rating = 7
        };
        var reviewRequestDTOAsJSON = JsonConvert.SerializeObject(
            reviewRequestDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        
        var response = _client.PostAsync("/api/Review/10000000-0000-0000-0000-000000000000",
            new StringContent(reviewRequestDTOAsJSON, Encoding.UTF8, "application/json")).Result;
        var reviews = context.Reviews
            .Select(review => review).ToList();
        var reviewResponseDTO = _mapper.Map<ReviewResponseDTO>(reviews[2]);
        var expectedResult = JsonConvert.SerializeObject(reviewResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var actualHttpStatusCode = response.StatusCode;
        var actualReview = response.Content.ReadAsStringAsync().Result;

        
        Assert.Equal(HttpStatusCode.OK, actualHttpStatusCode);
        Assert.Equal(expectedResult, actualReview);
        
    }
    
    [Fact]
    public async Task CreateReviewEndpoint_Returns400StatusCode_WhenGivenInvalidInput()
    {
        var reviewRequestDTO = new ReviewRequestDTO()
        {
            rating = -1
        };
        var reviewRequestDTOAsJson = JsonConvert.SerializeObject(
            reviewRequestDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = await _client.PostAsync($"/api/Review/10000000-0000-0000-0000-000000000000",
            new StringContent(reviewRequestDTOAsJson, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public void UpdateReviewEndpoint_ReturnsReviewResponseDTOAnd200StatusCode_WhenGivenValidInput()
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var reviewRequestDTO = new ReviewRequestDTO()
            { rating = 4 };
        var reviewRequestAsJSON = JsonConvert.SerializeObject(reviewRequestDTO, 
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
         
        var response = _client.PutAsync($"/api/Review/00000000-0000-0000-0000-000000000002", new StringContent(reviewRequestAsJSON, Encoding.UTF8, "application/json")).Result;
        var reviews = context.Reviews
            .Select(review => review).ToList();
        var reviewResponseDTO = _mapper.Map<ReviewResponseDTO>(reviews[1]);
        var expectedResult = JsonConvert.SerializeObject(reviewResponseDTO,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var actualUpdatedReview = response.Content.ReadAsStringAsync().Result;
         
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedResult, actualUpdatedReview);
    }
    
    [Fact]
    public void DeleteVoiceActorEndpoint_Returns200StatusCode_WhenGivenValidId()
    {
        var reviewToBeRemoved = new ReviewResponseDTO()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Rating = Rating.From(8),
        };
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
         
        var response = _client.DeleteAsync($"api/Review/00000000-0000-0000-0000-000000000002").Result;
        var actualHttpStatusCode = response.StatusCode;
        var reviews = context.Reviews
            .Select(review => review).ToList();
         
        Assert.Equal(HttpStatusCode.OK, actualHttpStatusCode);
        Assert.DoesNotContain(reviews, review => review.Equals(reviewToBeRemoved));
    }
}