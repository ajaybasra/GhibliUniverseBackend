using System.Text;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Repository;

namespace GhibliUniverse.Core.Services;

public class FilmService : IFilmService
{
    private readonly IFilmRepository _filmRepository;

    public FilmService(IFilmRepository filmRepository)
    {
        _filmRepository = filmRepository;
    }
    
    public async Task<List<Film>> GetAllFilms()
    {
        var films = await _filmRepository.GetAllFilms();
        films.ForEach(CalculateAndSetReviewInfo);
        return films;
    }

    public async Task<Film> GetFilmById(Guid filmId)
    {
        var film = await _filmRepository.GetFilmById(filmId);
        CalculateAndSetReviewInfo(film);
        return film;
    }

    public async Task<List<VoiceActor>> GetVoiceActorsByFilm(Guid filmId)
    {
        return await _filmRepository.GetVoiceActorsByFilm(filmId);
    }

    public async Task<Film> CreateFilm(Film filmRequest)
    {
        return await _filmRepository.CreateFilm(filmRequest);
    }

    public async Task<Film> UpdateFilm(Guid filmId, Film updatedFilm)
    {
        var film = await _filmRepository.UpdateFilm(filmId, updatedFilm);
        CalculateAndSetReviewInfo(film);
        return film;
    }

    public async Task DeleteFilm(Guid filmId)
    {
        await _filmRepository.DeleteFilm(filmId);
    }
    
    public async Task LinkVoiceActor(Guid filmId, Guid voiceActorId)  
    {
        await _filmRepository.LinkVoiceActor(filmId, voiceActorId);
    }

    public async Task UnlinkVoiceActor(Guid filmId, Guid voiceActorId)
    {
       await _filmRepository.UnlinkVoiceActor(filmId, voiceActorId);
    }
    
    public async Task<bool> FilmIdAlreadyExistsAsync(Guid filmId)
    {
        var films = await GetAllFilms();
        return films.Any(f => f.Id == filmId);
    }

    public async Task<bool> FilmTitleAlreadyExists(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return false;
        }
        var films = await GetAllFilms();
        return films.Any(f => f.FilmInfo.Title == ValidatedString.From(title));
    }

    public async Task<string> BuildFilmList()
    {
        var films = await GetAllFilms();
        var stringBuilder = new StringBuilder();
        foreach (var film in films)
        {
            stringBuilder.Append(film);
            stringBuilder.Append('\n');
        }
        
        return stringBuilder.ToString();
    }
    
    private double CalculateAverageRating(List<Review> reviews) 
    {
        if (reviews.Count == 0)
        {
            return 0; 
        }

        double totalRating = reviews.Sum(review => review.Rating.Value);
        return totalRating / reviews.Count;
    }
    
    private void CalculateAndSetReviewInfo(Film film)
    {
        var averageRating = CalculateAverageRating(film.FilmInfo.Reviews);
        var numberOfRatings = film.FilmInfo.Reviews.Count;
    
        film.FilmReviewInfo.AverageRating = averageRating;
        film.FilmReviewInfo.NumberOfRatings = numberOfRatings;
    }
    
}