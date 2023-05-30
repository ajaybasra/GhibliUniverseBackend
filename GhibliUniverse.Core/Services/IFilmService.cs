using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Services;

public interface IFilmService
{
    List<Film> GetAllFilms();
    Film GetFilmById(Guid filmId);
    List<VoiceActor> GetVoiceActorsByFilm(Guid filmId);
    public void CreateFilm(string title, string description, string director, string composer, int releaseYear);
    public void UpdateFilm(Guid filmId, Film film);
    public void DeleteFilm(Guid filmId);
    void AddFilm(Film film);
    List<Film> AddReviewsToFilms();
}