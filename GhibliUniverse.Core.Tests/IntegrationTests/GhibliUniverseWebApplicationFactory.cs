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
                
                InitializeDbForTests(_ghibliUniverseContext).Wait();
            }
        });
    }

    private async Task InitializeDbForTests(GhibliUniverseContext ghibliUniverseContext)
    {
        await ghibliUniverseContext.Database.EnsureDeletedAsync();
        await ghibliUniverseContext.Database.EnsureCreatedAsync();
        var voiceActor = new VoiceActor()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
            Name = ValidatedString.From("John Doe")

        };
        var voiceActorTwo = new VoiceActor()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), // cheeky
            Name = ValidatedString.From("Test Actor")
        };
        var voiceActors = new List<VoiceActor> { voiceActor, voiceActorTwo };
        var filmOne = new Film()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
            Title = ValidatedString.From("Spirited Away"),
            Description =
                ValidatedString.From(
                    "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts."),
            Director = ValidatedString.From("Hayao Miyazaki"),
            Composer = ValidatedString.From("Joe Hisaishi"),
            ReleaseYear = ReleaseYear.From(2001)
        };
        var filmTwo = new Film()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Title = ValidatedString.From("Not Spirited Away"),
            Description =
                ValidatedString.From(
                    "This is a description.."),
            Director = ValidatedString.From("Lebron"),
            Composer = ValidatedString.From("MJ"),
            ReleaseYear = ReleaseYear.From(1995),
            VoiceActors = new List<VoiceActor> {voiceActor}
        };
        var films = new List<Film> { filmOne, filmTwo };
        var review = new Review()
        {
            FilmId = Guid.Empty,
            Id = Guid.Empty,
            Rating = Rating.From(10)
        };

        ghibliUniverseContext.VoiceActors.AddRange(voiceActors);
        ghibliUniverseContext.Films.AddRange(films);
        ghibliUniverseContext.Reviews.Add(review);
        await ghibliUniverseContext.SaveChangesAsync();
    }
}
