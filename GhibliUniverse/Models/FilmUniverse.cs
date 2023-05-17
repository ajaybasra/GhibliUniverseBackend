using System.Collections.Immutable;
using System.Text;

namespace GhibliUniverse;

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
        return _filmList.First(film => film.Id == filmId);
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
        
        _filmList.Add(new Film
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Director = director,
            Composer = composer,
            ReleaseYear = releaseYear,
        });
    }
    public void DeleteFilm(Guid filmId)
    {
        _filmList.RemoveAll(film => film.Id == filmId);
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
        return _voiceActorList.First(voiceActor => voiceActor.Id == voiceActorId);
    }
    public void CreateVoiceActor(string firstName, string lastName)
    {
        _voiceActorList.Add(new VoiceActor
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName
        });
        
    }

    public void DeleteVoiceActor(Guid voiceActorId)
    {
        _voiceActorList.RemoveAll(voiceActor => voiceActor.Id == voiceActorId);
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

    public FilmRating? GetFilmRatingById(Guid filmId, Guid filmRatingId)
    {//fix
        var matchingFilm =  _filmList.FirstOrDefault(film => film.Id == filmId);

        return matchingFilm?.FilmRatings.First(voiceActor => voiceActor.Id == filmRatingId);
    }
    
    public List<FilmRating> GetAllFilmRatings(Guid filmId)
    {
        return _filmList.First(film => film.Id == filmId).FilmRatings;
    }
    public void CreateFilmRating(int rating, Guid filmId)
    {
        var filmRating = new FilmRating()
        {
            Id = Guid.NewGuid(),
            Rating = rating,
            FilmId = filmId
        };
        
        _filmList.First(film => film.Id == filmId).FilmRatings.Add(filmRating);
    }
    
    public void DeleteFilmRating(Guid filmId, Guid filmRatingId)
    {
        var matchingFilm = _filmList.FirstOrDefault(film => film.Id == filmId);

        matchingFilm.FilmRatings.RemoveAll(filmRating => filmRating.Id == filmRatingId);
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
                Title = filmTitles[i],
                Description = filmDescriptions[i],
                Director = "Hayao Miyazaki",
                Composer = "Joe Hisaishi",
                ReleaseYear = releaseYears[i],
            });
 
                for (var j = 0; j < 2; j++)
                {
                    CreateVoiceActor( "John", "Doe");
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
                FirstName = "John",
                LastName = "Doe"
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
