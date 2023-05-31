using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;
using Moq;

namespace GhibliUniverse.Core.Tests;

public class VoiceActorServiceTests
{
    private readonly VoiceActorService _voiceActorService;
    private readonly Mock<IVoiceActorPersistence> _mockedVoiceActorPersistence;
    private readonly List<VoiceActor> _voiceActors = new();
    
    public VoiceActorServiceTests()
    {
        PopulateVoiceActorsList(2);
        _mockedVoiceActorPersistence = new Mock<IVoiceActorPersistence>();
        _mockedVoiceActorPersistence.Setup(x => x.ReadVoiceActors()).Returns(_voiceActors);
        _voiceActorService = new VoiceActorService(_mockedVoiceActorPersistence.Object);
    }
    
    private void PopulateVoiceActorsList(int numberOfVoiceActors)
    {
        for (var i = 0; i < numberOfVoiceActors; i++)
        {
            _voiceActors.Add(new VoiceActor
            {
                Id = new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"),
                Name = ValidatedString.From("John Doe")
            });
        }
    }

    [Fact]
    public void GetAllVoiceActors_ReturnsAllVoiceActors_WhenCalled() // probs need to check that the two returned va's are not duplicates
    {
        var voiceActorsCount = _voiceActorService.GetAllVoiceActors().Count;
        
        Assert.Equal(2, voiceActorsCount);
    }
    
     [Fact]
     public void GetVoiceActorById_ReturnsVoiceActorWithMatchingId_WhenGivenVoiceActorId()
     {
         var voiceActorId = _voiceActors[0].Id;

         var expectedVoiceActor = new VoiceActor()
         {
             Id = voiceActorId,
             Name = ValidatedString.From("John Doe")
         };

         var actualVoiceActor = _voiceActorService.GetVoiceActorById(voiceActorId);
         
         Assert.Equivalent(expectedVoiceActor, actualVoiceActor);
     }

     [Fact]
     public void GetFilmsByVoiceActor_ReturnsFilmsWhichTheVoiceActorBelongsTo_WhenCalled() //tests should be independent, try not rely on ext methods
     {
         var voiceActorId = _voiceActors[0].Id;
         _voiceActors[0].Films.Add(new Film());

         var filmCount = _voiceActorService.GetFilmsByVoiceActor(voiceActorId).Count;
         
         Assert.Equal(1, filmCount);
     }
     
     [Fact]
     public void CreateVoiceActor_PersistsNewlyCreatedVoiceActor_WhenCalled()
     {
         _voiceActorService.CreateVoiceActor("John D");
         var voiceActorId = _voiceActors[2].Id;
         
         var voiceActorCount = _voiceActors.Count;
         var voiceActor = _voiceActorService.GetVoiceActorById(voiceActorId);
         
         Assert.Equal(3, voiceActorCount);
         Assert.Equal(voiceActorId, voiceActor.Id);
     }

     [Fact]
     public void UpdateVoiceActor_UpdatesVoiceActorName_WhenCalled()
     {
         var voiceActorId = _voiceActors[0].Id;
         
         _voiceActorService.UpdateVoiceActor(voiceActorId, "Joe Doe");
         var voiceActorWithUpdatedName = _voiceActorService.GetVoiceActorById(voiceActorId);

         Assert.Equal(ValidatedString.From("Joe Doe"), voiceActorWithUpdatedName.Name);
     }
     
     [Fact]
    public void DeleteVoiceActor_RemovesActorWithMatchingIdFromVoiceActorList_WhenGivenVoiceActorId()
     {
         var voiceActorId = _voiceActors[0].Id;;
         
         _voiceActorService.DeleteVoiceActor(voiceActorId);
         var voiceActorCount = _voiceActors.Count;
         
         Assert.Equal(1, voiceActorCount);
     }
}