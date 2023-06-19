using System.Text;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
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
    
    public List<Film> GetAllFilms()
    {
        return _filmRepository.GetAllFilms();
    }

    public Film GetFilmById(Guid filmId)
    {
        return _filmRepository.GetFilmById(filmId);
    }

    public List<VoiceActor> GetVoiceActorsByFilm(Guid filmId)
    {
        return _filmRepository.GetVoiceActorsByFilm(filmId);
    }

    public Film CreateFilm(string title, string description, string director, string composer, int releaseYear)
    {
        return _filmRepository.CreateFilm(title, description, director, composer, releaseYear);
    }

    public Film UpdateFilm(Guid filmId, Film updatedFilm)
    {
        return _filmRepository.UpdateFilm(filmId, updatedFilm);
    }

    public void DeleteFilm(Guid filmId)
    {
        _filmRepository.DeleteFilm(filmId);
    }
    
    public void LinkVoiceActor(Guid filmId, Guid voiceActorId)  
    {
        _filmRepository.LinkVoiceActor(filmId, voiceActorId);
    }

    public void UnlinkVoiceActor(Guid filmId, Guid voiceActorId)
    {
       _filmRepository.UnlinkVoiceActor(filmId, voiceActorId);
    }

    public bool FilmIdAlreadyExists(Guid filmId)
    {
        var filmsWithMatchingId = GetAllFilms().FirstOrDefault(f => f.Id == filmId);
        return filmsWithMatchingId != null;
    }

    public bool FilmTitleAlreadyExists(string title)
    {
        var filmsWithMatchingTitle = GetAllFilms().FirstOrDefault(f => f.Title == ValidatedString.From(title));
        return filmsWithMatchingTitle != null;
    }

    public string BuildFilmList()
    {
        var films = GetAllFilms();
        var stringBuilder = new StringBuilder();
        foreach (var film in films)
        {
            stringBuilder.Append(film);
            stringBuilder.Append('\n');
        }
        
        return stringBuilder.ToString();
    }
    
}