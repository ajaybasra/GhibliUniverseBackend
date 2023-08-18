using AutoMapper;
using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.DataEntities;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GhibliUniverse.Core.Repository;

public class FilmRepository : IFilmRepository
{
    private readonly GhibliUniverseContext _ghibliUniverseContext;
    private readonly IMapper _mapper;

    public FilmRepository(GhibliUniverseContext ghibliUniverseContext, IMapper mapper)
    {
        _ghibliUniverseContext = ghibliUniverseContext;
        _mapper = mapper;
    }
    public async Task<List<Film>> GetAllFilms()
    {
        var films = await _ghibliUniverseContext.Films
            .Include(f => f.Reviews)
            .Include(f => f.VoiceActors)
            .ToListAsync();
        
        return _mapper.Map<List<Film>>(films);
    }

    public async Task<Film> GetFilmById(Guid filmId)
    {
        var film = await _ghibliUniverseContext.Films.Include(f => f.Reviews).FirstOrDefaultAsync(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        return _mapper.Map<Film>(film);
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

        return _mapper.Map<List<VoiceActor>>(film.VoiceActors);
    }

    public async Task<Film> CreateFilm(Film filmCreateRequest)
    {
        var film = new FilmEntity()  
        {
            Id = Guid.NewGuid(),
            Title = filmCreateRequest.FilmInfo.Title.Value,
            Description = filmCreateRequest.FilmInfo.Description.Value,
            Director = filmCreateRequest.FilmInfo.Director.Value,
            Composer = filmCreateRequest.FilmInfo.Composer.Value,
            ReleaseYear = filmCreateRequest.FilmInfo.ReleaseYear.Value
        };

        _ghibliUniverseContext.Films.Add(film);
        await _ghibliUniverseContext.SaveChangesAsync();

        return _mapper.Map<Film>(film);
    }

    public async Task<Film> UpdateFilm(Guid filmId, Film filmUpdateRequest)
    {
        var filmToUpdate = await _ghibliUniverseContext.Films.Include(f => f.Reviews).FirstOrDefaultAsync(f => f.Id == filmId);
        if (filmToUpdate == null)
        {
            throw new ModelNotFoundException(filmId);
        }

        filmToUpdate.Title = filmUpdateRequest.FilmInfo.Title.Value;
        filmToUpdate.Description = filmUpdateRequest.FilmInfo.Description.Value;
        filmToUpdate.Director = filmUpdateRequest.FilmInfo.Director.Value;
        filmToUpdate.Composer = filmUpdateRequest.FilmInfo.Composer.Value;
        filmToUpdate.ReleaseYear = filmUpdateRequest.FilmInfo.ReleaseYear.Value;

        await _ghibliUniverseContext.SaveChangesAsync();

        return _mapper.Map<Film>(filmToUpdate);
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