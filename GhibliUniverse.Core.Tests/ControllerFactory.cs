using AutoMapper;
using GhibliUniverse.API.Controllers;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Core.Tests;

public class ControllerFactory
{
    public static FilmController GenerateFilmController(IFilmService filmService,  IMapper mapper)
    {

        var filmController = new FilmController(filmService, mapper);

        return filmController;
    }
}