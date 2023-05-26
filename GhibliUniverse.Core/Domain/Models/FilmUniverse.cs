using System.Text;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.Models.ValueObjects;

namespace GhibliUniverse.Core.Domain.Models;

public class FilmUniverse
{
    private readonly List<Film> _filmList = new();
    private readonly List<VoiceActor> _voiceActorList = new();
    public List<Film> GetAllFilms()
    {
        return _filmList;
    }
    public Film GetFilmById(Guid filmId)
    {
        var film = _filmList.FirstOrDefault(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }
        
        return film;
    }

    public List<Film> GetFilmsFilteredByProperty(string propertyName, string filterValue)
    {
        var propInfo = typeof(Film).GetProperty(propertyName);
        if (propInfo == null) // property not found
        {
            throw new ArgumentException("That property does not exist.");
        }
        // film => propInfo.GetValue(film)!.Equals(parsedFilterValue)
        return _filmList.Where(film => CheckFilmProperty(propertyName, propInfo.GetValue(film)!, filterValue)).ToList();
    }
    
    private bool CheckFilmProperty(string propertyName, Object propertyValue, string filterValue)
    {
        return propertyName switch
        {
            "Title" => propertyValue.ToString() == filterValue,
            "Description" => propertyValue.ToString() == filterValue,
            "Director" => propertyValue.ToString() == filterValue,
            "Composer" => propertyValue.ToString() == filterValue,
            "ReleaseYear" => int.Parse(propertyValue.ToString() ?? string.Empty) == int.Parse(filterValue),
            _ => false
        };
    }
    public void CreateFilm(string title, string description, string director, string composer, int releaseYear)
    {
        try
        {
            var film = new Film
            {
                Id = Guid.NewGuid(),
                Title = ValidatedString.From(title),
                Description = ValidatedString.From(description),
                Director = ValidatedString.From(director),
                Composer = ValidatedString.From(composer),
                ReleaseYear = ReleaseYear.From(releaseYear)
            };
            _filmList.Add(film);
        }
        catch (ReleaseYear.NotFourCharactersException e)
        {
            Console.WriteLine(e);
        }
        catch (ReleaseYear.ReleaseYearLessThanOldestReleaseYearException e)
        {
            Console.WriteLine(e);
        }
        catch (ArgumentException ae)
        {
            Console.WriteLine(ae);
        }
    }
    public void DeleteFilm(Guid filmId)
    {
        var film = _filmList.FirstOrDefault(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }
        _voiceActorList.ForEach(v => v.RemoveFilm(film));
        _filmList.Remove(film);
    }

    public string BuildFilmList()
    {
        var stringBuilder = new StringBuilder();
        foreach (var film in _filmList)
        {
            stringBuilder.Append(film);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }

    public List<VoiceActor> GetAllVoiceActors()
    {
        return _voiceActorList;
    }

    public VoiceActor GetVoiceActorById(Guid voiceActorId)
    {
        var voiceActor = _voiceActorList.FirstOrDefault(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return voiceActor;
    }
    public void CreateVoiceActor(string name)
    {
        try
        {
            var voiceActor = new VoiceActor
            {
                Id = Guid.NewGuid(),
                Name = ValidatedString.From(name)
            };
            _voiceActorList.Add(voiceActor);
        }
        catch (ArgumentException ae)
        {
            Console.WriteLine(ae);
        }
        
    }

    public void DeleteVoiceActor(Guid voiceActorId)
    {
        var voiceActor = _voiceActorList.FirstOrDefault(f => f.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }
        _filmList.ForEach(f => f.RemoveVoiceActor(voiceActor));
        _voiceActorList.Remove(voiceActor);

    }

    public string BuildVoiceActorList()
    {
        var stringBuilder = new StringBuilder();
        foreach (var voiceActor in _voiceActorList)
        {
            stringBuilder.Append(voiceActor);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }
    
    public List<FilmRating> GetAllFilmRatings(Guid filmId)
    {
        return _filmList.First(f => f.Id == filmId).FilmRatings;
    }
    
    public FilmRating GetFilmRatingById(Guid filmId, Guid filmRatingId)
    {
        var matchingFilm =  _filmList.FirstOrDefault(f => f.Id == filmId);
        if (matchingFilm == null)
        {
            throw new ModelNotFoundException(filmId);
        }
        var filmRating = matchingFilm.FilmRatings.FirstOrDefault(r => r.Id == filmRatingId);
        if (filmRating == null)
        {
            throw new ModelNotFoundException(filmRatingId);
        }

        return filmRating;
    }
    
    public void CreateFilmRating(int rating, Guid filmId)
    {
        try
        {
            var filmRating = new FilmRating
            {
                Id = Guid.NewGuid(),
                Rating = Rating.From(rating),
                FilmId = filmId
            };
            var film = GetFilmById(filmId);
            film.FilmRatings.Add(filmRating);
        }
        catch (ModelNotFoundException e)
        {
            Console.WriteLine(e);
        }
        catch (Rating.RatingOutOfRangeException e)
        {
            Console.WriteLine(e);
        }
    }
    
    public void DeleteFilmRating(Guid filmId, Guid filmRatingId)
    {
        try
        {
            var matchingFilm = GetFilmById(filmId);
            var filmRating = GetFilmRatingById(filmId, filmRatingId);
            matchingFilm.FilmRatings.Remove(filmRating);
        }
        catch (ModelNotFoundException e)
        {
            Console.WriteLine(e);
        }

    }
    public void PopulateFilmsList(int numberOfFilms)
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
            _filmList.Add(new Film
            {
                Id = new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"),
                Title = ValidatedString.From(filmTitles[i]),
                Description = ValidatedString.From(filmDescriptions[i]),
                Director = ValidatedString.From("Hayao Miyazaki"),
                Composer = ValidatedString.From("Joe Hisaishi"),
                ReleaseYear = ReleaseYear.From(releaseYears[i]),
            });
 
                for (var j = 0; j < 2; j++)
                {
                    CreateVoiceActor( "John Doe");
                    _filmList.Last().AddVoiceActor(_voiceActorList.Last());
                    
                    CreateFilmRating( 10, new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"));
                }

        }
    }

    public void PopulateVoiceActorsList(int numberOfVoiceActors)
    {
        for (var i = 0; i < numberOfVoiceActors; i++)
        {
            _voiceActorList.Add(new VoiceActor
            {
                Id = new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"),
                Name = ValidatedString.From("John Doe")
            });
        }
    }
    public void AddFilm(Film film)
    {
        _filmList.Add(film);
    }

    public void AddVoiceActor(VoiceActor voiceActor)
    {
        _voiceActorList.Add(voiceActor);
    }
}
