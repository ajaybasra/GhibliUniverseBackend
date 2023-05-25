using GhibliUniverse.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace GhibliUniverse.API.Controllers;
// the two below are data  attributes
[ApiController]
[Route("api/[controller]")] // this is the route
public class FilmController : Controller // db repo would be injected into constructor
{

    [HttpGet]
    public IActionResult GetFilms()
    {
        return Ok("ye");
    }

    [HttpGet("{filmId:guid}")]
    public IActionResult GetFilm(Guid filmId)
    {
        throw new NotImplementedException();
    }
    
    // [HttpGet("{filmId:guid}/filmRatings")]
    // public IActionResult GetFilmRatings(Guid filmId)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // [HttpGet("{filmId:guid}/filmRatings/{filmRatingId:guid}")]
    // public IActionResult GetFilmRating(Guid filmId, Guid filmRatingId)
    // {
    //     throw new NotImplementedException();
    // }
    
    [HttpGet("{filmId:guid}/voiceActors")]
    public IActionResult GetVoiceActorsByFilm(Guid filmId)
    {
        throw new NotImplementedException();
    }
// https://localhost:7212/api/film?title=Ponyo&description=wasd&director=lebron&composer=mj&releaseYear=2016
    [HttpPost]
    public IActionResult CreateFilm([FromQuery]ValidatedString title, [FromQuery]ValidatedString description, [FromQuery]ValidatedString director, [FromQuery]ValidatedString composer, [FromQuery]ReleaseYear releaseYear)
    {
        return Ok("yo");
    }

    [HttpPut("{filmId:guid}")]
    public IActionResult UpdateFilm(Guid filmId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{filmId:guid}")]
    public IActionResult DeleteFilm(Guid filmId)
    {
        throw new NotImplementedException();
    }
    
    
}