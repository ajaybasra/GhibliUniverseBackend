using System.Text;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Repository;

namespace GhibliUniverse.Core.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }
    public List<Review> GetAllReviews()
    {
        return _reviewRepository.GetAllReviews();
    }

    public Review GetReviewById(Guid reviewId)
    {
        return _reviewRepository.GetReviewById(reviewId);
    }

    public Review CreateReview(Guid filmId, int rating)
    {
        return _reviewRepository.CreateReview(filmId, rating);
    }

    public Review UpdateReview(Guid reviewId, int rating)
    {
        return _reviewRepository.UpdateReview(reviewId, rating);
    }
    public void DeleteReview(Guid reviewId)
    {
        _reviewRepository.DeleteReview(reviewId);
    }
    
    public string BuildReviewList()
    {
        var stringBuilder = new StringBuilder();
        var allReviews = GetAllReviews();
        foreach (var review in allReviews)
        {
            stringBuilder.Append(review);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }
}