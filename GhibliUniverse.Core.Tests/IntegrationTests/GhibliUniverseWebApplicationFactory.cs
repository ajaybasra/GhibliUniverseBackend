using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GhibliUniverse.Core.Tests.IntegrationTests;

public class GhibliUniverseWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private GhibliUniverseContext _ghibliUniverseContext;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<GhibliUniverseContext>));

            if (descriptor != null) services.Remove(descriptor);
            
            services.AddDbContext<GhibliUniverseContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var _ghibliUniverseContext = scopedServices.GetRequiredService<GhibliUniverseContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<GhibliUniverseWebApplicationFactory<TProgram>>>();

                _ghibliUniverseContext.Database.EnsureCreated();

                try
                {
                    InitializeDbForTests(this._ghibliUniverseContext);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                                        "database with test messages. Error: {Message}", ex.Message);
                }
            }
        });
    }

    private static void InitializeDbForTests(GhibliUniverseContext db)
    {
        var voiceActor = new VoiceActor()
        {
            Id = Guid.Empty,
            Name = ValidatedString.From("John Doe")
        };
        var film = new Film()
        {
            Id = Guid.Empty,
            Title = ValidatedString.From("Spirited Away"),
            Description =
                ValidatedString.From(
                    "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts."),
            Director = ValidatedString.From("Hayao Miyazaki"),
            Composer = ValidatedString.From("Joe Hisaishi"),
            ReleaseYear = ReleaseYear.From(2001)
        };
        var review = new Review()
        {
            FilmId = Guid.Empty,
            Id = Guid.Empty,
            Rating = Rating.From(10)
        };
        db.VoiceActors.Add(voiceActor);
        db.Films.Add(film);
        db.Reviews.Add(review);
        db.SaveChanges();
    }
}