using System.Net;
using GhibliUniverse.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GhibliUniverse.Core.Tests.IntegrationTests;

public class FilmTests
{
    private readonly GhibliUniverseWebApplicationFactory<Program> _ghibliUniverseWebApplicationFactory;
    private readonly HttpClient _client;

    public FilmTests()
    {
        _ghibliUniverseWebApplicationFactory = new GhibliUniverseWebApplicationFactory<Program>();
        _client = _ghibliUniverseWebApplicationFactory.CreateClient();
    }

    [Fact]
    public void GetAllFilms_ReturnsListOfFilmResponseAnd200StatusCode_WhenCalled()
    {
        using var scope = _ghibliUniverseWebApplicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
        var films = context.Films
            .Include(f => f.VoiceActors)
            .Include(f => f.Reviews)
            .Select(film => film).ToList();
        var expectResult = JsonConvert.SerializeObject(films,
            new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        var response = _client.GetAsync("api/Film").Result;
        var result = response.Content.ReadAsStringAsync().Result;
        
        Assert.True(response.StatusCode == HttpStatusCode.Conflict);
        Assert.Equal(expectResult, result);
    }
}
// using var scope = _factory.Services.CreateScope();
// var context = scope.ServiceProvider.GetRequiredService<TicTacToeContext>();
// var games = context.Games.Include(g => g.Board).Select(game => game.ToDomain()).ToList();
// var expectedResult = JsonConvert.SerializeObject(games.ToDto(),
//     new JsonSerializerSettings() {ContractResolver = new CamelCasePropertyNamesContractResolver()});
//
// var response = _client.GetAsync("/Game").Result;
// var result = response.Content.ReadAsStringAsync().Result;
//
// Assert.True(response.StatusCode == HttpStatusCode.OK);
// Assert.Equal(expectedResult, result);