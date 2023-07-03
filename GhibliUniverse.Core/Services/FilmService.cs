using System.Text;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace GhibliUniverse.Core.Services;

public class FilmService : IFilmService
{
    private readonly IFilmRepository _filmRepository;

    public FilmService(IFilmRepository filmRepository)
    {
        _filmRepository = filmRepository;
    }
    
    public async Task<List<Film>> GetAllFilmsAsync()
    {
        return await _filmRepository.GetAllFilmsAsync();
    }

    public async Task<Film> GetFilmByIdAsync(Guid filmId)
    {
        return await _filmRepository.GetFilmByIdAsync(filmId);
    }

    public async Task<List<VoiceActor>> GetVoiceActorsByFilmAsync(Guid filmId)
    {
        return await _filmRepository.GetVoiceActorsByFilmAsync(filmId);
    }

    public async Task<Film> CreateFilmAsync(string title, string description, string director, string composer, int releaseYear)
    {
        return await _filmRepository.CreateFilmAsync(title, description, director, composer, releaseYear);
    }

    public async Task<Film> UpdateFilmAsync(Guid filmId, Film updatedFilm)
    {
        return await _filmRepository.UpdateFilmAsync(filmId, updatedFilm);
    }

    public async Task DeleteFilmAsync(Guid filmId)
    {
        await _filmRepository.DeleteFilmAsync(filmId);
    }
    
    public async Task LinkVoiceActorAsync(Guid filmId, Guid voiceActorId)  
    {
        await _filmRepository.LinkVoiceActorAsync(filmId, voiceActorId);
    }

    public async Task UnlinkVoiceActorAsync(Guid filmId, Guid voiceActorId)
    {
       await _filmRepository.UnlinkVoiceActorAsync(filmId, voiceActorId);
    }
    
    public async Task<bool> FilmIdAlreadyExistsAsync(Guid filmId)
    {
        var films = await GetAllFilmsAsync();
        return films.Any(f => f.Id == filmId);
    }

    public async Task<bool> FilmTitleAlreadyExistsAsync(string title)
    {
        var films = await GetAllFilmsAsync();
        return films.Any(f => f.Title == ValidatedString.From(title));
    }

    public async Task<string> BuildFilmListAsync()
    {
        var films = await GetAllFilmsAsync();
        var stringBuilder = new StringBuilder();
        foreach (var film in films)
        {
            stringBuilder.Append(film);
            stringBuilder.Append('\n');
        }
        
        return stringBuilder.ToString();
    }
    
}