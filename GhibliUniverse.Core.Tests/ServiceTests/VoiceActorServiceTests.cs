using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Repository;
using GhibliUniverse.Core.Services;
using Moq;

namespace GhibliUniverse.Core.Tests.ServiceTests;

public class VoiceActorServiceTests
{
    private readonly VoiceActorService _voiceActorService;
    private readonly Mock<IVoiceActorRepository> _mockedVoiceActorRepository;
    private readonly List<VoiceActor> _voiceActors = new();
    
    public VoiceActorServiceTests()
    {
        PopulateVoiceActorsList(2);
        _mockedVoiceActorRepository = new Mock<IVoiceActorRepository>();
        _voiceActorService = new VoiceActorService(_mockedVoiceActorRepository.Object);
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
    public void GetAllVoiceActors_ReturnsAllVoiceActors_WhenCalled() 
    {
        _mockedVoiceActorRepository.Setup(x => x.GetAllVoiceActors()).Returns(_voiceActors);
        
        var voiceActorsCount = _voiceActorService.GetAllVoiceActors().Count;
        
        Assert.Equal(2, voiceActorsCount);
    }
    
     [Fact]
     public void GetVoiceActorById_ReturnsVoiceActorWithMatchingId_WhenGivenVoiceActorId()
     {
         var voiceActorId = _voiceActors[0].Id;
         _mockedVoiceActorRepository.Setup(x => x.GetVoiceActorById(Guid.Parse("00000000-0000-0000-0000-000000000000"))).Returns(_voiceActors[0]);
         var expectedVoiceActor = new VoiceActor()
         {
             Id = voiceActorId,
             Name = ValidatedString.From("John Doe")
         };

         var actualVoiceActor = _voiceActorService.GetVoiceActorById(voiceActorId);
         
         Assert.Equivalent(expectedVoiceActor, actualVoiceActor);
     }

     [Fact]
     public void GetVoiceActorById_ThrowsModelNotFoundException_WhenGivenIdOfVoiceActorWhichDoesNotExist()
     {
         _mockedVoiceActorRepository.Setup(x => x.GetVoiceActorById(Guid.Parse("00000000-0000-0000-0000-000000000005")))
             .Throws(new ModelNotFoundException(Guid.Parse("00000000-0000-0000-0000-000000000005")));
         
         Assert.Throws<ModelNotFoundException>(() => _voiceActorService.GetVoiceActorById(Guid.Parse("00000000-0000-0000-0000-000000000005")));
     }

     [Fact]
     public void GetFilmsByVoiceActor_ReturnsFilmsWhichTheVoiceActorBelongsTo_WhenCalled()  
     {
         var voiceActorId = _voiceActors[0].Id;
         _mockedVoiceActorRepository.Setup(x => x.GetFilmsByVoiceActor(voiceActorId)).Returns(_voiceActors[0].Films);
         _voiceActors[0].Films.Add(new Film());

         var filmCount = _voiceActorService.GetFilmsByVoiceActor(voiceActorId).Count;
         
         Assert.Equal(1, filmCount);
     }
     
     [Fact]
     public void GetFilmsByVoiceActor_ThrowsModelNotFoundException_WhenGivenIdOfVoiceActorWhichDoesNotExist()
     {
         _mockedVoiceActorRepository
             .Setup(x => x.GetFilmsByVoiceActor(Guid.Parse("00000000-0000-0000-0000-000000000005")))
             .Throws(new ModelNotFoundException(Guid.Parse("00000000-0000-0000-0000-000000000005")));
         
         Assert.Throws<ModelNotFoundException>(() => _voiceActorService.GetFilmsByVoiceActor(Guid.Parse("00000000-0000-0000-0000-000000000005")));
     }
     
     [Fact]
     public void CreateVoiceActor_PersistsNewlyCreatedVoiceActor_WhenCalled()
     {
         _mockedVoiceActorRepository
             .Setup(x => x.CreateVoiceActor("John Doe"))
             .Callback((string name) =>
             {
                 var newVoiceActor = new VoiceActor()
                 {
                     Name = ValidatedString.From(name)
                 };
        
                 _voiceActors.Add(newVoiceActor);
             });
         
         _voiceActorService.CreateVoiceActor("John Doe");
         var voiceActorId = _voiceActors[2].Id;
         
         var voiceActorCount = _voiceActors.Count;
         var voiceActor = _voiceActors[2];
         
         Assert.Equal(3, voiceActorCount);
         Assert.Equal(voiceActorId, voiceActor.Id);
     }

     [Fact]
     public void CreateVoiceActor_ThrowsArgumentException_WhenGivenInvalidInput()
     {
         _mockedVoiceActorRepository.Setup(x => x.CreateVoiceActor("")).Throws(new ArgumentException());
         
         Assert.Throws<ArgumentException>(() => _voiceActorService.CreateVoiceActor(""));
     }

     [Fact]
     public void UpdateVoiceActor_UpdatesVoiceActorName_WhenCalled()
     {
         var voiceActorId = _voiceActors[0].Id;
         _mockedVoiceActorRepository.Setup(x => x.UpdateVoiceActor(voiceActorId, "Joe Doe")).Callback((Guid voiceActorId, string newName) =>
         {
             _voiceActors[0].Name = ValidatedString.From(newName);

         });
         
         _voiceActorService.UpdateVoiceActor(voiceActorId, "Joe Doe");
         var voiceActorWithUpdatedName = _voiceActors[0];

         Assert.Equal(ValidatedString.From("Joe Doe"), voiceActorWithUpdatedName.Name);
     }
     
     [Fact]
    public void DeleteVoiceActor_RemovesActorWithMatchingIdFromVoiceActorList_WhenGivenVoiceActorId()
     {
         var voiceActorId = _voiceActors[0].Id;
         _mockedVoiceActorRepository.Setup(x => x.DeleteVoiceActor(voiceActorId))
             .Callback(() => _voiceActors.Remove(_voiceActors[0]));

         _voiceActorService.DeleteVoiceActor(voiceActorId);
         var voiceActorCount = _voiceActors.Count;
         
         Assert.Equal(1, voiceActorCount);
     }

    [Fact]
    public void DeleteVoiceActor_ThrowsModelNotFoundException_WhenGivenNonExistentVoiceActorId()
    {
        _mockedVoiceActorRepository.Setup(x => x.DeleteVoiceActor(Guid.Parse("00000000-0000-0000-0000-000000000005")))
            .Throws(new ModelNotFoundException(Guid.Parse("00000000-0000-0000-0000-000000000005")));
        
        Assert.Throws<ModelNotFoundException>(() =>
            _voiceActorService.DeleteVoiceActor(Guid.Parse("00000000-0000-0000-0000-000000000005")));
    }
}