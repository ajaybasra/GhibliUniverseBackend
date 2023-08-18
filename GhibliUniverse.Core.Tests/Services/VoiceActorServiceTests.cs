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
    public async Task GetAllVoiceActors_ReturnsAllVoiceActors_WhenCalled()
    {
        _mockedVoiceActorRepository.Setup(x => x.GetAllVoiceActors()).ReturnsAsync(_voiceActors);

        var voiceActors = await _voiceActorService.GetAllVoiceActors();

        Assert.Equal(2, voiceActors.Count);
    }
    
    [Fact]
    public async Task GetVoiceActorById_ReturnsVoiceActorWithMatchingId_WhenGivenVoiceActorId()
    {
        var voiceActorId = _voiceActors[0].Id;
        _mockedVoiceActorRepository.Setup(x => x.GetVoiceActorById(voiceActorId)).ReturnsAsync(_voiceActors[0]);
        var expectedVoiceActor = new VoiceActor()
        {
            Id = voiceActorId,
            Name = ValidatedString.From("John Doe")
        };

        var actualVoiceActor = await _voiceActorService.GetVoiceActorById(voiceActorId);

        Assert.Equal(expectedVoiceActor.Id, actualVoiceActor.Id);
        Assert.Equal(expectedVoiceActor.Name.Value, actualVoiceActor.Name.Value);
    }

    [Fact]
    public async Task GetVoiceActorById_ThrowsModelNotFoundException_WhenGivenIdOfVoiceActorWhichDoesNotExist()
    {
        var nonExistentVoiceActorId = Guid.Parse("00000000-0000-0000-0000-000000000005");
        _mockedVoiceActorRepository.Setup(x => x.GetVoiceActorById(nonExistentVoiceActorId))
            .ThrowsAsync(new ModelNotFoundException(nonExistentVoiceActorId));

        await Assert.ThrowsAsync<ModelNotFoundException>(() =>
            _voiceActorService.GetVoiceActorById(nonExistentVoiceActorId));
    }

    [Fact]
    public async Task GetFilmsByVoiceActor_ReturnsFilmsWhichTheVoiceActorBelongsTo_WhenCalled()
    {
        var voiceActorId = _voiceActors[0].Id;
        _mockedVoiceActorRepository.Setup(x => x.GetFilmsByVoiceActor(voiceActorId)).ReturnsAsync(_voiceActors[0].Films);
        _voiceActors[0].Films.Add(new VoiceActorFilm());

        var films = await _voiceActorService.GetFilmsByVoiceActor(voiceActorId);

        Assert.Single(films);
    }
     

    [Fact]
    public async Task GetFilmsByVoiceActor_ThrowsModelNotFoundException_WhenGivenIdOfVoiceActorWhichDoesNotExist()
    {
        var nonExistentVoiceActorId = Guid.Parse("00000000-0000-0000-0000-000000000005");
        _mockedVoiceActorRepository
            .Setup(x => x.GetFilmsByVoiceActor(nonExistentVoiceActorId))
            .ThrowsAsync(new ModelNotFoundException(nonExistentVoiceActorId));

        await Assert.ThrowsAsync<ModelNotFoundException>(() =>
            _voiceActorService.GetFilmsByVoiceActor(nonExistentVoiceActorId));
    }
     
    [Fact]
    public async Task CreateVoiceActor_PersistsNewlyCreatedVoiceActor_WhenCalled()
    {
        var newVoiceActorName = "John Doe";
        var newVoiceActorId = Guid.NewGuid(); 

        _mockedVoiceActorRepository
            .Setup(x => x.CreateVoiceActor(It.IsAny<VoiceActor>()))  
            .Callback((VoiceActor voiceActor) =>
            {
                voiceActor.Id = newVoiceActorId; 
                _voiceActors.Add(voiceActor);
            });

        await _voiceActorService.CreateVoiceActor(new VoiceActor() { Name = ValidatedString.From(newVoiceActorName) });

        var voiceActorCount = _voiceActors.Count;
        var voiceActor = _voiceActors[^1];  

        Assert.Equal(3, voiceActorCount); 
        Assert.Equal(newVoiceActorId, voiceActor.Id);
        Assert.Equal(newVoiceActorName, voiceActor.Name.Value);
    }
    

    [Fact]
    public async Task UpdateVoiceActor_UpdatesVoiceActorName_WhenCalled()
    {
        var voiceActorId = _voiceActors[0].Id;
        _mockedVoiceActorRepository.Setup(x => x.UpdateVoiceActor(voiceActorId, new VoiceActor() {Name = ValidatedString.From("John Doe")}))
            .Callback((Guid voiceActorId, string newName) =>
            {
                _voiceActors[0].Name = ValidatedString.From(newName);
            });

        await _voiceActorService.UpdateVoiceActor(voiceActorId, new VoiceActor() {Name = ValidatedString.From("Joe Doe")});
        var voiceActorWithUpdatedName = _voiceActors[0];

        Assert.Equal(ValidatedString.From("Joe Doe"), voiceActorWithUpdatedName.Name);
    }
     
    [Fact]
    public async Task DeleteVoiceActor_RemovesActorWithMatchingIdFromVoiceActorList_WhenGivenVoiceActorId()
    {
        var voiceActorId = _voiceActors[0].Id;
        _mockedVoiceActorRepository.Setup(x => x.DeleteVoiceActor(voiceActorId))
            .Callback(() => _voiceActors.Remove(_voiceActors[0]));

        await _voiceActorService.DeleteVoiceActor(voiceActorId);
        var voiceActorCount = _voiceActors.Count;

        Assert.Equal(1, voiceActorCount);
    }

    [Fact]
    public async Task DeleteVoiceActor_ThrowsModelNotFoundException_WhenGivenNonExistentVoiceActorId()
    {
        var nonExistentVoiceActorId = Guid.Parse("00000000-0000-0000-0000-000000000005");
        _mockedVoiceActorRepository.Setup(x => x.DeleteVoiceActor(nonExistentVoiceActorId))
            .ThrowsAsync(new ModelNotFoundException(nonExistentVoiceActorId));

        await Assert.ThrowsAsync<ModelNotFoundException>(() =>
            _voiceActorService.DeleteVoiceActor(nonExistentVoiceActorId));
    }
}