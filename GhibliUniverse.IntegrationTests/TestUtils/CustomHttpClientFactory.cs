using GhibliUniverse.Core.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GhibliUniverse.IntegrationTests.TestUtils;

public static class CustomHttpClientFactory
{
    public static HttpClient Create(DbContextOptions<GhibliUniverseContext> dbContextOptions)
    {
        var factory = new WebApplicationFactory<Program>();

        var customFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(_ => dbContextOptions);
            });
        });

        return customFactory.CreateClient();
    }
}