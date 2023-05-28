using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Services;

public interface IVoiceActorService
{
    List<VoiceActor> GetAllVoiceActors();
    VoiceActor GetVoiceActorById(Guid voiceActorId);
    List<Film> GetFilmsByVoiceActor(Guid voiceActorId);
    public void CreateVoiceActor(string name);
    public void UpdateVoiceActor(Guid voiceActorId, string name);
    public void DeleteVoiceActor(Guid voiceActorId);
    void AddVoiceActor(VoiceActor voiceActor);
}