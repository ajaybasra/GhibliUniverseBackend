using System.Text;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Services;

public class VoiceActorService : IVoiceActorService
{
    private readonly List<VoiceActor> _voiceActorList = new();
    
    public List<VoiceActor> GetAllVoiceActors()
    {
        return _voiceActorList;
    }

    public VoiceActor GetVoiceActorById(Guid voiceActorId)
    {
        var voiceActor = _voiceActorList.FirstOrDefault(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return voiceActor;
    }

    public List<Film> GetFilmsByVoiceActor(Guid voiceActorId)
    {
        var voiceActor = _voiceActorList.FirstOrDefault(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return voiceActor.Films;
    }

    public void CreateVoiceActor(string name)
    {
        try
        {
            var voiceActor = new VoiceActor
            {
                Id = Guid.NewGuid(),
                Name = ValidatedString.From(name)
            };
            _voiceActorList.Add(voiceActor);
        }
        catch (ArgumentException ae)
        {
            Console.WriteLine(ae);
        }
    }

    public void UpdateVoiceActor(Guid voiceActorId, string name)
    {
        var voiceActorToUpdate = _voiceActorList.FirstOrDefault(f => f.Id == voiceActorId);
        if (voiceActorToUpdate == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        voiceActorToUpdate.Name = ValidatedString.From(name);
    }

    public void DeleteVoiceActor(Guid voiceActorId)
    {
        var voiceActor = _voiceActorList.FirstOrDefault(f => f.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }
        voiceActor.Films.ForEach(f => f.RemoveVoiceActor(voiceActor));
        _voiceActorList.Remove(voiceActor);
    }
    
    public void AddVoiceActor(VoiceActor voiceActor)
    {
        _voiceActorList.Add(voiceActor);
    }
    
    public void PopulateVoiceActorsList(int numberOfVoiceActors)
    {
        for (var i = 0; i < numberOfVoiceActors; i++)
        {
            _voiceActorList.Add(new VoiceActor
            {
                Id = new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"),
                Name = ValidatedString.From("John Doe")
            });
        }
    }
    
    public string BuildVoiceActorList()
    {
        var stringBuilder = new StringBuilder();
        foreach (var voiceActor in _voiceActorList)
        {
            stringBuilder.Append(voiceActor);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }
}