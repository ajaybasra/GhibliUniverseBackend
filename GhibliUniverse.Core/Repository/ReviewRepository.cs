using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly GhibliUniverseContext _ghibliUniverseContext;

    public ReviewRepository(GhibliUniverseContext ghibliUniverseContext)
    {
        _ghibliUniverseContext = ghibliUniverseContext;
    }

    public List<Review> GetAllReviews()
    {
        return _ghibliUniverseContext.Reviews.ToList();
    }

    public Review GetReviewById(Guid reviewId)
    {
        var review = _ghibliUniverseContext.Reviews.FirstOrDefault(r => r.Id == reviewId);
        if (review == null)
        {
            throw new ModelNotFoundException(reviewId);
        }

        return review;
    }

    public Review CreateReview(Guid filmId, int rating)
    {
        var review = new Review
        {
            Id = Guid.NewGuid(),
            Rating = Rating.From(rating),
            FilmId = filmId
        };

        _ghibliUniverseContext.Reviews.Add(review);
        _ghibliUniverseContext.SaveChanges();

        return review;
    }

    public Review UpdateReview(Guid reviewId, int rating)
    {
        var reviewToUpdate = _ghibliUniverseContext.Reviews.FirstOrDefault(r => r.Id == reviewId);
        if (reviewToUpdate == null)
        {
            throw new ModelNotFoundException(reviewId);
        }
        
        reviewToUpdate.Rating = Rating.From(rating);
        _ghibliUniverseContext.Reviews.Update(reviewToUpdate);
        _ghibliUniverseContext.SaveChanges();
        return reviewToUpdate;
    }

    public void DeleteReview(Guid reviewId)
    {
        var reviewToDelete = _ghibliUniverseContext.Reviews.FirstOrDefault(r => r.Id == reviewId);
        if (reviewToDelete == null)
        {
            throw new ModelNotFoundException(reviewId);
        }

        _ghibliUniverseContext.Reviews.Remove(reviewToDelete);
        _ghibliUniverseContext.SaveChanges();
    }
}
