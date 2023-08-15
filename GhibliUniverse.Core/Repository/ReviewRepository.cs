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

    public ReviewRepository(GhibliUniverseContext ghibliUniverseContext)
    {
        _ghibliUniverseContext = ghibliUniverseContext;
    }

    public async Task<List<Review>> GetAllReviews()
    {
        var reviews = await _ghibliUniverseContext.Reviews.ToListAsync();
        return reviews.Select(r => new Review(r)).ToList();
    }

    public async Task<Review> GetReviewById(Guid reviewId)
    {
        var review = await _ghibliUniverseContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (review == null)
        {
            throw new ModelNotFoundException(reviewId);
        }

        return new Review(review);
    }
    public async Task<Review> CreateReview(Guid filmId, int rating)
    {
        var film = await _ghibliUniverseContext.Films.FirstOrDefaultAsync(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }
        
        var review = new ReviewEntity
        {
            Id = Guid.NewGuid(),
            Rating = rating,
            FilmId = filmId
        };

        _ghibliUniverseContext.Reviews.Add(review);
        await _ghibliUniverseContext.SaveChangesAsync();

        return new Review(review);
    }

    public async Task<Review> UpdateReview(Guid reviewId, int rating)
    {
        var reviewToUpdate = await _ghibliUniverseContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (reviewToUpdate == null)
        {
            throw new ModelNotFoundException(reviewId);
        }
        
        reviewToUpdate.Rating = rating;
        _ghibliUniverseContext.Reviews.Update(reviewToUpdate);
        await _ghibliUniverseContext.SaveChangesAsync();
        return new Review(reviewToUpdate);
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
