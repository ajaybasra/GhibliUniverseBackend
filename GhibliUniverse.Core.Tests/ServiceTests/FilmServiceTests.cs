using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Repository;
using GhibliUniverse.Core.Services;
using Moq;

namespace GhibliUniverse.Core.Tests.ServiceTests;

public class FilmServiceTests
{
    private readonly FilmService _filmService;
    private readonly Mock<IFilmRepository> _mockedFilmRepository;
    private readonly List<Film> _films = new();
    private readonly List<VoiceActor> _voiceActors = new();

    public FilmServiceTests()
    {
        PopulateFilmsList(2);
        PopulateVoiceActorsList(2);
        _mockedFilmRepository = new Mock<IFilmRepository>();
        _filmService = new FilmService(_mockedFilmRepository.Object);
    }
    
    public void PopulateFilmsList(int numberOfFilms)
    {
        var filmTitles = new List<string> { "Spirited Away", "My Neighbor Totoro", "Ponyo" };
        var filmDescriptions = new List<string>
        {
            "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.",
            "Mei and Satsuki shift to a new house to be closer to their mother who is in the hospital. They soon become friends with Totoro, a giant rabbit-like creature who is a spirit.",
            "During a forbidden excursion to see the surface world, a goldfish princess encounters a human boy named Sosuke, who gives her the name Ponyo."
        };
        var releaseYears = new List<int> { 2001, 1988, 2008 };
        
        for (var i = 0; i < numberOfFilms; i++)
        {
            _films.Add(new Film
            {
                Id = new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"),
                Title = ValidatedString.From(filmTitles[i]),
                Description = ValidatedString.From(filmDescriptions[i]),
                Director = ValidatedString.From("Hayao Miyazaki"),
                Composer = ValidatedString.From("Joe Hisaishi"),
                ReleaseYear = ReleaseYear.From(releaseYears[i]),
            });
        }

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
    public async Task GetAllFilms_ReturnsAllFilms_WhenCalled()
    {
        _mockedFilmRepository.Setup(x => x.GetAllFilmsAsync()).ReturnsAsync(_films);

        var filmCount = (await _filmService.GetAllFilmsAsync()).Count;

        Assert.Equal(2, filmCount);
    }
    
     [Fact]
     public async Task GetFilmById_ReturnsFilmWithMatchingId_WhenGivenFilmId()
     {
         var expectedId = new Guid("00000000-0000-0000-0000-000000000000");
         _mockedFilmRepository.Setup(x => x.GetFilmByIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000000"))).ReturnsAsync(_films[0]);

         var actualFilm = await _filmService.GetFilmByIdAsync(new Guid("00000000-0000-0000-0000-000000000000"));

         Assert.Equal(expectedId, actualFilm.Id);
     }
     
     [Fact]
     public void GetFilmById_ThrowsModelNotFoundException_WhenGivenIdOfFilmWhichDoesNotExist()
     {
         _mockedFilmRepository.Setup(x => x.GetFilmByIdAsync(Guid.Parse("90000000-0000-0000-0000-000000000005")))
             .ThrowsAsync(new ModelNotFoundException(Guid.Parse("90000000-0000-0000-0000-000000000005")));
         
         Assert.ThrowsAsync<ModelNotFoundException>(() => _filmService.GetFilmByIdAsync(Guid.Parse("90000000-0000-0000-0000-000000000005")));
     }

     // [Fact]
     // public void GetVoiceActorsByFilm_ReturnsVoiceActorsWhichTheFilmBelongsTo_WhenCalled()
     // {
     //     var filmId = _films[0].Id;
     //     _mockedFilmRepository.Setup(x => x.GetVoiceActorsByFilm(filmId)).Returns(_films[0].VoiceActors);
     //     _films[0].VoiceActors.Add(new VoiceActor());
     //
     //     var voiceActorCount = _filmService.GetVoiceActorsByFilm(filmId).Count;
     //     
     //     Assert.Equal(1, voiceActorCount);
     // }
     
     [Fact]
     public async Task GetVoiceActorsByFilmAsync_ReturnsVoiceActorsForFilm_WhenGivenFilmId()
     {
         var filmId = new Guid("00000000-0000-0000-0000-000000000000");
         var expectedVoiceActors = new List<VoiceActor> { _voiceActors[0], _voiceActors[1] };
         _mockedFilmRepository.Setup(x => x.GetVoiceActorsByFilmAsync(filmId)).ReturnsAsync(expectedVoiceActors);

         var actualVoiceActors = await _filmService.GetVoiceActorsByFilmAsync(filmId);

         Assert.Equal(expectedVoiceActors.Count, actualVoiceActors.Count);
         Assert.Equal(expectedVoiceActors[0].Id, actualVoiceActors[0].Id);
         Assert.Equal(expectedVoiceActors[1].Id, actualVoiceActors[1].Id);
     }
     
     
     [Fact]
     public void GetVoiceActorsByFilm_ThrowsModelNotFoundException_WhenGivenIdOfFilmWhichDoesNotExist()
     {
         var nonExistentFilmId = Guid.Parse("00000000-0000-0000-0000-000000000005");
         _mockedFilmRepository
             .Setup(x => x.GetVoiceActorsByFilmAsync(nonExistentFilmId))
             .ThrowsAsync(new ModelNotFoundException(nonExistentFilmId));
         
         Assert.ThrowsAsync<ModelNotFoundException>(() =>
             _filmService.GetVoiceActorsByFilmAsync(nonExistentFilmId));
     }

     [Fact]
     public async Task CreateFilm_ReturnsCreatedFilm_WhenCalledWithValidData()
     {
         var filmTitle = "Spirited Away";
         var filmDescription = "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.";
         var director = "Hayao Miyazaki";
         var composer = "Joe Hisaishi";
         var releaseYear = 2001;
         var createdFilm = new Film
         {
             Id = Guid.NewGuid(),
             Title = ValidatedString.From(filmTitle),
             Description = ValidatedString.From(filmDescription),
             Director = ValidatedString.From(director),
             Composer = ValidatedString.From(composer),
             ReleaseYear = ReleaseYear.From(releaseYear),
         };
         _mockedFilmRepository.Setup(x => x.CreateFilmAsync(filmTitle, filmDescription, director, composer, releaseYear)).ReturnsAsync(createdFilm);

         var result = await _filmService.CreateFilmAsync(filmTitle, filmDescription, director, composer, releaseYear);

         Assert.Equal(createdFilm.Id, result.Id);
         Assert.Equal(createdFilm.Title, result.Title);
         Assert.Equal(createdFilm.Description, result.Description);
         Assert.Equal(createdFilm.Director, result.Director);
         Assert.Equal(createdFilm.Composer, result.Composer);
         Assert.Equal(createdFilm.ReleaseYear, result.ReleaseYear);
     }
     
     [Fact]
     public void CreateFilm_ThrowsArgumentException_WhenGivenEmptyInputs()
     {
         _mockedFilmRepository.Setup(x => x.CreateFilmAsync("", "", "", "", 2001)).Throws(new ArgumentException());
         
         Assert.ThrowsAsync<ArgumentException>(() => _filmService.CreateFilmAsync("", "", "", "", 2001));
     }
     
     [Fact]
     public void CreateFilm_ThrowsReleaseYearLessThanOldestReleaseYearException_WhenGivenReleaseYearLesserThan1984()
     {
         _mockedFilmRepository.Setup(x => x.CreateFilmAsync("test", "test", "test", "test", 1983))
             .Throws(new ReleaseYear.ReleaseYearLessThanOldestReleaseYearException(1983));
         
         Assert.ThrowsAsync<ReleaseYear.ReleaseYearLessThanOldestReleaseYearException>(() => _filmService.CreateFilmAsync("test", "test", "test", "test", 1983));
     }
     
     [Fact]
     public void CreateFilm_ThrowsNotFourCharactersException_WhenGivenReleaseYearWhichIsNotOfLengthFour()
     {
         _mockedFilmRepository.Setup(x => x.CreateFilmAsync("test", "test", "test", "test", 200))
             .ThrowsAsync(new ReleaseYear.NotFourCharactersException(200));
         
         Assert.ThrowsAsync<ReleaseYear.NotFourCharactersException>(() => _filmService.CreateFilmAsync("test", "test", "test", "test", 200));
     }
     
     [Fact]
     public async Task UpdateFilmAsync_ReturnsUpdatedFilm_WhenCalledWithValidData()
     {
         var filmId = new Guid("00000000-0000-0000-0000-000000000000");
         var updatedFilm = new Film
         {
             Title = ValidatedString.From("Updated Title"),
             Description = ValidatedString.From("Updated Description"),
             Director = ValidatedString.From("Updated Director"),
             Composer = ValidatedString.From("Updated Composer"),
             ReleaseYear = ReleaseYear.From(2000),
         };
         _mockedFilmRepository.Setup(x => x.UpdateFilmAsync(filmId, updatedFilm)).ReturnsAsync(updatedFilm);

         var result = await _filmService.UpdateFilmAsync(filmId, updatedFilm);

         Assert.Equal(filmId, result.Id);
         Assert.Equal(updatedFilm.Title, result.Title);
         Assert.Equal(updatedFilm.Description, result.Description);
         Assert.Equal(updatedFilm.Director, result.Director);
         Assert.Equal(updatedFilm.Composer, result.Composer);
         Assert.Equal(updatedFilm.ReleaseYear, result.ReleaseYear);
     }
     
     [Fact]
     public void DeleteFilm_RemovesFilmWithMatchingIdFromFilmList_WhenGivenFilmId()
     {
         var filmId = _films[0].Id;
         _mockedFilmRepository.Setup(x => x.DeleteFilmAsync(filmId))
             .Callback(() => _films.Remove(_films[0]));

         _filmService.DeleteFilmAsync(filmId);
         var voiceActorCount = _films.Count;
         
         Assert.Equal(1, voiceActorCount);
     }
     
     [Fact]
     public void DeleteFilm_ThrowsModelNotFoundException_WhenGivenNonExistentFilmId()
     {
         _mockedFilmRepository.Setup(x => x.DeleteFilmAsync(Guid.Parse("00000000-0000-0000-0000-000000000005")))
             .Throws(new ModelNotFoundException(Guid.Parse("00000000-0000-0000-0000-000000000005")));
         
         Assert.ThrowsAsync<ModelNotFoundException>(() =>
             _filmService.DeleteFilmAsync(Guid.Parse("00000000-0000-0000-0000-000000000005")));
     }

     [Fact]
     public void LinkVoiceActor_LinksVoiceActorToAFilm_WhenCalled()
     {
         var filmToHaveVoiceActorAddedId = _films[0].Id;
         var voiceActor = new VoiceActor();
         _mockedFilmRepository.Setup(x => x.LinkVoiceActorAsync(filmToHaveVoiceActorAddedId, voiceActor.Id))
             .Callback(() => _films[0].VoiceActors.Add(new VoiceActor()));
         
         _filmService.LinkVoiceActorAsync(filmToHaveVoiceActorAddedId, voiceActor.Id);
         var filmVoiceActorCount = _films[0].VoiceActors.Count;
         
         Assert.Equal(1, filmVoiceActorCount);
     }
     
     [Fact]
     public void UnlinkVoiceActor_CallsRepositoryUnlinkVoiceActorMethod_WhenCalledWithValidData()
     {
         var filmId = new Guid("00000000-0000-0000-0000-000000000000");
         var voiceActorId = new Guid("11111111-1111-1111-1111-111111111111");

         _filmService.UnlinkVoiceActorAsync(filmId, voiceActorId);

         _mockedFilmRepository.Verify(x => x.UnlinkVoiceActorAsync(filmId, voiceActorId), Times.Once);
     }

     [Fact]
     public async Task FilmIdAlreadyExistsAsync_ReturnsTrue_WhenFilmIdExists()
     {
         var filmId = new Guid("00000000-0000-0000-0000-000000000000");
         _mockedFilmRepository.Setup(x => x.GetAllFilmsAsync()).ReturnsAsync(_films);

         var result = await _filmService.FilmIdAlreadyExistsAsync(filmId);

         Assert.True(result);
     }

     [Fact]
     public async Task FilmIdAlreadyExistsAsync_ReturnsFalse_WhenFilmIdDoesNotExist()
     {
         var filmId = new Guid("00000000-0000-0000-0000-000000000001");
         _mockedFilmRepository.Setup(x => x.GetAllFilmsAsync()).ReturnsAsync(_films);

         var result = await _filmService.FilmIdAlreadyExistsAsync(filmId);

         Assert.False(result);
     }

     [Fact]
     public async Task FilmTitleAlreadyExistsAsync_ReturnsTrue_WhenFilmTitleExists()
     {
         var filmTitle = "Spirited Away";
         _mockedFilmRepository.Setup(x => x.GetAllFilmsAsync()).ReturnsAsync(_films);

         var result = await _filmService.FilmTitleAlreadyExistsAsync(filmTitle);

         Assert.True(result);
     }

     [Fact]
     public async Task FilmTitleAlreadyExistsAsync_ReturnsFalse_WhenFilmTitleDoesNotExist()
     {
         var filmTitle = "Princess Mononoke";
         _mockedFilmRepository.Setup(x => x.GetAllFilmsAsync()).ReturnsAsync(_films);

         var result = await _filmService.FilmTitleAlreadyExistsAsync(filmTitle);

         Assert.False(result);
     }

}