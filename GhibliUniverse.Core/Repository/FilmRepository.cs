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
    public async Task<List<Film>> GetAllFilms()
    {
        return await _ghibliUniverseContext.Films
            .Include(f => f.Reviews)
            .Include(f => f.VoiceActors)
            .ToListAsync();
    }

    public async Task<Film> GetFilmById(Guid filmId)
    {
        var film = await _ghibliUniverseContext.Films.FirstOrDefaultAsync(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        return film;
    }

    public async Task<List<VoiceActor>> GetVoiceActorsByFilm(Guid filmId)
    {
        var film = await _ghibliUniverseContext.Films
            .Include(f => f.VoiceActors)
            .FirstOrDefaultAsync(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        return film.VoiceActors;
    }

    public async Task<Film> CreateFilm(string title, string description, string director, string composer, int releaseYear)
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
        await _ghibliUniverseContext.SaveChangesAsync();

        return film;
    }

    public async Task<Film> UpdateFilm(Guid filmId, Film updatedFilm)
    {
        var filmToUpdate = await _ghibliUniverseContext.Films.FirstOrDefaultAsync(f => f.Id == filmId);
        if (filmToUpdate == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        filmToUpdate.Title = updatedFilm.Title;
        filmToUpdate.Description = updatedFilm.Description;
        filmToUpdate.Director = updatedFilm.Director;
        filmToUpdate.Composer = updatedFilm.Composer;
        filmToUpdate.ReleaseYear = updatedFilm.ReleaseYear;

        await _ghibliUniverseContext.SaveChangesAsync();

        return filmToUpdate;
    }

    public async Task DeleteFilm(Guid filmId)
    {
        var filmToDelete = await _ghibliUniverseContext.Films.FirstOrDefaultAsync(f => f.Id == filmId);
        if (filmToDelete == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        _ghibliUniverseContext.Films.Remove(filmToDelete);
        await _ghibliUniverseContext.SaveChangesAsync();
    }

    public async Task LinkVoiceActor(Guid filmId, Guid voiceActorId)
    {
        var filmToAddVoiceActor = await _ghibliUniverseContext.Films
            .Include(f => f.VoiceActors)
            .FirstOrDefaultAsync(f => f.Id == filmId);
        if (filmToAddVoiceActor == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        var voiceActorToLink = await _ghibliUniverseContext.VoiceActors
            .Include(v => v.Films)
            .FirstOrDefaultAsync(v => v.Id == voiceActorId);
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
            await _ghibliUniverseContext.SaveChangesAsync();
        }
    }

    public async Task UnlinkVoiceActor(Guid filmId, Guid voiceActorId)
    {
        var filmToHaveVoiceActorRemoved = await _ghibliUniverseContext.Films
            .Include(f => f.VoiceActors)
            .FirstOrDefaultAsync(f => f.Id == filmId);
        if (filmToHaveVoiceActorRemoved == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        var voiceActorToUnlink = await _ghibliUniverseContext.VoiceActors
            .Include(v => v.Films)
            .FirstOrDefaultAsync(v => v.Id == voiceActorId);
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

            await _ghibliUniverseContext.SaveChangesAsync();
        }
    }
}