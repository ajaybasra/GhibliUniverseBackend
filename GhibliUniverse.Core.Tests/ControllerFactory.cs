using AutoMapper;
using GhibliUniverse.API.Controllers;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Core.Tests;

public static class ControllerFactory
{
    public static FilmController GenerateFilmController(IFilmService filmService,  IMapper mapper)
    {

        var filmController = new FilmController(filmService, mapper);

        return filmController;
    }

    public static ReviewController GenerateReviewController(IReviewService reviewService, IMapper mapper)
    {
        var reviewController = new ReviewController(reviewService, mapper);

        return reviewController;
    }
}