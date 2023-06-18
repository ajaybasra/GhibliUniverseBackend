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

    public Film CreateFilm(string title, string description, string director, string composer, int releaseYear)
    {
        var savedFilms = _filmPersistence.ReadFilms();
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
        
        return film;
    }

    public Film UpdateFilm(Guid filmId, Film updatedFilm)  
    {
        var savedFilms = GetFilmsWithVoiceActorsAndReviewsAdded();
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
        return filmToUpdate;
    }

    public void DeleteFilm(Guid filmId)
    {
        var savedFilms = GetFilmsWithVoiceActorsAndReviewsAdded();
        var savedReviews = _reviewPersistence.ReadReviews();
        
        var filmToBeDeleted = savedFilms.FirstOrDefault(f => f.Id == filmId);
        if (filmToBeDeleted == null)
        {
            throw new ModelNotFoundException(filmId);
        }
        savedReviews.RemoveAll(review => filmToBeDeleted.Reviews.Contains(review));

        foreach (var voiceActor in filmToBeDeleted.VoiceActors)
        {
            var filmIdList = voiceActor.Films.Select(f => f.Id).ToList();
            if (filmIdList.Contains(filmToBeDeleted.Id))
            {
                voiceActor.Films.RemoveAll(f => f.Id == filmToBeDeleted.Id);
            }
        }
        savedFilms.RemoveAll(f => f.Id == filmToBeDeleted.Id);
        _filmPersistence.WriteFilms(savedFilms);
        _reviewPersistence.WriteReviews(savedReviews);
        _filmVoiceActorPersistence.WriteFilmVoiceActors(savedFilms);
    }
    
  

    public void LinkVoiceActor(Guid filmId, Guid voiceActorId)  
    {
        var savedFilms = GetFilmsWithVoiceActorsAndReviewsAdded();
        var filmToAddVoiceActor = savedFilms.FirstOrDefault(f => f.Id == filmId);
        if (filmToAddVoiceActor == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        var voiceActorToLink = _voiceActorPersistence.ReadVoiceActors().FirstOrDefault(v => v.Id == voiceActorId);
        if (voiceActorToLink == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }
        var voiceActorsIdList = filmToAddVoiceActor.VoiceActors.Select(v => v.Id).ToList();

        if (!voiceActorsIdList.Contains(voiceActorId))
        {
            filmToAddVoiceActor.VoiceActors.Add(voiceActorToLink);
            var filmIdList = voiceActorToLink.Films.Select(f => f.Id).ToList();
            if (!filmIdList.Contains(filmToAddVoiceActor.Id))
            {
                voiceActorToLink.Films.Add(filmToAddVoiceActor);
            }
        }
        _filmVoiceActorPersistence.WriteFilmVoiceActors(savedFilms);
    }

    public void UnlinkVoiceActor(Guid filmId, Guid voiceActorId)
    {
        var savedFilms = GetFilmsWithVoiceActorsAndReviewsAdded();
        var filmToHaveVoiceActorRemoved = savedFilms.FirstOrDefault(f => f.Id == filmId);
        if (filmToHaveVoiceActorRemoved == null)
        {
            throw new ModelNotFoundException(filmId);
        }
        
        var voiceActorToUnlink = _voiceActorPersistence.ReadVoiceActors().FirstOrDefault(v => v.Id == voiceActorId);
        if (voiceActorToUnlink == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }
        
        var voiceActorIdList = filmToHaveVoiceActorRemoved.VoiceActors.Select(v => v.Id).ToList();
        if (voiceActorIdList.Contains(voiceActorId))
        {
            filmToHaveVoiceActorRemoved.VoiceActors.RemoveAll(v => v.Id == voiceActorId);
            var filmIdList = voiceActorToUnlink.Films.Select(f => f.Id).ToList();
            if (filmIdList.Contains(filmToHaveVoiceActorRemoved.Id))
            {
                voiceActorToUnlink.Films.RemoveAll(f => f.Id == filmToHaveVoiceActorRemoved.Id);
            }
        }
        _filmVoiceActorPersistence.WriteFilmVoiceActors(savedFilms);
    }

    public bool FilmIdAlreadyExists(Guid filmId)
    {
        var filmsWithMatchingId = GetAllFilms().FirstOrDefault(f => f.Id == filmId);
        return filmsWithMatchingId != null;
    }

    public bool FilmTitleAlreadyExists(string title)
    {
        var filmsWithMatchingTitle = GetAllFilms().FirstOrDefault(f => f.Title == ValidatedString.From(title));
        return filmsWithMatchingTitle != null;
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