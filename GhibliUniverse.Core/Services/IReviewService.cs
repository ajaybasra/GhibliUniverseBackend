using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Services;

public interface IReviewService
{
    List<Review> GetAllReviews();
    Review GetReviewById(Guid reviewId);
    void CreateReview(Guid filmId, int rating);
    void UpdateReview(Guid reviewId, int rating);
    void DeleteReview(Guid reviewId);
}