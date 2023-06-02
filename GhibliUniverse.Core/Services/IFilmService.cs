using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Services;

public interface IFilmService
{
    List<Film> GetAllFilms();
    Film GetFilmById(Guid filmId);
    List<VoiceActor> GetVoiceActorsByFilm(Guid filmId);
    void CreateFilm(string title, string description, string director, string composer, int releaseYear);
    void UpdateFilm(Guid filmId, Film film);
    void DeleteFilm(Guid filmId);
    void LinkVoiceActor(Guid filmId, Guid voiceActorId);
    void UnlinkVoiceActor(Guid filmId, Guid voiceActorId);
    bool FilmIdAlreadyExists(Guid filmId);
    bool FilmTitleAlreadyExists(string title);
}