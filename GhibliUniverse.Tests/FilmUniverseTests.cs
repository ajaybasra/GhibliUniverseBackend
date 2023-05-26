using GhibliUniverse;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;

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

    public FilmUniverseTests()
    {
        _filmUniverse = new FilmUniverse();
        _filmUniverse.PopulateFilmsList(3);
        _filmUniverse.PopulateVoiceActorsList(2);
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
        var expectedId = new Guid("00000000-0000-0000-0000-000000000000");

        var actualFilm = _filmUniverse.GetFilmById(new Guid("00000000-0000-0000-0000-000000000000"));

        Assert.Equal(expectedId, actualFilm.Id);
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
        var voiceActorsCount = _filmUniverse.GetAllVoiceActors().Count;
        
        Assert.Equal(8, voiceActorsCount);
    }

    [Fact]
    public void GetVoiceActorById_ReturnsVoiceActorWithMatchingId_WhenGivenVoiceActorId()
    {
        var voiceActorId = _filmUniverse.GetAllVoiceActors()[0].Id;

        var expectedVoiceActor = new VoiceActor()
        {
            Id = voiceActorId,
            Name = ValidatedString.From("John Doe")
        };

        var actualVoiceActor = _filmUniverse.GetVoiceActorById(voiceActorId);
        
        Assert.Equivalent(expectedVoiceActor, actualVoiceActor);
    }

    [Fact]
    public void CreateVoiceActor_AddsNewRecordToVoiceActorList_WhenCalled()
    {
        _filmUniverse.CreateVoiceActor("John Doe");
        var voiceActorId = _filmUniverse.GetAllVoiceActors()[2].Id;
        
        var voiceActorCount = _filmUniverse.GetAllVoiceActors().Count;
        var voiceActor = _filmUniverse.GetVoiceActorById(voiceActorId);
        
        Assert.Equal(9, voiceActorCount);
        Assert.Equal(voiceActorId, voiceActor.Id);
    }

    [Fact]
    public void DeleteVoiceActor_RemovesActorWithMatchingIdFromVoiceActorList_WhenGivenVoiceActorId()
    {
        var voiceActorId = _filmUniverse.GetAllVoiceActors()[0].Id;

        
        _filmUniverse.DeleteVoiceActor(voiceActorId);
        var voiceActorCount = _filmUniverse.GetAllVoiceActors().Count;
        
        Assert.Equal(7, voiceActorCount);
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
        var filmRatingId = _filmUniverse.GetAllFilmRatings(new Guid("00000000-0000-0000-0000-000000000000"))[0].Id;

        var expectedFilmRating = new FilmRating()
        {
            Id = filmRatingId,
            Rating = Rating.From(10),
            FilmId = new Guid("00000000-0000-0000-0000-000000000000")
        };

        var actualFilmRating = _filmUniverse.GetFilmRatingById(new Guid("00000000-0000-0000-0000-000000000000"),
            filmRatingId);

        Assert.Equivalent(expectedFilmRating, actualFilmRating);
    }

    [Fact]
    public void CreateFilmRating_AddsNewRecordToFilmRatingList_WhenCalled()
    {
        _filmUniverse.CreateFilmRating(10, new Guid("11111111-1111-1111-1111-111111111111"));
        var filmRatingId = _filmUniverse.GetAllFilmRatings(new Guid("11111111-1111-1111-1111-111111111111"))[2].Id;

        
        var filmRatingCount = _filmUniverse.GetAllFilmRatings(new Guid("11111111-1111-1111-1111-111111111111")).Count;
        var filmRating = _filmUniverse.GetFilmRatingById(new Guid("11111111-1111-1111-1111-111111111111"),
            filmRatingId);
        
        Assert.Equal(3, filmRatingCount);
        Assert.Equal(filmRatingId, filmRating.Id);
    }

    [Fact]
    public void DeleteFilmRating_RemovesRatingWithMatchingIdFromFilmRatingList_WhenGivenFilmRatingId()
    {
        var filmRatingId = _filmUniverse.GetAllFilmRatings(new Guid("00000000-0000-0000-0000-000000000000"))[0].Id;
        
        _filmUniverse.DeleteFilmRating(new Guid("00000000-0000-0000-0000-000000000000"),
            filmRatingId);
        var filmRatingCount = _filmUniverse.GetAllFilmRatings(new Guid("00000000-0000-0000-0000-000000000000")).Count;
        
        Assert.Equal(1, filmRatingCount);
    }

    [Fact]
    public void FilterFilmsByField_ReturnsCorrectlyFilteredList_WhenCalled()
    {
        var filteredFilms = _filmUniverse.GetFilmsFilteredByProperty("Title", "Spirited Away");
        
        Assert.Single(filteredFilms);
    }

    [Fact]
    public void BuildVoiceActorList_ReturnsCorrectOutput_WhenCalled()
    {
        var expected = "John Doe\nJohn Doe\nJohn Doe\nJohn Doe\nJohn Doe\nJohn Doe\nJohn Doe\nJohn Doe\n";

        var actual = _filmUniverse.BuildVoiceActorList();
        
        Assert.Equal(expected, actual);
    }
}