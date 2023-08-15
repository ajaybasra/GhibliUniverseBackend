using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.DataEntities;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.IntegrationTests.TestUtils;

public static class TestDatabaseSeeder
{
    public static void SeedContext(GhibliUniverseContext context)
    {
        var voiceActors = GetSeedingVoiceActors();
        var films = GetSeedingFilms(voiceActors);
        var reviews = GetSeedingReviews();
        context.Films.AddRange(films);
        context.Reviews.AddRange(reviews);
        context.VoiceActors.AddRange(voiceActors);
        context.SaveChanges();
    }

    private static List<VoiceActorEntity> GetSeedingVoiceActors()
    {
        return new List<VoiceActorEntity>() { 
            new()
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                Name = "John Doe", 
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),  
                Name = "Test Actor"
            }
        };
    }

    private static List<FilmEntity> GetSeedingFilms(List<VoiceActorEntity> voiceActors)
    {
        return new List<FilmEntity>
        {
            new()
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                Title = "Spirited Away",
                Description =
                    "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.",
                Director = "Hayao Miyazaki",
                Composer = "Joe Hisaishi",
                ReleaseYear = 2001
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Title = "Not Spirited Away",
                Description = "This is a description..",
                Director = "Lebron",
                Composer = "MJ",
                ReleaseYear = 1995,
                VoiceActors = new List<VoiceActorEntity> {voiceActors[0]}
            }
        };
    }

    private static List<ReviewEntity> GetSeedingReviews()
    {
        return new List<ReviewEntity>
        {
            new()
            {
                FilmId = Guid.Empty,
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Rating = 10
            },
            new()
            {
                FilmId = Guid.Empty,
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Rating = 8
            }
        };
    }
}