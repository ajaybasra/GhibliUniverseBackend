using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Repository;

public interface IFilmRepository
{
    Task<List<Film>> GetAllFilmsAsync();
    Task<Film> GetFilmByIdAsync(Guid filmId);
    Task<List<VoiceActor>> GetVoiceActorsByFilmAsync(Guid filmId);
    Task<Film> CreateFilmAsync(string title, string description, string director, string composer, int releaseYear);
    Task<Film> UpdateFilmAsync(Guid filmId, Film film);
    Task DeleteFilmAsync(Guid filmId);
    Task LinkVoiceActorAsync(Guid filmId, Guid voiceActorId);
    Task UnlinkVoiceActorAsync(Guid filmId, Guid voiceActorId);
}