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
    public async Task<IActionResult> GetAllReviews()
    {
        var reviews = await _reviewService.GetAllReviews();
        var reviewsResponseDTO = _mapper.Map<List<ReviewResponseDTO>>(reviews);
        return Ok(reviewsResponseDTO);
    }
    
    [HttpGet("{reviewId:guid}")]
    public async Task<IActionResult> GetReviewById(Guid reviewId)
    {
        try
        {
            var review = await _reviewService.GetReviewById(reviewId);
            var reviewResponseDTO = _mapper.Map<ReviewResponseDTO>(review);
            return Ok(reviewResponseDTO);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No review found with the following id: " + reviewId);
        }
    }

    [HttpPost("{filmId:guid}")] 
    public async Task<IActionResult> CreateReview(Guid filmId, [FromBody] ReviewRequestDTO reviewCreate)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var reviewCreateRequestAsValueObject = _mapper.Map<Review>(reviewCreate);
            var createdReview = await _reviewService.CreateReview(filmId, reviewCreateRequestAsValueObject);
            var reviewResponseDto = _mapper.Map<ReviewResponseDTO>(createdReview);
            return Ok(reviewResponseDto);
        }
        catch (ModelNotFoundException)
        {
            return  NotFound("No film found with the following id: " + filmId);
        }
        catch (AutoMapperMappingException ex)
        {
            return BadRequest(ex.InnerException.Message);
        }
    }
    
    [HttpPut("{reviewId:guid}")]
    public async Task<IActionResult> UpdateReview(Guid reviewId, [FromBody] ReviewRequestDTO reviewUpdate)
    {
        try
        {
            var reviewUpdateRequestAsValueObject = _mapper.Map<Review>(reviewUpdate);
            var updatedReview = await _reviewService.UpdateReview(reviewId, reviewUpdateRequestAsValueObject);
            var reviewResponseDTO = _mapper.Map<ReviewResponseDTO>(updatedReview);
            return Ok(reviewResponseDTO);
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No review found with the following id: " + reviewId);
        }
        catch (AutoMapperMappingException ex)
        {
            return BadRequest(ex.InnerException.Message);
        }
        
    }
    
    [HttpDelete("{reviewId:guid}")]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        try
        {
            await _reviewService.DeleteReview(reviewId);
            return Ok("Successfully deleted review");
        }
        catch (ModelNotFoundException)
        {
            return NotFound("No review found with the following id: " + reviewId);
        }
    }
}