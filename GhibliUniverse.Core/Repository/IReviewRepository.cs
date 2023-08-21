using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Repository;

public interface IReviewRepository
{
    Task<List<Review>> GetAllReviews();
    Task<Review> GetReviewById(Guid reviewId);
    Task<Review> CreateReview(Guid filmId, Review reviewCreateRequest);
    Task<Review> UpdateReview(Guid reviewId, Review reviewUpdateRequest);
    Task DeleteReview(Guid reviewId);
}