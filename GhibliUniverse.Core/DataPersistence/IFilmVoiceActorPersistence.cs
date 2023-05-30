using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.DataPersistence;

public interface IFilmVoiceActorPersistence
{
    List<(Guid, Guid)> ReadFilmVoiceActorData();
    void WriteFilmVoiceActors(List<Film> films);
}