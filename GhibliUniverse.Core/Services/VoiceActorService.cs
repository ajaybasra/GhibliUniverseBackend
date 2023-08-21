using System.Text;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Repository;

namespace GhibliUniverse.Core.Services;

public class VoiceActorService : IVoiceActorService
{
    private readonly IVoiceActorRepository _voiceActorRepository;

    public VoiceActorService(IVoiceActorRepository voiceActorRepository)
    {
        _voiceActorRepository = voiceActorRepository;
    }
    
    public async Task<List<VoiceActor>> GetAllVoiceActors()
    {
        return await _voiceActorRepository.GetAllVoiceActors();
    }

    public async Task<VoiceActor> GetVoiceActorById(Guid voiceActorId)
    {
        return await _voiceActorRepository.GetVoiceActorById(voiceActorId);
    }

    public async Task<List<VoiceActorFilm>> GetFilmsByVoiceActor(Guid voiceActorId)
    {
        return await _voiceActorRepository.GetFilmsByVoiceActor(voiceActorId);
    }

    public async Task<VoiceActor> CreateVoiceActor(VoiceActor voiceActorCreateRequest)
    {
        return await _voiceActorRepository.CreateVoiceActor(voiceActorCreateRequest);
    }

    public async Task<VoiceActor> UpdateVoiceActor(Guid voiceActorId, VoiceActor voiceActorUpdateRequest)
    {
        return await _voiceActorRepository.UpdateVoiceActor(voiceActorId, voiceActorUpdateRequest);
    }

    public async Task DeleteVoiceActor(Guid voiceActorId)
    {
        await _voiceActorRepository.DeleteVoiceActor(voiceActorId);
    }

    public async Task<string> BuildVoiceActorList()
    {
        var savedVoiceActors = await GetAllVoiceActors();
        var stringBuilder = new StringBuilder();
        foreach (var voiceActor in savedVoiceActors)
        {
            stringBuilder.Append(voiceActor);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }
}