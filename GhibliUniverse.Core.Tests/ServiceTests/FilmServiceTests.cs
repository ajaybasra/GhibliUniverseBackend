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
    public void GetAllFilms_ReturnsAllFilms_WhenCalled()
    {
        _mockedFilmRepository.Setup(x => x.GetAllFilms()).Returns(_films);
        
        var filmCount = _filmService.GetAllFilms().Count;
        
        Assert.Equal(2, filmCount);
    }
    
    [Fact]
     public void GetFilmById_ReturnsFilmWithMatchingId_WhenGivenFilmId()
     {
         var expectedId = new Guid("00000000-0000-0000-0000-000000000000");
         _mockedFilmRepository.Setup(x => x.GetFilmById(Guid.Parse("00000000-0000-0000-0000-000000000000"))).Returns(_films[0]);
         
         var actualFilm = _filmService.GetFilmById(new Guid("00000000-0000-0000-0000-000000000000"));

         Assert.Equal(expectedId, actualFilm.Id);
     }
     
     [Fact]
     public void GetFilmById_ThrowsModelNotFoundException_WhenGivenIdOfFilmWhichDoesNotExist()
     {
         _mockedFilmRepository.Setup(x => x.GetFilmById(Guid.Parse("90000000-0000-0000-0000-000000000005")))
             .Throws(new ModelNotFoundException(Guid.Parse("90000000-0000-0000-0000-000000000005")));
         
         Assert.Throws<ModelNotFoundException>(() => _filmService.GetFilmById(Guid.Parse("90000000-0000-0000-0000-000000000005")));
     }
     
     [Fact]
     public void GetVoiceActorsByFilm_ReturnsVoiceActorsWhichTheFilmBelongsTo_WhenCalled()
     {
         var filmId = _films[0].Id;
         _mockedFilmRepository.Setup(x => x.GetVoiceActorsByFilm(filmId)).Returns(_films[0].VoiceActors);
         _films[0].VoiceActors.Add(new VoiceActor());

         var voiceActorCount = _filmService.GetVoiceActorsByFilm(filmId).Count;
         
         Assert.Equal(1, voiceActorCount);
     }
     
     [Fact]
     public void GetVoiceActorsByFilm_ThrowsModelNotFoundException_WhenGivenIdOfFilmWhichDoesNotExist()
     {
         _mockedFilmRepository.Setup(x => x.GetVoiceActorsByFilm(Guid.Parse("00000000-0000-0000-0000-000000000005")))
             .Throws(new ModelNotFoundException(
                 Guid.Parse("00000000-0000-0000-0000-000000000005")));
         
         Assert.Throws<ModelNotFoundException>(() => _filmService.GetVoiceActorsByFilm(Guid.Parse("00000000-0000-0000-0000-000000000005")));
     }
     
     [Fact]
     public void CreateFilm_PersistsNewlyCreatedFilm_WhenCalled()
     {
         _mockedFilmRepository
             .Setup(x => x.CreateFilm("Title", "Description", "Director", "Composer", 2000))
             .Callback((string title, string description, string director, string composer, int releaseYear) =>
             {
                 var newFilm = new Film
                 {
                     Title = ValidatedString.From(title),
                     Description = ValidatedString.From(description),
                     Director = ValidatedString.From(director),
                     Composer = ValidatedString.From(composer),
                     ReleaseYear = ReleaseYear.From(releaseYear)
                 };
        
                 _films.Add(newFilm);
             });
         
         _filmService.CreateFilm("Title", "Description", "Director", "Composer", 2000);
         _mockedFilmRepository.Setup(x => x.GetFilmById(_films[2].Id)).Returns(_films[2]);

         var filmId = _films[2].Id;
         var filmCount = _films.Count;
         var film = _films[2];
         
         Assert.Equal(3, filmCount);
         Assert.Equal(filmId, film.Id);
     }
     
     [Fact]
     public void CreateFilm_ThrowsArgumentException_WhenGivenEmptyInputs()
     {
         _mockedFilmRepository.Setup(x => x.CreateFilm("", "", "", "", 2001)).Throws(new ArgumentException());
         
         Assert.Throws<ArgumentException>(() => _filmService.CreateFilm("", "", "", "", 2001));
     }
     
     [Fact]
     public void CreateFilm_ThrowsReleaseYearLessThanOldestReleaseYearException_WhenGivenReleaseYearLesserThan1984()
     {
         _mockedFilmRepository.Setup(x => x.CreateFilm("test", "test", "test", "test", 1983))
             .Throws(new ReleaseYear.ReleaseYearLessThanOldestReleaseYearException(1983));
         
         Assert.Throws<ReleaseYear.ReleaseYearLessThanOldestReleaseYearException>(() => _filmService.CreateFilm("test", "test", "test", "test", 1983));
     }
     
     [Fact]
     public void CreateFilm_ThrowsNotFourCharactersException_WhenGivenReleaseYearWhichIsNotOfLengthFour()
     {
         _mockedFilmRepository.Setup(x => x.CreateFilm("test", "test", "test", "test", 200))
             .Throws(new ReleaseYear.NotFourCharactersException(200));
         
         Assert.Throws<ReleaseYear.NotFourCharactersException>(() => _filmService.CreateFilm("test", "test", "test", "test", 200));
     }
     
     [Fact]
     public void UpdateFilm_UpdatesFilmFields_WhenPassedAFilmInstance()
     {
         var filmId = _films[0].Id;
         var newFilm = new Film
         {
             Title = ValidatedString.From("UpdatedTitle"),
             Description = ValidatedString.From("UpdatedDescription"),
             Director = ValidatedString.From("UpdatedDirector"),
             Composer = ValidatedString.From("UpdatedComposer"),
             ReleaseYear = ReleaseYear.From(2001)
             
         };
         _mockedFilmRepository.Setup(x => x.UpdateFilm(filmId, newFilm)).Callback(() =>
         {
             _films[0].Title = newFilm.Title;
             _films[0].Description = newFilm.Description;
             _films[0].Director = newFilm.Director;
             _films[0].Composer = newFilm.Composer;
             _films[0].ReleaseYear = newFilm.ReleaseYear;

         });
         
         _filmService.UpdateFilm(filmId, newFilm);
         var filmWithUpdatedFields = _films[0];

         Assert.Equal(ValidatedString.From("UpdatedTitle"), filmWithUpdatedFields.Title);
         Assert.Equal(ValidatedString.From("UpdatedDescription"), filmWithUpdatedFields.Description);
         Assert.Equal(ValidatedString.From("UpdatedDirector"), filmWithUpdatedFields.Director);
         Assert.Equal(ValidatedString.From("UpdatedComposer"), filmWithUpdatedFields.Composer);
         Assert.Equal(ReleaseYear.From(2001), filmWithUpdatedFields.ReleaseYear);
     }
     
     [Fact]
     public void DeleteFilm_RemovesFilmWithMatchingIdFromFilmList_WhenGivenFilmId()
     {
         var filmId = _films[0].Id;
         _mockedFilmRepository.Setup(x => x.DeleteFilm(filmId)).Callback(() => _films.Remove(_films[0]));

         _filmService.DeleteFilm(filmId);
         var voiceActorCount = _films.Count;
         
         Assert.Equal(1, voiceActorCount);
     }

     [Fact]
     public void LinkVoiceActor_LinksVoiceActorToAFilm_WhenCalled()
     {
         var filmToHaveVoiceActorAddedId = _films[0].Id;
         var voiceActor = new VoiceActor();
         _mockedFilmRepository.Setup(x => x.LinkVoiceActor(filmToHaveVoiceActorAddedId, voiceActor.Id))
             .Callback(() => _films[0].VoiceActors.Add(new VoiceActor()));
         
         _filmService.LinkVoiceActor(filmToHaveVoiceActorAddedId, voiceActor.Id);
         var filmVoiceActorCount = _films[0].VoiceActors.Count;
         
         Assert.Equal(1, filmVoiceActorCount);
     }
     
     [Fact]
     public void DeleteFilm_ThrowsModelNotFoundException_WhenGivenNonExistentFilmId()
     {
         _mockedFilmRepository.Setup(x => x.DeleteFilm(Guid.Parse("00000000-0000-0000-0000-000000000005")))
             .Throws(new ModelNotFoundException(Guid.Parse("00000000-0000-0000-0000-000000000005")));
         
         Assert.Throws<ModelNotFoundException>(() =>
             _filmService.DeleteFilm(Guid.Parse("00000000-0000-0000-0000-000000000005")));
     }
}