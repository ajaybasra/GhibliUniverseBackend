using GhibliUniverse;

namespace GhibliUniverseTests;

public class FilmUniverseTests
{
    private readonly FilmUniverse _filmUniverse;
    private static readonly Guid Id = new ("33333333-3333-3333-3333-333333333333");
    private const string Title = "Castle in the Sky";

    private const string Description = "Young orphan Sheeta and her kidnapper, Col. Muska, are flying to a military prison when their plane is attacked by a gang of air pirates led by the matronly Dola.";

    private const string Director = "Hayao Miyazaki";
    private const string Composer = "Joe Hisaishi";
    private const int ReleaseYear = 1986;

    // private static readonly List<VoiceActor> VoiceActors = new()
    // {
    //     new VoiceActor
    //     {
    //         Id = new Guid("66666666-6666-6666-6666-666666666666"),
    //         FirstName = "John",
    //         LastName = "Doe",
    //         FilmId = new Guid("44444444-4444-4444-4444-444444444444")
    //         
    //     }
    // };
    //
    // private static readonly List<FilmRating> FilmRatings = new()
    // {
    //     new FilmRating
    //     {
    //         Id = new Guid("66666666-6666-6666-6666-666666666666"),
    //         Rating = 10,
    //         FilmId = new Guid("44444444-4444-4444-4444-444444444444")
    //     }
    // };

    public FilmUniverseTests()
    {
        _filmUniverse = new FilmUniverse();
        _filmUniverse.PopulateFilmsList(3);
    }

    [Fact]
    public void GetAllFilms_ReturnsAllFilms_WhenCalled() 
    {
        var filmCount = _filmUniverse.GetAllFilms().Count;
        
        Assert.Equal(3, filmCount);
    }
    
    [Fact]
    public void GetFilmById_ReturnsFilmWithMatchingId_WhenGivenFilmId()
    {
        var expectedFilm = new Film
        {
            Id = new Guid("00000000-0000-0000-0000-000000000000"),
            Title = "Spirited Away",
            Description = "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.",
            Director = Director,
            Composer = Composer,
            ReleaseYear = 2001,
            VoiceActors = new List<VoiceActor>()
            {
                new()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000000"),
                    FirstName = "John",
                    LastName = "Doe",
                    FilmId = new Guid("00000000-0000-0000-0000-000000000000")
            
                },
                new()
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    FirstName = "John",
                    LastName = "Doe",
                    FilmId = new Guid("00000000-0000-0000-0000-000000000000")
            
                }
            },
            FilmRatings = new List<FilmRating>()
            {
                new()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000000"),
                    Rating = 10,
                    FilmId = new Guid("00000000-0000-0000-0000-000000000000")
                },
                new()
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    Rating = 10,
                    FilmId = new Guid("00000000-0000-0000-0000-000000000000")
                }
            }
        };
        
        var actualFilm = _filmUniverse.GetFilmById(new Guid("00000000-0000-0000-0000-000000000000"));

        Assert.Equivalent(expectedFilm, actualFilm);
    }

    [Fact]
    public void CreateFilm_AddsNewFilmRecordToFilmList_WhenCalled()
    {
        _filmUniverse.CreateFilm(Title, Description, Director, Composer, ReleaseYear);
        var filmUniverseCount = _filmUniverse.GetAllFilms().Count;
        
        Assert.Equivalent(4, filmUniverseCount);
    }

    [Fact]
    public void DeleteFilm_RemovesFilmWithMatchingIdFromFilmList_WhenGivenFilmId()
    {
        _filmUniverse.DeleteFilm(new Guid("11111111-1111-1111-1111-111111111111"));
        var filmUniverseCount = _filmUniverse.GetAllFilms().Count;
        
        Assert.Equivalent(2, filmUniverseCount);
    }
    
    [Fact]
    public void BuildFilmList_ReturnsCorrectOutput_WhenCalled()
    {
        var expected =
            "[Title:Spirited Away,Description:During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts.,Director:Hayao Miyazaki,Composer:Joe Hisaishi,Release Year:2001,Voice Actors:[John Doe,John Doe],Film Ratings:[10,10]]\n" +
            "[Title:My Neighbor Totoro,Description:Mei and Satsuki shift to a new house to be closer to their mother who is in the hospital. They soon become friends with Totoro, a giant rabbit-like creature who is a spirit.,Director:Hayao Miyazaki,Composer:Joe Hisaishi,Release Year:1988,Voice Actors:[John Doe,John Doe],Film Ratings:[10,10]]\n" +
            "[Title:Ponyo,Description:During a forbidden excursion to see the surface world, a goldfish princess encounters a human boy named Sosuke, who gives her the name Ponyo.,Director:Hayao Miyazaki,Composer:Joe Hisaishi,Release Year:2008,Voice Actors:[John Doe,John Doe],Film Ratings:[10,10]]\n" + 
            $"[Title:{Title},Description:{Description},Director:Hayao Miyazaki,Composer:Joe Hisaishi,Release Year:{ReleaseYear},Voice Actors:[],Film Ratings:[]]\n";
        
        _filmUniverse.CreateFilm(Title, Description, Director, Composer, ReleaseYear);
        var actual = _filmUniverse.BuildFilmList();
        
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetAllVoiceActors_ReturnsAllVoiceActors_WhenCalled()
    {
        var voiceActorsCount = _filmUniverse.GetAllVoiceActors(new Guid("00000000-0000-0000-0000-000000000000")).Count;
        
        Assert.Equal(2, voiceActorsCount);
    }

    [Fact]
    public void GetVoiceActorById_ReturnsVoiceActorWithMatchingId_WhenGivenVoiceActorId()
    {
        var expectedVoiceActor = new VoiceActor()
        {
            Id = new Guid("11111111-1111-1111-1111-111111111111"),
            FirstName = "John",
            LastName = "Doe",
            FilmId = new Guid("00000000-0000-0000-0000-000000000000")
        };

        var actualVoiceActor = _filmUniverse.GetVoiceActorById(new Guid("00000000-0000-0000-0000-000000000000"),
            new Guid("11111111-1111-1111-1111-111111111111"));
        
        Assert.Equivalent(expectedVoiceActor, actualVoiceActor);
    }

    [Fact]
    public void CreateVoiceActor_AddsNewRecordToVoiceActorList_WhenCalled()
    {
        _filmUniverse.CreateVoiceActor(new Guid("31111111-1111-1111-1111-111111111111"), "First", "Last", new Guid("11111111-1111-1111-1111-111111111111"));
        var voiceActorCount = _filmUniverse.GetAllVoiceActors(new Guid("11111111-1111-1111-1111-111111111111")).Count;

        var voiceActor = _filmUniverse.GetVoiceActorById(new Guid("11111111-1111-1111-1111-111111111111"),
            new Guid("31111111-1111-1111-1111-111111111111"));
        
        Assert.Equal(3, voiceActorCount);
        Assert.Equal(new Guid("31111111-1111-1111-1111-111111111111"), voiceActor.Id);
    }

    [Fact]
    public void DeleteVoiceActor_RemovesActorWithMatchingIdFromVoiceActorList_WhenGivenVoiceActorId()
    {
        _filmUniverse.DeleteVoiceActor(new Guid("00000000-0000-0000-0000-000000000000"),
            new Guid("11111111-1111-1111-1111-111111111111"));

        var voiceActorCount = _filmUniverse.GetAllVoiceActors(new Guid("00000000-0000-0000-0000-000000000000")).Count;
        
        Assert.Equal(1, voiceActorCount);

    }
    [Fact]
    public void GetAllFilmRatings_ReturnsAllFilmRatings_WhenCalled()
    {
        var filmReviewsCount = _filmUniverse.GetAllFilmRatings(new Guid("00000000-0000-0000-0000-000000000000")).Count;
        
        Assert.Equal(2, filmReviewsCount);
    }

    [Fact]
    public void GetFilmRatingById_ReturnsFilmRatingWithMatchingId_WhenGivenAFilmRatingId()
    {
        var expectedFilmRating = new FilmRating()
        {
            Id = new Guid("11111111-1111-1111-1111-111111111111"),
            Rating = 10,
            FilmId = new Guid("00000000-0000-0000-0000-000000000000")
        };

        var actualFilmRating = _filmUniverse.GetFilmRatingById(new Guid("00000000-0000-0000-0000-000000000000"),
            new Guid("11111111-1111-1111-1111-111111111111"));

        Assert.Equivalent(expectedFilmRating, actualFilmRating);
    }

    [Fact]
    public void CreateFilmRating_AddsNewRecordToFilmRatingList_WhenCalled()
    {
        
        _filmUniverse.CreateFilmRating(new Guid("31111111-1111-1111-1111-111111111111"),10, new Guid("11111111-1111-1111-1111-111111111111"));
        var filmRatingCount = _filmUniverse.GetAllFilmRatings(new Guid("11111111-1111-1111-1111-111111111111")).Count;
        
        var filmRating = _filmUniverse.GetFilmRatingById(new Guid("11111111-1111-1111-1111-111111111111"),
            new Guid("31111111-1111-1111-1111-111111111111"));
        
        Assert.Equal(3, filmRatingCount);
        Assert.Equal(new Guid("31111111-1111-1111-1111-111111111111"), filmRating.Id);
    }

    [Fact]
    public void DeleteFilmRating_RemovesRatingWithMatchingIdFromFilmRatingList_WhenGivenFilmRatingId()
    {
        _filmUniverse.DeleteFilmRating(new Guid("00000000-0000-0000-0000-000000000000"),
            new Guid("11111111-1111-1111-1111-111111111111"));
        
        var filmRatingCount = _filmUniverse.GetAllFilmRatings(new Guid("00000000-0000-0000-0000-000000000000")).Count;
        
        Assert.Equal(1, filmRatingCount);
    }

    [Fact]
    public void FilterFilmsByField_ReturnsCorrectlyFilteredList_WhenCalled()
    {
        var filteredFilms = _filmUniverse.GetFilmsFilteredByProperty("Title", "Spirited Away");
        
        Assert.Single(filteredFilms);
    }

}