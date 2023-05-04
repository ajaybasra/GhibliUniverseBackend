using GhibliUniverse;

namespace GhibliUniverseTests;

public class FilmUniverseTests
{
    private readonly FilmUniverse _filmUniverse;
    private const int Id = 2;
    private const string Title = "Spirited Away";

    private const string Description = "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.";

    private const string Director = "Hayao Miyazaki";
    private const string Composer = "Joe Hisaishi";
    private const int ReleaseYear = 2001;
    private const int Score = 10;

    public FilmUniverseTests()
    {
        _filmUniverse = new FilmUniverse();
    }
    
    [Fact]
    public void Add_AddsNewFilmRecordToFilmUniverse_WhenCalled()
    {
        _filmUniverse.Add(Id, Title, Description, Director, Composer, ReleaseYear, Score);
        var filmUniverseCount = _filmUniverse.GetAllFilms().Count;
        
        Assert.Equivalent(2, filmUniverseCount);
    }

    [Fact]
    public void Remove_RemovesFilmWithMatchingIdFromFilmUniverse_WhenGivenFilmId()
    {
        _filmUniverse.Remove(1);
        var filmUniverseCount = _filmUniverse.GetAllFilms().Count;
        
        Assert.Equivalent(0, filmUniverseCount);
    }

    [Fact]
    public void GetFilmById_ReturnsFilmWithMatchingId_WhenGivenFilmId()
    {

        var expectedFilm = new Film
        {
            FilmId = 1,
            Title = Title,
            Description = Description,
            Director = Director,
            Composer = Composer,
            ReleaseYear = ReleaseYear,
            Score = Score
        };
        
        var actualFilm = _filmUniverse.GetFilmById(1);
        
        Assert.Equivalent(expectedFilm, actualFilm);
    }
    
    [Fact]
    public void GetAllFilms_ReturnsAllFilms_WhenCalled() //hmm
    {
        _filmUniverse.Add(Id, Title, Description, Director, Composer, ReleaseYear, Score);
        var actualFilm = _filmUniverse.GetAllFilms();
        
        Assert.Equivalent(2, actualFilm.Count);
    }

    [Fact]
    public void BuildFilmUniverse_ReturnsCorrectOutput_WhenCalled()
    {
        var expected =
            "Film { FilmId = 1, Title = Spirited Away, Description = During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts., Director = Hayao Miyazaki, Composer = Joe Hisaishi, ReleaseYear = 2001, Score = 10 }\n" +
            "Film { FilmId = 2, Title = Spirited Away, Description = During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts., Director = Hayao Miyazaki, Composer = Joe Hisaishi, ReleaseYear = 2001, Score = 10 }\n";
        
        _filmUniverse.Add(Id, Title, Description, Director, Composer, ReleaseYear, Score);
        var actual = _filmUniverse.BuildFilmUniverse();
        
        Assert.Equal(expected, "yo");
    }
}