using System.Collections;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;
using Moq;

namespace GhibliUniverse.Console.Tests;

public class ArgumentProcessorTests
{
    private ArgumentProcessor _argumentProcessor;

    private readonly FakeWriter _fakeWriter;
    private readonly Mock<ICommandLine> _mockedCommandLine;
    private readonly Mock<IFilmService> _mockedFilmService;
    private readonly Mock<IReviewService> _mockedReviewService;
    private readonly Mock<IVoiceActorService> _mockedVoiceActorService;

    public ArgumentProcessorTests()
    {
        _mockedCommandLine = new Mock<ICommandLine>();
        _fakeWriter = new FakeWriter();
        _mockedFilmService = new Mock<IFilmService>();
        _mockedReviewService = new Mock<IReviewService>();
        _mockedVoiceActorService = new Mock<IVoiceActorService>();
    }
    [Fact]
    public void Process_InvokesGetAllFilmsMethod_WhenGetAllFilmsCommandGiven()
    {
        var args = new[] { "pathname", "get-all-films" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _mockedFilmService.Setup(x => x.GetAllFilms()).Returns(Task.FromResult(new List<FilmWrapper>()));
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedFilmService.Verify(x => x.GetAllFilms());
    }
    
    [Fact]
    public void Process_PrintsFilmAsExpected_WhenGetAllFilmsCommandGivenWithValidData()
    {
        var filmList = new List<FilmWrapper>
        {
            new(new Film
            {
                Title = ValidatedString.From("Spirited Away"),
                Description = ValidatedString.From("Amazing movie, it is. Watch, you must."),
                Director = ValidatedString.From("Hayao Miyazaki"),
                Composer = ValidatedString.From("Joe Hisaishi"),
                ReleaseYear = ReleaseYear.From(2001),
                VoiceActors = new List<VoiceActor>
                {
                    new() { Name = ValidatedString.From("Miyu Irino") }
                },
                Reviews = new List<Review>
                {
                    new() { Rating = Rating.From(10) }
                }
            })
        };
        
        var expectedOutput = "[Title:Spirited Away,Description:Amazing movie, it is. Watch, you must.,Director:Hayao Miyazaki,Composer:Joe Hisaishi,Release Year:2001,Voice Actors:[Miyu Irino],Film Ratings:[10]]";
        var args = new[] { "pathname", "get-all-films" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _mockedFilmService.Setup(x => x.GetAllFilms()).Returns(Task.FromResult(filmList));
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);

        _argumentProcessor.Process();
        var actualOutput = _fakeWriter.GetOutput()[0].ToString();

        Assert.Equal(expectedOutput, actualOutput);
    }
    
    [Fact]
    public void Process_InvokesGetFilmByIdMethod_WhenGetFilmByIdCommandGiven()
    {
        var args = new[] { "pathname", "get-film-by-id", "00000000-0000-0000-0000-000000000000" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _mockedFilmService.Setup(x => x.GetFilmById(It.IsAny<Guid>())).Returns(Task.FromResult(It.IsAny<FilmWrapper>()));
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedFilmService.Verify(x => x.GetFilmById(It.IsAny<Guid>()));
    }
    
    [Fact]
    public void Process_PrintsErrorMessage_WhenGetFilmByIdCommandGivenWithInvalidId()
    {
        var expectedOutput = "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).";
        var args = new[] { "pathname", "get-film-by-id", "8" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);

        _argumentProcessor.Process();
        var actualOutput = _fakeWriter.GetOutput()[0];

        Assert.Equal(expectedOutput, actualOutput);

    }
    
    [Fact]
    public void Process_InvokesGetVoiceActorsByFilm_WhenGetVoiceActorsByFilmCommandGiven()
    {
        var args = new[] { "pathname", "get-voice-actors-by-film", "00000000-0000-0000-0000-000000000000" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _mockedFilmService.Setup(x => x.GetVoiceActorsByFilm(It.IsAny<Guid>())).Returns(Task.FromResult(new List<VoiceActor>()));
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedFilmService.Verify(x => x.GetVoiceActorsByFilm(It.IsAny<Guid>()));
    }
    
    [Fact]
    public void Process_InvokesCreateFilm_WhenCreateFilmCommandGiven()
    {
        var args = new[] { "pathname", "create-film", "Test Movie-This is a random movie description made for the purpose of testing.-John Doe-Hans Zimmer-2019" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedFilmService.Verify(x => x.CreateFilm(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
    }
    
    [Fact]
    public void Process_InvokesDeleteFilm_WhenDeleteFilmCommandGiven()
    {
        var args = new[] { "pathname", "delete-film", "00000000-0000-0000-0000-000000000000" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedFilmService.Verify(x => x.DeleteFilm(It.IsAny<Guid>()));
    }
    
    [Fact]
    public void Process_InvokesAddVoiceActor_WhenAddVoiceActorToFilmCommandGiven()
    {
        var args = new[] { "pathname", "link-voice-actor-to-film", "00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedFilmService.Verify(x => x.LinkVoiceActor(It.IsAny<Guid>(), It.IsAny<Guid>()));
    }
    
    [Fact]
    public void Process_InvokesRemoveVoiceActor_WhenRemoveVoiceActorFromFilmCommandGiven()
    {
        var args = new[] { "pathname", "unlink-voice-actor-from-film", "00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedFilmService.Verify(x => x.UnlinkVoiceActor(It.IsAny<Guid>(), It.IsAny<Guid>()));
    }
    
    [Fact]
    public void Process_PrintsErrorMessage_WhenUnlinkVoiceActorCommandGivenWithWrongInput()
    {
        var expectedOutput = "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).";
        var args = new[] { "pathname", "unlink-voice-actor-from-film", "00000000-0000-0000-0000-000000000000", "10" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);

        _argumentProcessor.Process();
        var actualOutput = _fakeWriter.GetOutput()[0];

        Assert.Equal(expectedOutput, actualOutput);

    }
    
    [Fact]
    public void Process_InvokesGetAllVoiceActors_WhenGetAllVoiceActorsCommandGiven()
    {
        var args = new[] { "pathname", "get-all-voice-actors" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _mockedVoiceActorService.Setup(x => x.GetAllVoiceActors()).Returns(Task.FromResult(new List<VoiceActor>()));
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);

        _argumentProcessor.Process();

        _mockedVoiceActorService.Verify(x => x.GetAllVoiceActors());
    }
    
    [Fact]
    public void Process_InvokesGetVoiceActorById_WhenGetVoiceActorByIdCommandGiven()
    {
        var args = new[] { "pathname", "get-voice-actor-by-id", "00000000-0000-0000-0000-000000000000" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedVoiceActorService.Verify(x => x.GetVoiceActorById(It.IsAny<Guid>()));
    }
    
    [Fact]
    public void Process_InvokesGetFilmsByVoiceActor_WhenGetFilmsByVoiceActorCommandGiven()
    {
        var args = new[] { "pathname", "get-films-by-voice-actor", "00000000-0000-0000-0000-000000000000" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _mockedVoiceActorService.Setup(x => x.GetFilmsByVoiceActor(It.IsAny<Guid>())).Returns(Task.FromResult(new List<Film>()));
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);

        _argumentProcessor.Process();

        _mockedVoiceActorService.Verify(x => x.GetFilmsByVoiceActor(It.IsAny<Guid>()));
    }
    
    [Fact]
    public void Process_InvokesCreateVoiceActor_WhenCreateVoiceActorCommandGiven()
    {
        var args = new[] { "pathname", "create-voice-actor", "Lebron" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedVoiceActorService.Verify(x => x.CreateVoiceActor(It.IsAny<string>()));
    }
    
    [Fact]
    public void Process_InvokesDeleteVoiceActor_WhenDeleteVoiceActorCommandGiven()
    {
        var args = new[] { "pathname", "delete-voice-actor", "00000000-0000-0000-0000-000000000000" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedVoiceActorService.Verify(x => x.DeleteVoiceActor(It.IsAny<Guid>()));
    }
    
    [Fact]
    public void Process_InvokesGetAllReviews_WhenGetAllReviewsCommandGiven()
    {
        var args = new[] { "pathname", "get-all-reviews" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _mockedReviewService.Setup(x => x.GetAllReviews()).Returns(Task.FromResult(new List<Review>()));
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedReviewService.Verify(x => x.GetAllReviews());
    }
    
    [Fact]
    public void Process_InvokesGetReviewById_WhenGetReviewByIdCommandGiven()
    {
        var args = new[] { "pathname", "get-review-by-id", "00000000-0000-0000-0000-000000000000" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedReviewService.Verify(x => x.GetReviewById(It.IsAny<Guid>()));
    }
    
    [Fact]
    public void Process_InvokesCreateReview_WhenCreateReviewCommandGiven()
    {
        var args = new[] { "pathname", "create-review", "00000000-0000-0000-0000-000000000000", "10" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedReviewService.Verify(x => x.CreateReview(It.IsAny<Guid>(), It.IsAny<int>()));
    }
    
    [Fact]
    public void Process_InvokesDeleteReview_WhenDeleteReviewCommandGiven()
    {
        var args = new[] { "pathname", "delete-review", "00000000-0000-0000-0000-000000000000" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedReviewService.Verify(x => x.DeleteReview(It.IsAny<Guid>()));
    }

    [Fact]
    public void Process_PrintsErrorMessage_WhenGivenNonExistentCommand()
    {
        var expectedOutput = "Invalid command(s)!";
        var args = new[] { "pathname", "random" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        var actualOutput = _fakeWriter.GetOutput()[0];
        
        Assert.Equal(expectedOutput, actualOutput);
    }
    
    
}   