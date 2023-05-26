using GhibliUniverse.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace GhibliUniverse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmRatingController : Controller
{
    [HttpGet]
    public IActionResult GetFilmRatings()
    {
        return Ok("ye");
    }
    
    [HttpGet("{filmRatingId:guid}")]
    public IActionResult GetFilmRating(Guid filmRatingId)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("filmId:guid")]
    public IActionResult CreateFilmRating([FromRoute]Guid filmId, [FromBody]Rating rating)
    {
        return Ok("yo");
    }
    
    [HttpPut("{filmRatingId:guid}")]
    public IActionResult UpdateFilmRating(Guid filmRatingId)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("{filmRatingId:guid}")]
    public IActionResult DeleteFilmRating(Guid filmRatingId)
    {
        throw new NotImplementedException();
    }
}