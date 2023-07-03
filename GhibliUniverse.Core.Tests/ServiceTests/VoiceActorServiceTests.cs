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
    public async Task GetAllVoiceActors_ReturnsAllVoiceActors_WhenCalled()
    {
        _mockedVoiceActorRepository.Setup(x => x.GetAllVoiceActorsAsync()).ReturnsAsync(_voiceActors);

        var voiceActors = await _voiceActorService.GetAllVoiceActorsAsync();

        Assert.Equal(2, voiceActors.Count);
    }
    
    [Fact]
    public async Task GetVoiceActorById_ReturnsVoiceActorWithMatchingId_WhenGivenVoiceActorId()
    {
        var voiceActorId = _voiceActors[0].Id;
        _mockedVoiceActorRepository.Setup(x => x.GetVoiceActorByIdAsync(voiceActorId)).ReturnsAsync(_voiceActors[0]);
        var expectedVoiceActor = new VoiceActor()
        {
            Id = voiceActorId,
            Name = ValidatedString.From("John Doe")
        };

        var actualVoiceActor = await _voiceActorService.GetVoiceActorByIdAsync(voiceActorId);

        Assert.Equal(expectedVoiceActor.Id, actualVoiceActor.Id);
        Assert.Equal(expectedVoiceActor.Name.Value, actualVoiceActor.Name.Value);
    }

    [Fact]
    public void GetVoiceActorById_ThrowsModelNotFoundException_WhenGivenIdOfVoiceActorWhichDoesNotExist()
    {
        var nonExistentVoiceActorId = Guid.Parse("00000000-0000-0000-0000-000000000005");
        _mockedVoiceActorRepository.Setup(x => x.GetVoiceActorByIdAsync(nonExistentVoiceActorId))
            .ThrowsAsync(new ModelNotFoundException(nonExistentVoiceActorId));

        Assert.ThrowsAsync<ModelNotFoundException>(() =>
            _voiceActorService.GetVoiceActorByIdAsync(nonExistentVoiceActorId));
    }

    [Fact]
    public async Task GetFilmsByVoiceActor_ReturnsFilmsWhichTheVoiceActorBelongsTo_WhenCalled()
    {
        var voiceActorId = _voiceActors[0].Id;
        _mockedVoiceActorRepository.Setup(x => x.GetFilmsByVoiceActorAsync(voiceActorId)).ReturnsAsync(_voiceActors[0].Films);
        _voiceActors[0].Films.Add(new Film());

        var films = await _voiceActorService.GetFilmsByVoiceActorAsync(voiceActorId);

        Assert.Equal(1, films.Count);
    }
     

    [Fact]
    public void GetFilmsByVoiceActor_ThrowsModelNotFoundException_WhenGivenIdOfVoiceActorWhichDoesNotExist()
    {
        var nonExistentVoiceActorId = Guid.Parse("00000000-0000-0000-0000-000000000005");
        _mockedVoiceActorRepository
            .Setup(x => x.GetFilmsByVoiceActorAsync(nonExistentVoiceActorId))
            .ThrowsAsync(new ModelNotFoundException(nonExistentVoiceActorId));

        Assert.ThrowsAsync<ModelNotFoundException>(() =>
            _voiceActorService.GetFilmsByVoiceActorAsync(nonExistentVoiceActorId));
    }
     
    [Fact]
    public async Task CreateVoiceActor_PersistsNewlyCreatedVoiceActor_WhenCalled()
    {
        var newVoiceActorName = "John Doe";
        _mockedVoiceActorRepository
            .Setup(x => x.CreateVoiceActorAsync(newVoiceActorName))
            .Callback((string name) =>
            {
                var newVoiceActor = new VoiceActor()
                {
                    Id = Guid.NewGuid(),
                    Name = ValidatedString.From(name)
                };

                _voiceActors.Add(newVoiceActor);
            });

        await _voiceActorService.CreateVoiceActorAsync(newVoiceActorName);
        var voiceActorId = _voiceActors[^1].Id;

        var voiceActorCount = _voiceActors.Count;
        var voiceActor = _voiceActors[^1];

        Assert.Equal(3, voiceActorCount);
        Assert.Equal(voiceActorId, voiceActor.Id);
    }

    [Fact]
    public void CreateVoiceActor_ThrowsArgumentException_WhenGivenInvalidInput()
    {
        _mockedVoiceActorRepository.Setup(x => x.CreateVoiceActorAsync("")).ThrowsAsync(new ArgumentException());

        Assert.ThrowsAsync<ArgumentException>(() => _voiceActorService.CreateVoiceActorAsync(""));
    }

    [Fact]
    public async Task UpdateVoiceActor_UpdatesVoiceActorName_WhenCalled()
    {
        var voiceActorId = _voiceActors[0].Id;
        _mockedVoiceActorRepository.Setup(x => x.UpdateVoiceActorAsync(voiceActorId, "Joe Doe"))
            .Callback((Guid voiceActorId, string newName) =>
            {
                _voiceActors[0].Name = ValidatedString.From(newName);
            });

        await _voiceActorService.UpdateVoiceActorAsync(voiceActorId, "Joe Doe");
        var voiceActorWithUpdatedName = _voiceActors[0];

        Assert.Equal(ValidatedString.From("Joe Doe"), voiceActorWithUpdatedName.Name);
    }
     
    [Fact]
    public void DeleteVoiceActor_RemovesActorWithMatchingIdFromVoiceActorList_WhenGivenVoiceActorId()
    {
        var voiceActorId = _voiceActors[0].Id;
        _mockedVoiceActorRepository.Setup(x => x.DeleteVoiceActorAsync(voiceActorId))
            .Callback(() => _voiceActors.Remove(_voiceActors[0]));

        _voiceActorService.DeleteVoiceActorAsync(voiceActorId);
        var voiceActorCount = _voiceActors.Count;

        Assert.Equal(1, voiceActorCount);
    }

    [Fact]
    public void DeleteVoiceActor_ThrowsModelNotFoundException_WhenGivenNonExistentVoiceActorId()
    {
        var nonExistentVoiceActorId = Guid.Parse("00000000-0000-0000-0000-000000000005");
        _mockedVoiceActorRepository.Setup(x => x.DeleteVoiceActorAsync(nonExistentVoiceActorId))
            .ThrowsAsync(new ModelNotFoundException(nonExistentVoiceActorId));

        Assert.ThrowsAsync<ModelNotFoundException>(() =>
            _voiceActorService.DeleteVoiceActorAsync(nonExistentVoiceActorId));
    }
}