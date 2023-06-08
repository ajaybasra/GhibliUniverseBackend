using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.DataPersistence;

public interface IFilmPersistence
{
    List<Film> ReadFilms();
    void WriteFilms(List<Film> films);
}