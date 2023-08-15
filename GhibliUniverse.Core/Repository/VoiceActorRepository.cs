using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.DataEntities;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GhibliUniverse.Core.Repository;

public class VoiceActorRepository : IVoiceActorRepository
{
    private readonly GhibliUniverseContext _ghibliUniverseContext;

    public VoiceActorRepository(GhibliUniverseContext ghibliUniverseContext)
    {
        _ghibliUniverseContext = ghibliUniverseContext;
    }

    public async Task<List<VoiceActor>> GetAllVoiceActors()
    {
        var voiceActors = await _ghibliUniverseContext.VoiceActors
            .Include(voiceActor => voiceActor.Films)
            .ToListAsync();
        return voiceActors.Select(v => new VoiceActor(v)).ToList();
    }
    
    public async Task<VoiceActor> GetVoiceActorById(Guid voiceActorId)
    {
        var voiceActor = await _ghibliUniverseContext.VoiceActors.FirstOrDefaultAsync(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return new VoiceActor(voiceActor);
    }


    public async Task<List<Film>> GetFilmsByVoiceActor(Guid voiceActorId)
    {
        var voiceActor = await _ghibliUniverseContext.VoiceActors
            .Include(v => v.Films)
            .ThenInclude(f => f.Reviews)
            .FirstOrDefaultAsync(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return voiceActor.Films.Select(f => new Film(f)).ToList();
    }

    public async Task<VoiceActor> CreateVoiceActor(string name)
    {
        var voiceActor = new VoiceActorEntity()
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        _ghibliUniverseContext.VoiceActors.Add(voiceActor);
        await _ghibliUniverseContext.SaveChangesAsync();

        return new VoiceActor(voiceActor);
    }

    public async Task<VoiceActor> UpdateVoiceActor(Guid voiceActorId, string name)
    {
        var voiceActorToUpdate = await _ghibliUniverseContext.VoiceActors.FirstOrDefaultAsync(v => v.Id == voiceActorId);
        if (voiceActorToUpdate == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        voiceActorToUpdate.Name = name;

        _ghibliUniverseContext.VoiceActors.Update(voiceActorToUpdate);
        await _ghibliUniverseContext.SaveChangesAsync();

        return new VoiceActor(voiceActorToUpdate);
    }


    public async Task DeleteVoiceActor(Guid voiceActorId)
    {
        var voiceActorToDelete = await _ghibliUniverseContext.VoiceActors.FirstOrDefaultAsync(v => v.Id == voiceActorId);
        if (voiceActorToDelete == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        _ghibliUniverseContext.VoiceActors.Remove(voiceActorToDelete);
        await _ghibliUniverseContext.SaveChangesAsync();
    }
    
}