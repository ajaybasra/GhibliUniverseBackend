using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Repository;

public interface IVoiceActorRepository
{
    Task<List<VoiceActor>> GetAllVoiceActorsAsync();
    Task<VoiceActor> GetVoiceActorByIdAsync(Guid voiceActorId);
    Task<List<Film>> GetFilmsByVoiceActorAsync(Guid voiceActorId);
    Task<VoiceActor> CreateVoiceActorAsync(string name);
    Task<VoiceActor> UpdateVoiceActorAsync(Guid voiceActorId, string name);
    Task DeleteVoiceActorAsync(Guid voiceActorId);
}