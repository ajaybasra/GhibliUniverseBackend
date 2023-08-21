using System.Text;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Repository;

namespace GhibliUniverse.Core.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }
    public async Task<List<Review>> GetAllReviews()
    {
        return await _reviewRepository.GetAllReviews();
    }

    public async Task<Review> GetReviewById(Guid reviewId)
    {
        return await _reviewRepository.GetReviewById(reviewId);
    }

    public async Task<Review> CreateReview(Guid filmId, Review reviewCreateRequest)
    {
        return await _reviewRepository.CreateReview(filmId, reviewCreateRequest);
    }

    public async Task<Review> UpdateReview(Guid reviewId, Review reviewUpdateRequest)
    {
        return await _reviewRepository.UpdateReview(reviewId, reviewUpdateRequest);
    }
    public async Task DeleteReview(Guid reviewId)
    {
        await _reviewRepository.DeleteReview(reviewId);
    }
    
    public async Task<string> BuildReviewList()
    {
        var stringBuilder = new StringBuilder();
        var allReviews = await GetAllReviews();
        foreach (var review in allReviews)
        {
            stringBuilder.Append(review);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }
}