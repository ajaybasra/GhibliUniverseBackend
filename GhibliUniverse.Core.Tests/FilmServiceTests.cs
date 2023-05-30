using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;
using Moq;

namespace GhibliUniverse.Core.Tests;

public class FilmServiceTests
{
    private readonly FilmService _filmService;
    private readonly Mock<IFilmPersistence> _mockedFilmPersistence;
    private readonly Mock<IReviewPersistence> _mockedReviewPersistence;
    private readonly Mock<IVoiceActorPersistence> _mockedVoiceActorPersistence;
    private readonly Mock<IFilmVoiceActorPersistence> _mockedFilmVoiceActorPersistence;
    private readonly List<Film> _films = new();
    private readonly List<VoiceActor> _voiceActors = new();

    public FilmServiceTests()
    {
        PopulateFilmsList(2);
        PopulateVoiceActorsList(2);
        _mockedFilmPersistence = new Mock<IFilmPersistence>();
        _mockedFilmPersistence.Setup(x => x.ReadFilms()).Returns(_films);
        _mockedReviewPersistence = new Mock<IReviewPersistence>();
        _mockedReviewPersistence.Setup(x => x.ReadReviews()).Returns(new List<Review>());
        _mockedVoiceActorPersistence = new Mock<IVoiceActorPersistence>();
        _mockedVoiceActorPersistence.Setup(x => x.ReadVoiceActors()).Returns(_voiceActors);
        _mockedFilmVoiceActorPersistence = new Mock<IFilmVoiceActorPersistence>();
        _mockedFilmVoiceActorPersistence.Setup(x => x.ReadFilmVoiceActorData()).Returns(new List<(Guid, Guid)>());
        _filmService = new FilmService(_mockedFilmPersistence.Object, _mockedReviewPersistence.Object,
            _mockedVoiceActorPersistence.Object, _mockedFilmVoiceActorPersistence.Object);
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
    public void GetAllFilms_ReturnsAllFilms_WhenCalled()
    {
        var filmCount = _filmService.GetAllFilms().Count;
        
        Assert.Equal(2, filmCount);
    }
    
    [Fact]
     public void GetFilmById_ReturnsFilmWithMatchingId_WhenGivenFilmId()
     {
         var expectedId = new Guid("00000000-0000-0000-0000-000000000000");

         var actualFilm = _filmService.GetFilmById(new Guid("00000000-0000-0000-0000-000000000000"));

         Assert.Equal(expectedId, actualFilm.Id);
     }
     
     [Fact]
     public void GetVoiceActorsByFilm_ReturnsVoiceActorsWhichTheFilmBelongsTo_WhenCalled()
     {
         var filmId = _filmService.GetAllFilms()[0].Id;
         _films[0].VoiceActors.Add(new VoiceActor());

         var voiceActorCount = _filmService.GetVoiceActorsByFilm(filmId).Count;
         
         Assert.Equal(1, voiceActorCount);
     }
     
     [Fact]
     public void CreateFilm_PersistsNewlyCreatedFilm_WhenCalled()
     {
         _filmService.CreateFilm("Title", "Description", "Director", "Composer", 2000);
         var filmId = _filmService.GetAllFilms()[2].Id;
         
         var filmCount = _filmService.GetAllFilms().Count;
         var film = _filmService.GetFilmById(filmId);
         
         Assert.Equal(3, filmCount);
         Assert.Equal(filmId, film.Id);
     }
     
     [Fact]
     public void UpdateFilm_UpdatesFilmFields_WhenPassedAFilmInstance()
     {
         var filmId = _filmService.GetAllFilms()[0].Id;
         var newFilm = new Film
         {
             Title = ValidatedString.From("UpdatedTitle"),
             Description = ValidatedString.From("UpdatedDescription"),
             Director = ValidatedString.From("UpdatedDirector"),
             Composer = ValidatedString.From("UpdatedComposer"),
             ReleaseYear = ReleaseYear.From(2001)
             
         };
         _filmService.UpdateFilm(filmId, newFilm);
         var filmWithUpdatedFields = _filmService.GetFilmById(filmId);

         Assert.Equal(ValidatedString.From("UpdatedTitle"), filmWithUpdatedFields.Title);
         Assert.Equal(ValidatedString.From("UpdatedDescription"), filmWithUpdatedFields.Description);
         Assert.Equal(ValidatedString.From("UpdatedDirector"), filmWithUpdatedFields.Director);
         Assert.Equal(ValidatedString.From("UpdatedComposer"), filmWithUpdatedFields.Composer);
         Assert.Equal(ReleaseYear.From(2001), filmWithUpdatedFields.ReleaseYear);
     }
     
     [Fact]
     public void DeleteFilm_RemovesFilmWithMatchingIdFromFilmList_WhenGivenFilmId()
     {
         var filmId = _filmService.GetAllFilms()[0].Id;
         
         _filmService.DeleteFilm(filmId);
         var voiceActorCount = _filmService.GetAllFilms().Count;
         
         Assert.Equal(1, voiceActorCount);
     }

     [Fact]
     public void AddFilm_AddsFilmToFilmList_WhenCalled()
     {
         var film = new Film();
         
         _filmService.AddFilm(film);
         var filmCount = _filmService.GetAllFilms().Count;
         
         Assert.Equal(3, filmCount);
     }

     [Fact]
     public void AddVoiceActor_AddsVoiceActorToAFilm_WhenCalled()
     {
         var filmToHaveVoiceActorAddedId = _filmService.GetAllFilms()[0].Id;
         var voiceActor = new VoiceActor();
         
         _filmService.AddVoiceActor(filmToHaveVoiceActorAddedId, voiceActor);
         var filmVoiceActorCount = _filmService.GetAllFilms()[0].VoiceActors.Count;
         
         Assert.Equal(1, filmVoiceActorCount);
     }
}