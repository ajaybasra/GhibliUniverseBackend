using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Services;

public interface IFilmService
{
    Task<List<Film>> GetAllFilms();
    Task<Film> GetFilmById(Guid filmId);
    Task<List<VoiceActor>> GetVoiceActorsByFilm(Guid filmId);
    Task<List<FilmReviewInfo>> GetFilmReviewsInfo();
    Task<Film> CreateFilm(string title, string description, string director, string composer, int releaseYear);
    Task<Film> UpdateFilm(Guid filmId, Film film);
    Task DeleteFilm(Guid filmId);
    Task LinkVoiceActor(Guid filmId, Guid voiceActorId);
    Task UnlinkVoiceActor(Guid filmId, Guid voiceActorId);
    Task<bool> FilmTitleAlreadyExists(string title);
}