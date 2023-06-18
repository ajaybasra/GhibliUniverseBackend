using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Repository;

public class VoiceActorRepository : IVoiceActorRepository
{
    private readonly GhibliUniverseContext _ghibliUniverseContext;

    public VoiceActorRepository(GhibliUniverseContext ghibliUniverseContext)
    {
        _ghibliUniverseContext = ghibliUniverseContext;
    }

    public List<VoiceActor> GetAllVoiceActors()
    {
        return _ghibliUniverseContext.VoiceActors.ToList();
    }

    public VoiceActor GetVoiceActorById(Guid voiceActorId)
    {
        var voiceActor = _ghibliUniverseContext.VoiceActors.FirstOrDefault(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return voiceActor;
    }

    public List<Film> GetFilmsByVoiceActor(Guid voiceActorId)
    {
        var voiceActor = _ghibliUniverseContext.VoiceActors.FirstOrDefault(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return voiceActor.Films;
    }

    public VoiceActor CreateVoiceActor(string name)
    {
        var voiceActor = new VoiceActor
        {
            Id = Guid.NewGuid(),
            Name = ValidatedString.From(name)
        };

        _ghibliUniverseContext.VoiceActors.Add(voiceActor);
        _ghibliUniverseContext.SaveChanges();

        return voiceActor;
    }

    public VoiceActor UpdateVoiceActor(Guid voiceActorId, string name)
    {
        var voiceActorToUpdate = _ghibliUniverseContext.VoiceActors.FirstOrDefault(v => v.Id == voiceActorId);
        if (voiceActorToUpdate == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        voiceActorToUpdate.Name = ValidatedString.From(name);

        _ghibliUniverseContext.VoiceActors.Update(voiceActorToUpdate);
        _ghibliUniverseContext.SaveChanges();

        return voiceActorToUpdate;
    }

    public void DeleteVoiceActor(Guid voiceActorId)
    {
        var voiceActorToDelete = _ghibliUniverseContext.VoiceActors.FirstOrDefault(v => v.Id == voiceActorId);
        if (voiceActorToDelete == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        _ghibliUniverseContext.VoiceActors.Remove(voiceActorToDelete);
        _ghibliUniverseContext.SaveChanges();
    }
    
}