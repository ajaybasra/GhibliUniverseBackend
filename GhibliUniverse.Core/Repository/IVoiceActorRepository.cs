using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Repository;

public interface IVoiceActorRepository
{
    List<VoiceActor> GetAllVoiceActors();
    VoiceActor GetVoiceActorById(Guid voiceActorId);
    List<Film> GetFilmsByVoiceActor(Guid voiceActorId);
    public VoiceActor CreateVoiceActor(string name);
    public VoiceActor UpdateVoiceActor(Guid voiceActorId, string name);
    public void DeleteVoiceActor(Guid voiceActorId);
}