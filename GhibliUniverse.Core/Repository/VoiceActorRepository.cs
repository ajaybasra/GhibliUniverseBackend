using AutoMapper;
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
    private readonly IMapper _mapper;

    public VoiceActorRepository(GhibliUniverseContext ghibliUniverseContext, IMapper mapper)
    {
        _ghibliUniverseContext = ghibliUniverseContext;
        _mapper = mapper;
    }

    public async Task<List<VoiceActor>> GetAllVoiceActors()
    {
        var voiceActors = await _ghibliUniverseContext.VoiceActors
            .Include(voiceActor => voiceActor.Films)
            .ToListAsync();
        
        return _mapper.Map<List<VoiceActor>>(voiceActors);
    }
    
    public async Task<VoiceActor> GetVoiceActorById(Guid voiceActorId)
    {
        var voiceActor = await _ghibliUniverseContext.VoiceActors.FirstOrDefaultAsync(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return _mapper.Map<VoiceActor>(voiceActor);
    }


    public async Task<List<VoiceActorFilm>> GetFilmsByVoiceActor(Guid voiceActorId)
    {
        var voiceActor = await _ghibliUniverseContext.VoiceActors
            .Include(v => v.Films)
            .ThenInclude(f => f.Reviews)
            .FirstOrDefaultAsync(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return _mapper.Map<List<VoiceActorFilm>>(voiceActor.Films);
    }

    public async Task<VoiceActor> CreateVoiceActor(VoiceActor voiceActorCreateRequest)
    {
        var voiceActor = new VoiceActorEntity()
        {
            Id = Guid.NewGuid(),
            Name = voiceActorCreateRequest.Name.Value
        };

        _ghibliUniverseContext.VoiceActors.Add(voiceActor);
        await _ghibliUniverseContext.SaveChangesAsync();

        return _mapper.Map<VoiceActor>(voiceActor);
    }

    public async Task<VoiceActor> UpdateVoiceActor(Guid voiceActorId, VoiceActor voiceActorUpdateRequest)
    {
        var voiceActorToUpdate = await _ghibliUniverseContext.VoiceActors.FirstOrDefaultAsync(v => v.Id == voiceActorId);
        if (voiceActorToUpdate == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        voiceActorToUpdate.Name = voiceActorUpdateRequest.Name.Value;

        _ghibliUniverseContext.VoiceActors.Update(voiceActorToUpdate);
        await _ghibliUniverseContext.SaveChangesAsync();

        return _mapper.Map<VoiceActor>(voiceActorToUpdate);
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