using GhibliUniverse;

namespace GhibliUniverseTests;

public class FilmTests
{
    [Fact]
    public void AddVoiceActor_AddsVoiceActorToListOfVoiceActorsInFilmAndAddsCurrentFilmInstanceToListOfFilmsInVoiceActor_WhenCalled()
    {
        var film = new Film();
        var voiceActor = new VoiceActor();
        
        film.AddVoiceActor(voiceActor);
        
        Assert.Single(film.VoiceActors);
        Assert.Single(voiceActor.Films);
    }

    [Fact]
    public void
        RemoveVoiceActor_RemovesVoiceActorFromListOfVoiceActorsInFilmAndRemovesCurrentFilmInstanceFromListOfFilmsInVoiceActor_WhenCalled()
    {
        var film = new Film();
        var voiceActor = new VoiceActor();
        
        film.AddVoiceActor(voiceActor);
        film.RemoveVoiceActor(voiceActor);
        
        Assert.Empty(film.VoiceActors);
        Assert.Empty(voiceActor.Films);
    }
    
    [Fact]
    public void AddVoiceActor_DoesNotAddVoiceActorWhichAlreadyExistsInVoiceActorsList_WhenCalled()
    {
        var film = new Film();
        var voiceActor = new VoiceActor();
        
        film.AddVoiceActor(voiceActor);
        film.AddVoiceActor(voiceActor);

        Assert.Single(film.VoiceActors);
    }
}
 