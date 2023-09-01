using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "Get All Reviews", Description = "Get all reviews.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns all existing reviews.")]
    public async Task<IActionResult> GetAllReviews()
    {
        var reviews = await _reviewService.GetAllReviews();
        var reviewsResponseDTO = _mapper.Map<List<ReviewResponseDTO>>(reviews);
        return Ok(reviewsResponseDTO);
    }
    
    [HttpGet("{reviewId:guid}")]
    [SwaggerOperation(Summary = "Get Review", Description = "Get review by id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns review matching specified id.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Review with specified id cannot be found.")]
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
    [SwaggerOperation(Summary = "Create Review", Description = "Create review.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns newly created review.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Film with specified id cannot be found.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Request was formatted incorrectly.")]
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
    [SwaggerOperation(Summary = "Update Review", Description = "Update review by id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Review updated successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Review with specified id cannot be found.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Request was formatted incorrectly.")]
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
    [SwaggerOperation(Summary = "Delete Review", Description = "Delete review by id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Review deleted successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Review with specified id cannot be found.")]
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