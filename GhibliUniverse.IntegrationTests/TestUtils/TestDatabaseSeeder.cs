using GhibliUniverse.Core.Context;
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

    private static List<VoiceActor> GetSeedingVoiceActors()
    {
        return new List<VoiceActor>() { 
            new()
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                Name = ValidatedString.From("John Doe"), 
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),  
                Name = ValidatedString.From("Test Actor")
            }
        };
    }

    private static List<Film> GetSeedingFilms(List<VoiceActor> voiceActors)
    {
        return new List<Film>
        {
            new()
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                Title = ValidatedString.From("Spirited Away"),
                Description =
                    ValidatedString.From(
                        "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts."),
                Director = ValidatedString.From("Hayao Miyazaki"),
                Composer = ValidatedString.From("Joe Hisaishi"),
                ReleaseYear = ReleaseYear.From(2001)
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Title = ValidatedString.From("Not Spirited Away"),
                Description =
                    ValidatedString.From(
                        "This is a description.."),
                Director = ValidatedString.From("Lebron"),
                Composer = ValidatedString.From("MJ"),
                ReleaseYear = ReleaseYear.From(1995),
                VoiceActors = new List<VoiceActor> {voiceActors[0]}
            }
        };
    }

    private static List<Review> GetSeedingReviews()
    {
        return new List<Review>
        {
            new()
            {
                FilmId = Guid.Empty,
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Rating = Rating.From(10)
            },
            new()
            {
                FilmId = Guid.Empty,
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Rating = Rating.From(8)
            }
        };
    }
}