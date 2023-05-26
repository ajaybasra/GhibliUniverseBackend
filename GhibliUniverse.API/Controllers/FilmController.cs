using GhibliUniverse.Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GhibliUniverse.API.Controllers;

[ApiController]
[Route("api/[controller]")]  
public class FilmController : Controller  
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
// https://localhost:7212/api/film
    [HttpPost]
    public IActionResult CreateFilm([FromBody]Film film)
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