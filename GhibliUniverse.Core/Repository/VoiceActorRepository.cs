using GhibliUniverse.Core.Context;
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

    public async Task<List<VoiceActor>> GetAllVoiceActorsAsync()
    {
        return await _ghibliUniverseContext.VoiceActors
            .Include(voiceActor => voiceActor.Films)
            .ToListAsync();
    }
    
    public async Task<VoiceActor> GetVoiceActorByIdAsync(Guid voiceActorId)
    {
        var voiceActor = await _ghibliUniverseContext.VoiceActors.FirstOrDefaultAsync(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return voiceActor;
    }


    public async Task<List<Film>> GetFilmsByVoiceActorAsync(Guid voiceActorId)
    {
        var voiceActor = await _ghibliUniverseContext.VoiceActors
            .Include(v => v.Films)
            .ThenInclude(f => f.Reviews)
            .FirstOrDefaultAsync(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return voiceActor.Films;
    }

    public async Task<VoiceActor> CreateVoiceActorAsync(string name)
    {
        var voiceActor = new VoiceActor
        {
            Id = Guid.NewGuid(),
            Name = ValidatedString.From(name)
        };

        _ghibliUniverseContext.VoiceActors.Add(voiceActor);
        await _ghibliUniverseContext.SaveChangesAsync();

        return voiceActor;
    }

    public async Task<VoiceActor> UpdateVoiceActorAsync(Guid voiceActorId, string name)
    {
        var voiceActorToUpdate = await _ghibliUniverseContext.VoiceActors.FirstOrDefaultAsync(v => v.Id == voiceActorId);
        if (voiceActorToUpdate == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        voiceActorToUpdate.Name = ValidatedString.From(name);

        _ghibliUniverseContext.VoiceActors.Update(voiceActorToUpdate);
        await _ghibliUniverseContext.SaveChangesAsync();

        return voiceActorToUpdate;
    }


    public async Task DeleteVoiceActorAsync(Guid voiceActorId)
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