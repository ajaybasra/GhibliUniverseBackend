using AutoMapper;
using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.DataEntities;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GhibliUniverse.Core.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly GhibliUniverseContext _ghibliUniverseContext;
    private readonly IMapper _mapper;

    public ReviewRepository(GhibliUniverseContext ghibliUniverseContext, IMapper mapper)
    {
        _ghibliUniverseContext = ghibliUniverseContext;
        _mapper = mapper;
    }

    public async Task<List<Review>> GetAllReviews()
    {
        var reviews = await _ghibliUniverseContext.Reviews.ToListAsync();
        return _mapper.Map<List<Review>>(reviews);
    }

    public async Task<Review> GetReviewById(Guid reviewId)
    {
        var review = await _ghibliUniverseContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (review == null)
        {
            throw new ModelNotFoundException(reviewId);
        }

        return _mapper.Map<Review>(review);
    }
    public async Task<Review> CreateReview(Guid filmId, Review reviewCreateRequest)
    {
        var film = await _ghibliUniverseContext.Films.FirstOrDefaultAsync(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }
        
        var review = new ReviewEntity
        {
            Id = Guid.NewGuid(),
            Rating = reviewCreateRequest.Rating.Value,
            FilmId = filmId
        };

        _ghibliUniverseContext.Reviews.Add(review);
        await _ghibliUniverseContext.SaveChangesAsync();

        return _mapper.Map<Review>(review);
    }

    public async Task<Review> UpdateReview(Guid reviewId, Review reviewUpdateRequest)
    {
        var reviewToUpdate = await _ghibliUniverseContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (reviewToUpdate == null)
        {
            throw new ModelNotFoundException(reviewId);
        }
        
        reviewToUpdate.Rating = reviewUpdateRequest.Rating.Value;
        _ghibliUniverseContext.Reviews.Update(reviewToUpdate);
        await _ghibliUniverseContext.SaveChangesAsync();
        return _mapper.Map<Review>(reviewToUpdate);
    }

    public async Task DeleteReview(Guid reviewId)
    {
        var reviewToDelete = await _ghibliUniverseContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (reviewToDelete == null)
        {
            throw new ModelNotFoundException(reviewId);
        }

        _ghibliUniverseContext.Reviews.Remove(reviewToDelete);
        await _ghibliUniverseContext.SaveChangesAsync();
    }
}
