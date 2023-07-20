using GhibliUniverse.Core.Context;
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
        return await _ghibliUniverseContext.Reviews.ToListAsync();
    }

    public async Task<Review> GetReviewById(Guid reviewId)
    {
        var review = await _ghibliUniverseContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (review == null)
        {
            throw new ModelNotFoundException(reviewId);
        }

        return review;
    }
    public async Task<Review> CreateReview(Guid filmId, int rating)
    {
        var film = await _ghibliUniverseContext.Films.FirstOrDefaultAsync(f => f.Id == filmId);
        if (film == null)
        {
            throw new ModelNotFoundException(filmId);
        }
        
        var review = new Review
        {
            Id = Guid.NewGuid(),
            Rating = Rating.From(rating),
            FilmId = filmId
        };

        _ghibliUniverseContext.Reviews.Add(review);
        await _ghibliUniverseContext.SaveChangesAsync();

        return review;
    }

    public async Task<Review> UpdateReview(Guid reviewId, int rating)
    {
        var reviewToUpdate = await _ghibliUniverseContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (reviewToUpdate == null)
        {
            throw new ModelNotFoundException(reviewId);
        }
        
        reviewToUpdate.Rating = Rating.From(rating);
        _ghibliUniverseContext.Reviews.Update(reviewToUpdate);
        await _ghibliUniverseContext.SaveChangesAsync();
        return reviewToUpdate;
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
