using System.Collections.Immutable;
using System.Text;

namespace GhibliUniverse;

public class FilmList
{
    private readonly List<Film> _filmUniverse = new();
    public FilmList()
    {
        PopulateFilmsList(3);
    }
    
    public ImmutableList<Film> GetAllFilms()
    {
        return _filmUniverse.ToImmutableList();
    }
    public Film GetFilmById(Guid filmId)
    {
        return _filmUniverse.First(film => film.FilmId == filmId);
    }
    
    public void CreateFilm(Guid filmId, string title, string description, string director, string composer, int releaseYear, List<VoiceActor> voiceActors, List<FilmRating> filmRatings)
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
    public void DeleteFilm(Guid filmId)
    {
        _filmUniverse.RemoveAll(film => film.FilmId == filmId);
    }

    public string BuildFilmList()
    {
        var stringBuilder = new StringBuilder();
        foreach (var film in _filmUniverse)
        {
            stringBuilder.Append(film);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }

    public ImmutableList<VoiceActor> GetAllVoiceActors(Guid filmId)
    {
        return _filmUniverse.First(film => film.FilmId == filmId).VoiceActors.ToImmutableList();
    }

    public VoiceActor? GetVoiceActorById(Guid filmId, Guid voiceActorId)
    {//fix
        var matchingFilm =  _filmUniverse.FirstOrDefault(film => film.FilmId == filmId);

        return matchingFilm?.VoiceActors.First(voiceActor => voiceActor.VoiceActorId == voiceActorId);
    }
    public void CreateVoiceActor(Guid voiceActorId, string firstName, string lastName, Guid filmId)
    {
        var voiceActor = new VoiceActor()
        {
            VoiceActorId = voiceActorId,
            FirstName = firstName,
            LastName = lastName,
            FilmId = filmId
        };

        _filmUniverse.First(film => film.FilmId == filmId).VoiceActors.Add(voiceActor);
    }

    public void DeleteVoiceActor(Guid filmId, Guid voiceActorId)
    {//fix
        var matchingFilm = _filmUniverse.FirstOrDefault(film => film.FilmId == filmId);

        matchingFilm.VoiceActors.RemoveAll(voiceActor => voiceActor.VoiceActorId == voiceActorId);
    }
    
    public ImmutableList<FilmRating> GetAllFilmRatings(Guid filmId)
    {
        return _filmUniverse.First(film => film.FilmId == filmId).FilmRatings.ToImmutableList();
    }

    public FilmRating? GetFilmRatingById(Guid filmId, Guid filmRatingId)
    {//fix
        var matchingFilm =  _filmUniverse.FirstOrDefault(film => film.FilmId == filmId);

        return matchingFilm?.FilmRatings.First(voiceActor => voiceActor.FilmRatingId == filmRatingId);
    }
    public void CreateFilmRating(Guid voiceActorId, int rating, Guid filmId)
    {
        var filmRating = new FilmRating()
        {
            FilmRatingId = voiceActorId,
            Rating = rating,
            FilmId = filmId
        };
        
        _filmUniverse.First(film => film.FilmId == filmId).FilmRatings.Add(filmRating);
    }
    
    public void DeleteFilmRating(Guid filmId, Guid filmRatingId)
    {//fix
        var matchingFilm = _filmUniverse.FirstOrDefault(film => film.FilmId == filmId);

        matchingFilm.FilmRatings.RemoveAll(filmRating => filmRating.FilmRatingId == filmRatingId);
    }
    private void PopulateFilmsList(int numberOfFilms)
    {
        var filmTitles = new List<string> { "Spirited Away", "My Neighbor Totoro", "Ponyo" };
        var filmDescriptions = new List<string>
        {
            "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.",
            "Mei and Satsuki shift to a new house to be closer to their mother who is in the hospital. They soon become friends with Totoro, a giant rabbit-like creature who is a spirit.",
            "During a forbidden excursion to see the surface world, a goldfish princess encounters a human boy named Sosuke, who gives her the name Ponyo."
        };
        var releaseYears = new List<int> { 2001, 1988, 2008 };
        
        for (var i = 0; i < numberOfFilms; i++)
        {
            _filmUniverse.Add(new Film
            {
                FilmId = new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"),
                Title = filmTitles[i],
                Description = filmDescriptions[i],
                Director = "Hayao Miyazaki",
                Composer = "Joe Hisaishi",
                ReleaseYear = releaseYears[i],
                VoiceActors = new List<VoiceActor>(),
                FilmRatings = new List<FilmRating>()
            });

            for (var j = 0; j < 2; j++)
            {
                CreateVoiceActor(new Guid($"{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}-{i+j+i}{i+j+i}{i+j+i}{i+j+i}-{i+j+i}{i+j+i}{i+j+i}{i+j+i}-{i+j+i}{i+j+i}{i+j+i}{i+j+i}-{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}"), "John", "Doe", new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"));
                
                CreateFilmRating(new Guid($"{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}-{i+j+i}{i+j+i}{i+j+i}{i+j+i}-{i+j+i}{i+j+i}{i+j+i}{i+j+i}-{i+j+i}{i+j+i}{i+j+i}{i+j+i}-{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}{i+j+i}"), 10, new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"));
            }

        }
    }
}
