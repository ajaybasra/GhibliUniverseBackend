using System.Text;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Services;

public class FilmService : IFilmService
{
    private readonly IFilmPersistence _filmPersistence;
    private readonly IReviewPersistence _reviewPersistence;
    private readonly IVoiceActorPersistence _voiceActorPersistence;
    private readonly IFilmVoiceActorPersistence _filmVoiceActorPersistence;
    
    public FilmService(IFilmPersistence filmPersistence, IReviewPersistence reviewPersistence, IVoiceActorPersistence voiceActorPersistence, IFilmVoiceActorPersistence filmVoiceActorPersistence)
    {
        _filmPersistence = filmPersistence;
        _reviewPersistence = reviewPersistence;
        _voiceActorPersistence = voiceActorPersistence;
        _filmVoiceActorPersistence = filmVoiceActorPersistence;
    }
    
    public List<Film> GetAllFilms()
    {
        return GetFilmsWithVoiceActorsAndReviewsAdded();
    }

    public Film GetFilmById(Guid filmId)
    {
        var savedFilms = GetFilmsWithVoiceActorsAndReviewsAdded();
        var film = savedFilms.FirstOrDefault(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }
        
        return film;
    }

    public List<VoiceActor> GetVoiceActorsByFilm(Guid filmId)
    {
        var savedFilms = GetFilmsWithVoiceActorsAndReviewsAdded();
        var film = savedFilms.FirstOrDefault(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        return film.VoiceActors;
    }

    public void CreateFilm(string title, string description, string director, string composer, int releaseYear)
    {
        var savedFilms = _filmPersistence.ReadFilms();
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
            savedFilms.Add(film);
            _filmPersistence.WriteFilms(savedFilms);
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

    public void UpdateFilm(Guid filmId, Film updatedFilm) // look at
    {
        var savedFilms = _filmPersistence.ReadFilms();
        var filmToUpdate = savedFilms.FirstOrDefault(f => f.Id == filmId);
        if (filmToUpdate == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        filmToUpdate.Title = updatedFilm.Title;
        filmToUpdate.Description = updatedFilm.Description;
        filmToUpdate.Director = updatedFilm.Director;
        filmToUpdate.Composer = updatedFilm.Composer;
        filmToUpdate.ReleaseYear = updatedFilm.ReleaseYear;
        
        _filmPersistence.WriteFilms(savedFilms);
    }

    public void DeleteFilm(Guid filmId)
    {
        var savedFilms = _filmPersistence.ReadFilms();
        var savedReviews = _reviewPersistence.ReadReviews();
        var filmToBeDeleted = savedFilms.FirstOrDefault(f => f.Id == filmId);
        savedReviews.RemoveAll(review => filmToBeDeleted.Reviews.Contains(review));
        if (filmToBeDeleted == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        filmToBeDeleted.VoiceActors.ForEach(v => v.RemoveFilm(filmToBeDeleted));
        savedFilms.Remove(filmToBeDeleted);
        _filmPersistence.WriteFilms(savedFilms);
        _reviewPersistence.WriteReviews(savedReviews);
    }
    
    public void AddFilm(Film film) 
    {
        var savedFilms = _filmPersistence.ReadFilms();
        savedFilms.Add(film);
        _filmPersistence.WriteFilms(savedFilms);
    }

    public void AddVoiceActor(Guid filmId, VoiceActor voiceActor)
    {
        var savedFilms = _filmPersistence.ReadFilms();
        var filmToAddVoiceActor = savedFilms.FirstOrDefault(f => f.Id == filmId);
        if (filmToAddVoiceActor == null)
        {
            throw new ModelNotFoundException(filmId);
        }
        filmToAddVoiceActor.AddVoiceActor(voiceActor);
        _filmVoiceActorPersistence.WriteFilmVoiceActors(savedFilms);
    }

    private List<Film> GetFilmsWithVoiceActorsAndReviewsAdded()
    {
        var savedFilms = _filmPersistence.ReadFilms();
        var savedReviews = _reviewPersistence.ReadReviews();
        var savedFilmVoiceActorIds = _filmVoiceActorPersistence.ReadFilmVoiceActorData();
        foreach ((Guid filmId, Guid voiceActorId) in savedFilmVoiceActorIds)
        {
            var filmToHaveVoiceActorAdded = savedFilms.FirstOrDefault(f => f.Id == filmId);
            if (filmToHaveVoiceActorAdded == null)
            {
                throw new ModelNotFoundException(filmId);
            }

            var voiceActorToAdd = _voiceActorPersistence.ReadVoiceActors().FirstOrDefault(v => v.Id == voiceActorId);
            if (voiceActorToAdd == null)
            {
                throw new ModelNotFoundException(voiceActorId);
            }
            filmToHaveVoiceActorAdded.VoiceActors.Add(voiceActorToAdd);
        }
        foreach (var review in savedReviews)
        {
            var filmToAddReview = savedFilms.FirstOrDefault(f => f.Id == review.FilmId);
            if (filmToAddReview == null)
            {
                throw new ModelNotFoundException(review.FilmId);
            }
            filmToAddReview.Reviews.Add(review);
        }
        _filmVoiceActorPersistence.WriteFilmVoiceActors(savedFilms);
        return savedFilms;

    }
    
    public void PopulateFilmsList(int numberOfFilms)
    {
        var films = GetFilmsWithVoiceActorsAndReviewsAdded();
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
            films.Add(new Film
            {
                Id = new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"),
                Title = ValidatedString.From(filmTitles[i]),
                Description = ValidatedString.From(filmDescriptions[i]),
                Director = ValidatedString.From("Hayao Miyazaki"),
                Composer = ValidatedString.From("Joe Hisaishi"),
                ReleaseYear = ReleaseYear.From(releaseYears[i]),
            });
        }
        _filmPersistence.WriteFilms(films);
        _filmVoiceActorPersistence.WriteFilmVoiceActors(films);
    }
    
    public string BuildFilmList()
    {
        var films = GetFilmsWithVoiceActorsAndReviewsAdded();
        var stringBuilder = new StringBuilder();
        foreach (var film in films)
        {
            stringBuilder.Append(film);
            stringBuilder.Append('\n');
        }
        
        return stringBuilder.ToString();
    }
    
}