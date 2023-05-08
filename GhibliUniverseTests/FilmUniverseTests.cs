using GhibliUniverse;

namespace GhibliUniverseTests;

public class FilmUniverseTests
{
    private readonly FilmList _filmList;
    private static readonly Guid Id = new ("22222222-2222-2222-2222-222222222222");
    private const string Title = "Spirited Away";

    private const string Description = "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.";

    private const string Director = "Hayao Miyazaki";
    private const string Composer = "Joe Hisaishi";
    private const int ReleaseYear = 2001;

    private static readonly List<VoiceActor> VoiceActors = new()
    {
        new VoiceActor
        {
            VoiceActorId = new Guid("22222222-2222-2222-2222-222222222222"),
            FirstName = "John",
            LastName = "Cena",
            FilmId = new Guid("22222222-2222-2222-2222-222222222222")
            
        }
    };

    private static readonly List<FilmRating> FilmRatings = new()
    {
        new FilmRating
        {
            FilmRatingId = new Guid("22222222-2222-2222-2222-222222222222"),
            Score = 10,
            FilmId = new Guid("22222222-2222-2222-2222-222222222222")
        }
    };

    public FilmUniverseTests()
    {
        _filmList = new FilmList();
    }
    
    [Fact]
    public void Add_AddsNewFilmRecordToFilmUniverse_WhenCalled()
    {
        _filmList.Add(Id, Title, Description, Director, Composer, ReleaseYear, VoiceActors, FilmRatings);
        var filmUniverseCount = _filmList.GetAllFilms().Count;
        
        Assert.Equivalent(2, filmUniverseCount);
    }

    [Fact]
    public void Remove_RemovesFilmWithMatchingIdFromFilmUniverse_WhenGivenFilmId()
    {
        _filmList.Remove(new Guid("11111111-1111-1111-1111-111111111111"));
        var filmUniverseCount = _filmList.GetAllFilms().Count;
        
        Assert.Equivalent(0, filmUniverseCount);
    }

    [Fact]
    public void GetFilmById_ReturnsFilmWithMatchingId_WhenGivenFilmId()
    {

        var expectedFilm = new Film
        {
            FilmId = new Guid("11111111-1111-1111-1111-111111111111"),
            Title = Title,
            Description = Description,
            Director = Director,
            Composer = Composer,
            ReleaseYear = ReleaseYear,
            VoiceActors = new List<VoiceActor>
            {
                new()
                {
                    VoiceActorId = new Guid("11111111-1111-1111-1111-111111111111"),
                    FirstName = "John",
                    LastName = "Cena",
                    FilmId = new Guid("11111111-1111-1111-1111-111111111111")
            
                }
            },
            FilmRatings = new List<FilmRating>
            {
            new()
            {
            FilmRatingId = new Guid("11111111-1111-1111-1111-111111111111"),
            Score = 10,
            FilmId = new Guid("11111111-1111-1111-1111-111111111111")
        }
        }
        };
        
        var actualFilm = _filmList.GetFilmById(new Guid("11111111-1111-1111-1111-111111111111"));

        Assert.Equivalent(expectedFilm, actualFilm);
    }
    
    [Fact]
    public void GetAllFilms_ReturnsAllFilms_WhenCalled() 
    {
        _filmList.Add(Id, Title, Description, Director, Composer, ReleaseYear, VoiceActors, FilmRatings);
        var actualFilm = _filmList.GetAllFilms();
        
        Assert.Equal(2, actualFilm.Count);
    }

    [Fact]
    public void BuildFilmUniverse_ReturnsCorrectOutput_WhenCalled()
    {
        var expected =
            "[Title:Spirited Away,Description:During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.,Director:Hayao Miyazaki,Composer:Joe Hisaishi,Release Year:2001,Voice Actors:[John Cena],Film Ratings:[10]\n" +
            "[Title:Spirited Away,Description:During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.,Director:Hayao Miyazaki,Composer:Joe Hisaishi,Release Year:2001,Voice Actors:[John Cena],Film Ratings:[10]\n";
        
        _filmList.Add(Id, Title, Description, Director, Composer, ReleaseYear, VoiceActors, FilmRatings);
        var actual = _filmList.BuildFilmUniverse();
        
        Assert.Equal(expected, actual);
    }
}