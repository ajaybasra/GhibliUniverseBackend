using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GhibliUniverse.Core.Repository;

public class FilmRepository : IFilmRepository
{
    private readonly GhibliUniverseContext _ghibliUniverseContext;

    public FilmRepository(GhibliUniverseContext ghibliUniverseContext)
    {
        _ghibliUniverseContext = ghibliUniverseContext;
    }
    
    public List<Film> GetAllFilms()
    {
        return _ghibliUniverseContext.Films
            .Include(f => f.Reviews)
            .Include(f => f.VoiceActors)
            .ToList();
    }

    public Film GetFilmById(Guid filmId)
    {
        var film = _ghibliUniverseContext.Films.FirstOrDefault(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        return film;
    }

    public List<VoiceActor> GetVoiceActorsByFilm(Guid filmId)
    {
        var film = _ghibliUniverseContext.Films
            .Include(f => f.VoiceActors)
            .FirstOrDefault(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        return film.VoiceActors;
    }

    public Film CreateFilm(string title, string description, string director, string composer, int releaseYear)
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
        
        _ghibliUniverseContext.Films.Add(film);
        _ghibliUniverseContext.SaveChanges();
        
        return film;
    }

    public Film UpdateFilm(Guid filmId, Film updatedFilm)
    {
        var filmToUpdate = _ghibliUniverseContext.Films.FirstOrDefault(f => f.Id == filmId);
        if (filmToUpdate == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        filmToUpdate.Title = updatedFilm.Title;
        filmToUpdate.Description = updatedFilm.Description;
        filmToUpdate.Director = updatedFilm.Director;
        filmToUpdate.Composer = updatedFilm.Composer;
        filmToUpdate.ReleaseYear = updatedFilm.ReleaseYear;

        _ghibliUniverseContext.Films.Update(filmToUpdate);
        _ghibliUniverseContext.SaveChanges();
        
        return filmToUpdate;
    }

    public void DeleteFilm(Guid filmId)
    {
        var filmToDelete = _ghibliUniverseContext.Films.FirstOrDefault(f => f.Id == filmId);
        if (filmToDelete == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        _ghibliUniverseContext.Films.Remove(filmToDelete);
        _ghibliUniverseContext.SaveChanges();
    }

    public void LinkVoiceActor(Guid filmId, Guid voiceActorId)
    {
        var filmToAddVoiceActor = _ghibliUniverseContext.Films.FirstOrDefault(f => f.Id == filmId);
        if (filmToAddVoiceActor == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        var voiceActorToLink = _ghibliUniverseContext.VoiceActors.FirstOrDefault(v => v.Id == voiceActorId);
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
            _ghibliUniverseContext.SaveChanges();
        }
    }

    public void UnlinkVoiceActor(Guid filmId, Guid voiceActorId)
    {
        var filmToHaveVoiceActorRemoved = _ghibliUniverseContext.Films.FirstOrDefault(f => f.Id == filmId);
        if (filmToHaveVoiceActorRemoved == null)
        {
            throw new ModelNotFoundException(filmId);
        }
        
        var voiceActorToUnlink = _ghibliUniverseContext.VoiceActors.FirstOrDefault(v => v.Id == voiceActorId);
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

            _ghibliUniverseContext.SaveChanges();
        }
    }

}