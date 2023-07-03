using System.Text;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Repository;

namespace GhibliUniverse.Core.Services;

public class VoiceActorService : IVoiceActorService
{
    private readonly IVoiceActorRepository _voiceActorRepository;

    public VoiceActorService(IVoiceActorRepository voiceActorRepository)
    {
        _voiceActorRepository = voiceActorRepository;
    }
    
    public async Task<List<VoiceActor>> GetAllVoiceActorsAsync()
    {
        return await _voiceActorRepository.GetAllVoiceActorsAsync();
    }

    public async Task<VoiceActor> GetVoiceActorByIdAsync(Guid voiceActorId)
    {
        return await _voiceActorRepository.GetVoiceActorByIdAsync(voiceActorId);
    }

    public async Task<List<Film>> GetFilmsByVoiceActorAsync(Guid voiceActorId)
    {
        return await _voiceActorRepository.GetFilmsByVoiceActorAsync(voiceActorId);
    }

    public async Task<VoiceActor> CreateVoiceActorAsync(string name)
    {
        return await _voiceActorRepository.CreateVoiceActorAsync(name);
    }

    public async Task<VoiceActor> UpdateVoiceActorAsync(Guid voiceActorId, string name)
    {
        return await _voiceActorRepository.UpdateVoiceActorAsync(voiceActorId, name);
    }

    public async Task DeleteVoiceActorAsync(Guid voiceActorId)
    {
        await _voiceActorRepository.DeleteVoiceActorAsync(voiceActorId);
    }

    public async Task<string> BuildVoiceActorListAsync()
    {
        var savedVoiceActors = await GetAllVoiceActorsAsync();
        var stringBuilder = new StringBuilder();
        foreach (var voiceActor in savedVoiceActors)
        {
            stringBuilder.Append(voiceActor);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }
}