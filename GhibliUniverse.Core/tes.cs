using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Repository;

namespace GhibliUniverse.Core;

public class tes
{
    private readonly IFilmRepository _filmRepository;

    public tes(FilmRepository filmRepository)
    {
        _filmRepository = filmRepository;
    }

    public List<Film> getAll()
    {
        return _filmRepository.GetAllFilms();
    }
}