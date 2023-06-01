using System.Collections;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Services;
using Moq;

namespace GhibliUniverse.Console.Tests;

public class ArgumentProcessorTests
{
    private ArgumentProcessor _argumentProcessor;

    private readonly FakeWriter _fakeWriter;
    // private readonly Mock<IWriter> _mockedWriter;
    private readonly Mock<ICommandLine> _mockedCommandLine;
    private readonly Mock<IFilmService> _mockedFilmService;
    private readonly Mock<IReviewService> _mockedReviewService;
    private readonly Mock<IVoiceActorService> _mockedVoiceActorService;

    public ArgumentProcessorTests()
    {
        _mockedCommandLine = new Mock<ICommandLine>();
        _fakeWriter = new FakeWriter();
        // _mockedWriter = new Mock<IWriter>();
        _mockedFilmService = new Mock<IFilmService>();
        _mockedReviewService = new Mock<IReviewService>();
        _mockedVoiceActorService = new Mock<IVoiceActorService>();
    }
    [Fact]
    public void Process_InvokesGetAllFilmsMethod_WhenGetAllFilmsCommandGiven()
    {
        var args = new[] { "pathname", "get-all-films" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _mockedFilmService.Setup(x => x.GetAllFilms()).Returns(new List<Film>());
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedFilmService.Verify(x => x.GetAllFilms());
    }
    
    [Fact]
    public void Process_InvokesGetFilmByIdMethod_WhenGetFilmByIdCommandGiven()
    {
        var args = new[] { "pathname", "get-film-by-id", "00000000-0000-0000-0000-000000000000" };
        _mockedCommandLine.Setup(x => x.GetCommandLineArgs()).Returns(args);
        _mockedFilmService.Setup(x => x.GetFilmById(It.IsAny<Guid>())).Returns(new Film());
        _argumentProcessor = new ArgumentProcessor(_mockedCommandLine.Object, _fakeWriter, _mockedFilmService.Object,
            _mockedReviewService.Object, _mockedVoiceActorService.Object);
        
        _argumentProcessor.Process();
        
        _mockedFilmService.Verify(x => x.GetFilmById(It.IsAny<Guid>()));
    }
    
    [Fact]
    public void Process_PrintsErrorMessage_WhenGetFilmByIdCommandGivenWithWrongInput()
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
}