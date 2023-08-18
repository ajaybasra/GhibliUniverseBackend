using GhibliUniverse.Core.DataEntities;
using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Repository;

public interface IFilmRepository
{
    Task<List<Film>> GetAllFilms();
    Task<Film> GetFilmById(Guid filmId);
    Task<List<VoiceActor>> GetVoiceActorsByFilm(Guid filmId);
    Task<Film> CreateFilm(Film filmCreateRequest);
    Task<Film> UpdateFilm(Guid filmId, Film filmUpdateRequest);
    Task DeleteFilm(Guid filmId);
    Task LinkVoiceActor(Guid filmId, Guid voiceActorId);
    Task UnlinkVoiceActor(Guid filmId, Guid voiceActorId);
}