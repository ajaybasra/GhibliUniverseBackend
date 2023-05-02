using System.Collections.Immutable;
using System.Text;

namespace GhibliUniverse;

public class FilmUniverse
{
    private readonly List<Film> _filmUniverse;

    public FilmUniverse()
    {
        _filmUniverse = new List<Film>
        {
            new()
            {
                FilmId = 1,
                Title = "Spirited Away",
                Description = "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.",
                Director = "Hayao Miyazaki",
                Composer = "Joe Hisaishi",
                ReleaseYear = 2001,
                Score = 10
            }
        };
    }

    public void Add(int filmId, string title, string description, string director, string composer, int releaseYear,
        int score)
    {
        _filmUniverse.Add(new Film
        {
            FilmId = filmId,
            Title = title,
            Description = description,
            Director = director,
            Composer = composer,
            ReleaseYear = releaseYear,
            Score = score
        });
    }

    public void Remove(int filmId)
    {
        foreach (var film in _filmUniverse.ToList().Where(film => film.FilmId == filmId)) // what
        {
            _filmUniverse.Remove(film);
        }
    }

    public Film GetFilmById(int filmId)
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
