using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace GhibliUniverse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmRatingController : Controller
{
    [HttpGet]
    public IActionResult GetAllReviews()
    {
        return Ok("ye");
    }
    
    [HttpGet("{reviewId:guid}")]
    public IActionResult GetReviewById(Guid reviewId)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("filmId:guid")]
    public IActionResult CreateReview(Guid filmId, [FromBody]Rating rating)
    {
        return Ok("yo");
    }
    
    [HttpPut("{filmRatingId:guid}")]
    public IActionResult UpdateReview(Guid reviewId)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("{filmRatingId:guid}")]
    public IActionResult DeleteReview(Guid reviewId)
    {
        throw new NotImplementedException();
    }
}