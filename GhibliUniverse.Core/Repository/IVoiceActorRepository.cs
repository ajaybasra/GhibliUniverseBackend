using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Repository;

public interface IVoiceActorRepository
{
    Task<List<VoiceActor>> GetAllVoiceActors();
    Task<VoiceActor> GetVoiceActorById(Guid voiceActorId);
    Task<List<VoiceActorFilm>> GetFilmsByVoiceActor(Guid voiceActorId);
    Task<VoiceActor> CreateVoiceActor(VoiceActor voiceActorCreateRequest);
    Task<VoiceActor> UpdateVoiceActor(Guid voiceActorId, VoiceActor voiceActorUpdateRequest);
    Task DeleteVoiceActor(Guid voiceActorId);
}