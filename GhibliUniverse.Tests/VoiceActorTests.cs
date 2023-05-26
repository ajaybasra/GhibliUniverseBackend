using GhibliUniverse.WebAPI;
using GhibliUniverse.WebAPI.Domain.Models;

namespace GhibliUniverse.Tests;

public class VoiceActorTests
{
    [Fact]
    public void AddFilm_AddsFilmToListOfFilmsInVoiceActor_WhenCalled()
    {
        var voiceActor = new VoiceActor();
        var film = new Film();
        
        voiceActor.AddFilm(film);
        
        Assert.Single(voiceActor.Films);
    }

    [Fact]
    public void RemoveFilm_RemovesFilmFromListOfFilmsInVoiceActor_WhenCalled()
    {
        var voiceActor = new VoiceActor();
        var film = new Film();
        
        voiceActor.AddFilm(film);
        voiceActor.RemoveFilm(film);
        
        Assert.Empty(voiceActor.Films);
    }

    [Fact]
    public void AddFilm_DoesNotAddFilmWhichAlreadyExistsInFilmsList_WhenCalled()
    {
        var voiceActor = new VoiceActor();
        var film = new Film();
        
        voiceActor.AddFilm(film);
        voiceActor.AddFilm(film);

        Assert.Single(voiceActor.Films);
    }
}
