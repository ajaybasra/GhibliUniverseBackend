using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Services;

public interface IVoiceActorService
{
    List<VoiceActor> GetAllVoiceActors();
    VoiceActor GetVoiceActorById(Guid voiceActorId);
    List<Film> GetFilmsByVoiceActor(Guid voiceActorId);
    public VoiceActor CreateVoiceActor(string name);
    public VoiceActor UpdateVoiceActor(Guid voiceActorId, string name);
    public void DeleteVoiceActor(Guid voiceActorId);
    public bool VoiceActorAlreadyExists(string name);
}