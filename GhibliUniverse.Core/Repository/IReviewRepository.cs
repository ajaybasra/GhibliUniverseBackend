using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.Repository;

public interface IReviewRepository
{
    List<Review> GetAllReviews();
    Review GetReviewById(Guid reviewId);
    Review CreateReview(Guid filmId, int rating);
    Review UpdateReview(Guid reviewId, int rating);
    void DeleteReview(Guid reviewId);
}