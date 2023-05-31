using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;
using Moq;

namespace GhibliUniverse.Core.Tests;

public class ReviewServiceTests
{
    private readonly ReviewService _reviewService;
    private readonly Mock<IReviewPersistence> _mockedReviewPersistence;
    private readonly List<Review> _reviews = new();

    public ReviewServiceTests()
    {
        PopulateReviewsList(2);
        _mockedReviewPersistence = new Mock<IReviewPersistence>();
        _mockedReviewPersistence.Setup(x => x.ReadReviews()).Returns(_reviews);
        _reviewService = new ReviewService(_mockedReviewPersistence.Object);
    }
    
    private void PopulateReviewsList(int numberOfVoiceActors)
    {
        for (var i = 0; i < numberOfVoiceActors; i++)
        {
            _reviews.Add(new Review
            {
                Id = new Guid($"{i}{i}{i}{i}{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}-{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}"),
                Rating = Rating.From(10)
            });
        }
    }

    [Fact]
    public void GetAllReviews_ReturnsAllReviews_WhenCalled()
    {
        var reviewCount = _reviewService.GetAllReviews().Count;
        
        Assert.Equal(2, reviewCount);
    }
    
    [Fact]
    public void GetReviewById_ReturnsReviewWithMatchingId_WhenGivenReviewId()
    {
        var expectedId = new Guid("00000000-0000-0000-0000-000000000000");

        var actualFilm = _reviewService.GetReviewById(new Guid("00000000-0000-0000-0000-000000000000"));

        Assert.Equal(expectedId, actualFilm.Id);
    }
    
    [Fact]
    public void CreateFilm_PersistsNewlyCreatedReview_WhenCalled()
    {
        _reviewService.CreateReview(Guid.Empty, 10);
        var reviewId = _reviews[2].Id;
         
        var reviewCount = _reviews.Count;
        var review = _reviewService.GetReviewById(reviewId);
         
        Assert.Equal(3, reviewCount);
        Assert.Equal(reviewId, review.Id);
    }
    
    [Fact]
    public void UpdateReview_UpdatesReviewRating_WhenCalled()
    {
        var reviewId = _reviews[0].Id;
         
        _reviewService.UpdateReview(reviewId, 9);
        var reviewWithUpdatedName = _reviews[0];

        Assert.Equal(Rating.From(9), reviewWithUpdatedName.Rating);
    }

    [Fact]
    public void DeleteReview_RemovesReviewWithMatchingId_WhenGivenReviewId()
    {
        var reviewId = _reviews[0].Id;
        
        _reviewService.DeleteReview(reviewId);
        var reviewCount = _reviews.Count;
        
        Assert.Equal(1, reviewCount);
    }
}
