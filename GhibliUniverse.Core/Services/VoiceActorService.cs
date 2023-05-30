using System.Text;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Services;

public class VoiceActorService : IVoiceActorService
{
    private readonly VoiceActorPersistence _voiceActorPersistence;

    public VoiceActorService(VoiceActorPersistence voiceActorPersistence)
    {
        _voiceActorPersistence = voiceActorPersistence;
    }
    
    public List<VoiceActor> GetAllVoiceActors()
    {
        return _voiceActorPersistence.ReadVoiceActors();
    }

    public VoiceActor GetVoiceActorById(Guid voiceActorId)
    {
        var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();
        var voiceActor = savedVoiceActors.FirstOrDefault(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return voiceActor;
    }

    public List<Film> GetFilmsByVoiceActor(Guid voiceActorId)
    {
        var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();
        var voiceActor = savedVoiceActors.FirstOrDefault(v => v.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        return voiceActor.Films;
    }

    public void CreateVoiceActor(string name)
    {
        var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();
        try
        {
            var voiceActor = new VoiceActor
            {
                Id = Guid.NewGuid(),
                Name = ValidatedString.From(name)
            };
            savedVoiceActors.Add(voiceActor);
            _voiceActorPersistence.WriteVoiceActors(savedVoiceActors);
        }
        catch (ArgumentException ae)
        {
            Console.WriteLine(ae);
        }
    }

    public void UpdateVoiceActor(Guid voiceActorId, string name)
    {
        var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();
        var voiceActorToUpdate = savedVoiceActors.FirstOrDefault(f => f.Id == voiceActorId);
        if (voiceActorToUpdate == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }

        voiceActorToUpdate.Name = ValidatedString.From(name);
        _voiceActorPersistence.WriteVoiceActors(savedVoiceActors);
    }

    public void DeleteVoiceActor(Guid voiceActorId)
    {
        var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();
        var voiceActor = savedVoiceActors.FirstOrDefault(f => f.Id == voiceActorId);
        if (voiceActor == null)
        {
            throw new ModelNotFoundException(voiceActorId);
        }
        voiceActor.Films.ForEach(f => f.RemoveVoiceActor(voiceActor));
        savedVoiceActors.Remove(voiceActor);
        _voiceActorPersistence.WriteVoiceActors(savedVoiceActors);
    }
    
    public void AddVoiceActor(VoiceActor voiceActor)
    {
        var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();
        savedVoiceActors.Add(voiceActor);
        _voiceActorPersistence.WriteVoiceActors(savedVoiceActors);
    }
    
    public void PopulateVoiceActorsList(int numberOfVoiceActors)
    {
        var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();
        for (var i = 0; i < numberOfVoiceActors; i++)
        {
            savedVoiceActors.Add(new VoiceActor
            {
                Id = new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"),
                Name = ValidatedString.From("John Doe")
            });
        }
        _voiceActorPersistence.WriteVoiceActors(savedVoiceActors);
    }
    
    public string BuildVoiceActorList()
    {
        var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();
        var stringBuilder = new StringBuilder();
        foreach (var voiceActor in savedVoiceActors)
        {
            stringBuilder.Append(voiceActor);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }
}