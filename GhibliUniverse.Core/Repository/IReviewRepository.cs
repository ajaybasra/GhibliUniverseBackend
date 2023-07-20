using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Repository;

public interface IReviewRepository
{
    Task<List<Review>> GetAllReviews();
    Task<Review> GetReviewById(Guid reviewId);
    Task<Review> CreateReview(Guid filmId, int rating);
    Task<Review> UpdateReview(Guid reviewId, int rating);
    Task DeleteReview(Guid reviewId);
}