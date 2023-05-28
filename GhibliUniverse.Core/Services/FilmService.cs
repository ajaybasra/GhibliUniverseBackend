using System.Text;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Services;

public class FilmService : IFilmService
{
    private readonly List<Film> _filmList = new();
    
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

    public List<VoiceActor> GetVoiceActorsByFilm(Guid filmId)
    {
        var film = _filmList.FirstOrDefault(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        return film.VoiceActors;
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

    public void UpdateFilm(Guid filmId, Film updatedFilm)
    {
        var filmToUpdate = GetFilmById(filmId);

        filmToUpdate.Title = updatedFilm.Title;
        filmToUpdate.Description = updatedFilm.Description;
        filmToUpdate.Director = updatedFilm.Director;
        filmToUpdate.Composer = updatedFilm.Composer;
        filmToUpdate.ReleaseYear = updatedFilm.ReleaseYear;
    }

    public void DeleteFilm(Guid filmId)
    {
        var film = _filmList.FirstOrDefault(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        film.VoiceActors.ForEach(v => v.RemoveFilm(film));
        _filmList.Remove(film);
    }
    
    public void AddFilm(Film film) //ask
    {
        _filmList.Add(film);
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
 
            // for (var j = 0; j < 2; j++)
            // {
            //     var va = new VoiceActor(
            //     {
            //         Name = ValidatedString.From("John Doe")
            //     };
            //     var latestAddedFilm = _filmList.Last();
            //     latestAddedFilm.AddVoiceActor(va);
            //         
            //     CreateFilmRating( 10, new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"));
            // }
        }
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
}