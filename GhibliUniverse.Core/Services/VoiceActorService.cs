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
    
    public List<VoiceActor> GetAllVoiceActors()
    {
        return _voiceActorRepository.GetAllVoiceActors();
    }

    public VoiceActor GetVoiceActorById(Guid voiceActorId)
    {
        return _voiceActorRepository.GetVoiceActorById(voiceActorId);
    }

    public List<Film> GetFilmsByVoiceActor(Guid voiceActorId)
    {
        return _voiceActorRepository.GetFilmsByVoiceActor(voiceActorId);
    }

    public VoiceActor CreateVoiceActor(string name)
    {
        return _voiceActorRepository.CreateVoiceActor(name);
    }

    public VoiceActor UpdateVoiceActor(Guid voiceActorId, string name)
    {
        return _voiceActorRepository.UpdateVoiceActor(voiceActorId, name);
    }

    public void DeleteVoiceActor(Guid voiceActorId)
    {
        _voiceActorRepository.DeleteVoiceActor(voiceActorId);
    }
    
    public bool VoiceActorAlreadyExists(string name)
    {
        var voiceActorsWithMatchingName = GetAllVoiceActors().FirstOrDefault(f => f.Name == ValidatedString.From(name));
        return voiceActorsWithMatchingName != null;
    }

    public string BuildVoiceActorList()
    {
        var savedVoiceActors = GetAllVoiceActors();
        var stringBuilder = new StringBuilder();
        foreach (var voiceActor in savedVoiceActors)
        {
            stringBuilder.Append(voiceActor);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }
}