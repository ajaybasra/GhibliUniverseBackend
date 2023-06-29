using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace GhibliUniverse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : Controller
{
    private readonly IReviewService _reviewService;
    private readonly IMapper _mapper;

    public ReviewController(IReviewService reviewService, IMapper mapper)
    {
        _reviewService = reviewService;
        _mapper = mapper;
    }
    [HttpGet]
    public IActionResult GetAllReviews()
    {
        var reviews = _mapper.Map<List<ReviewResponseDTO>>(_reviewService.GetAllReviews());
        return Ok(reviews);
    }
    
    [HttpGet("{reviewId:guid}")]
    public IActionResult GetReviewById(Guid reviewId)
    {
        try
        {
            var review = _mapper.Map<ReviewResponseDTO>(_reviewService.GetReviewById(reviewId));
            return Ok(review);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No review found with the following id: " + reviewId);
        }
    }
    
    [HttpPost("{filmId:guid}")] 
    public IActionResult CreateReview(Guid filmId, [FromBody] ReviewRequestDTO reviewCreate)
    {
        if (reviewCreate == null)
        {
            return BadRequest(ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdReview = _mapper.Map<ReviewResponseDTO>(_reviewService.CreateReview(filmId, reviewCreate.rating));
            return Ok(createdReview);
        }
        catch (ModelNotFoundException)
        {
            return  NotFound("No film found with the following id: " + filmId);
        }
        catch (Rating.RatingOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("{reviewId:guid}")]
    public IActionResult UpdateReview(Guid reviewId, [FromBody] ReviewRequestDTO reviewUpdate)
    {
        if (reviewUpdate == null)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedReview = _mapper.Map<ReviewResponseDTO>(_reviewService.UpdateReview(reviewId, reviewUpdate.rating));
            return Ok(updatedReview);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No review found with the following id: " + reviewId);
        }
        catch (Rating.RatingOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpDelete("{reviewId:guid}")]
    public IActionResult DeleteReview(Guid reviewId)
    {
        try
        {
            _reviewService.DeleteReview(reviewId);
            return Ok("Successfully deleted review");
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No review found with the following id: " + reviewId);
        }
    }
}