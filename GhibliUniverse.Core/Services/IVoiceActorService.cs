using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Services;

public interface IVoiceActorService
{
    Task<List<VoiceActor>> GetAllVoiceActors();
    Task<VoiceActor> GetVoiceActorById(Guid voiceActorId);
    Task<List<Film>> GetFilmsByVoiceActor(Guid voiceActorId);
    Task<VoiceActor> CreateVoiceActor(string name);
    Task<VoiceActor> UpdateVoiceActor(Guid voiceActorId, string name);
    Task DeleteVoiceActor(Guid voiceActorId);
}