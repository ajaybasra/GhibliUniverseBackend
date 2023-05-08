using System.Collections.Immutable;
using System.Text;

namespace GhibliUniverse;

public class FilmList
{
    private readonly List<Film> _filmUniverse;

    public FilmList()
    {
        _filmUniverse = new List<Film>
        {
            new()
            {
                FilmId = new Guid("11111111-1111-1111-1111-111111111111"),
                Title = "Spirited Away",
                Description = "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.",
                Director = "Hayao Miyazaki",
                Composer = "Joe Hisaishi",
                ReleaseYear = 2001,
                VoiceActors = new List<VoiceActor>
                {
                    new()
                    {
                        VoiceActorId = new Guid("11111111-1111-1111-1111-111111111111"), 
                        FirstName = "John",
                        LastName = "Cena",
                        FilmId = new Guid("11111111-1111-1111-1111-111111111111")
                    }
                },
                FilmRatings = new List<FilmRating>
                {
                    new()
                    {
                        FilmRatingId = new Guid("11111111-1111-1111-1111-111111111111"),
                        Score = 10,
                        FilmId = new Guid("11111111-1111-1111-1111-111111111111")
                    }
                }
            }
        };
    }

    public void Add(Guid filmId, string title, string description, string director, string composer, int releaseYear, List<VoiceActor> voiceActors, List<FilmRating> filmRatings)
    {
        _filmUniverse.Add(new Film
        {
            FilmId = filmId,
            Title = title,
            Description = description,
            Director = director,
            Composer = composer,
            ReleaseYear = releaseYear,
            VoiceActors = voiceActors,
            FilmRatings = filmRatings
        });
    }

    public void Remove(Guid filmId)
    {
        _filmUniverse.RemoveAll(film => film.FilmId == filmId);
    }

    public Film GetFilmById(Guid filmId)
    {
        return _filmUniverse.First(film => film.FilmId == filmId);
    }

    public ImmutableList<Film> GetAllFilms()
    {
        return _filmUniverse.ToImmutableList();
    }

    public string BuildFilmUniverse()
    {
        var stringBuilder = new StringBuilder();
        foreach (var film in _filmUniverse)
        {
            stringBuilder.Append(film);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }
    
}
