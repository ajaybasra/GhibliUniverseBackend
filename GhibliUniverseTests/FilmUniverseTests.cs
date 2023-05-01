using GhibliUniverse;

namespace GhibliUniverseTests;

public class FilmUniverseTests
{
    private readonly FilmUniverse _filmUniverse;
    public FilmUniverseTests()
    {
        _filmUniverse = new FilmUniverse();
    }
    
    [Fact]
    public void Add_AddsNewFilmRecordToFilmUniverse_WhenCalled()
    {
        var filmDescription =
            "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.";
        _filmUniverse.Add(2, "Spirited Away", filmDescription, "Hayao Miyazaki", "Joe Hisaishi", 2001, 10);

        var filmUniverseCount = _filmUniverse.GetAllFilms().Count;
        
        Assert.Equivalent(1, filmUniverseCount);
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
        var id = 1;
        var title = "Spirited Away";
        var description =
            "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.";
        var director = "Hayao Miyazaki";
        var composer = "Joe Hisaishi";
        var releaseYear = 2001;
        var score = 10;
        var expectedFilm = new Film
        {
            FilmId = id,
            Title = title,
            Description = description,
            Director = director,
            Composer = composer,
            ReleaseYear = releaseYear,
            Score = score
        };
        
        var actualFilm = _filmUniverse.GetFilmById(1);
        
        Assert.Equivalent(expectedFilm, actualFilm);
    }
    
    [Fact]
    public void GetAllFilms_ReturnsAllFilms_WhenCalled() //hmm
    {
        _filmUniverse.Add(2, "testTitle", "testDescription", "testDirector", "testComposer", 1, 10);
        var actualFilm = _filmUniverse.GetAllFilms();
        
        Assert.Equivalent(2, actualFilm.Count);
    }
}