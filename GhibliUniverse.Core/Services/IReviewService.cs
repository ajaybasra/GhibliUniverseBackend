using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Services;

public interface IReviewService
{
    Task<List<Review>> GetAllReviews();
    Task<Review> GetReviewById(Guid reviewId);
    Task<Review> CreateReview(Guid filmId, Review reviewCreateRequest);
    Task<Review> UpdateReview(Guid reviewId, Review reviewUpdateRequest);
    Task DeleteReview(Guid reviewId);
}