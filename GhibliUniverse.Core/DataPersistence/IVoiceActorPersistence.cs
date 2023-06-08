using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.DataPersistence;

public interface IVoiceActorPersistence
{
    List<VoiceActor> ReadVoiceActors();
    void WriteVoiceActors(List<VoiceActor> voiceActors);
}