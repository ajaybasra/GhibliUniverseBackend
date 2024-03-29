using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Repository;
using GhibliUniverse.Core.Services;
using Moq;

namespace GhibliUniverse.Core.Tests.ServiceTests;

public class ReviewServiceTests
{
    private readonly ReviewService _reviewService;
    private readonly Mock<IReviewRepository> _mockedReviewRepository;
    private readonly List<Review> _reviews = new();

    public ReviewServiceTests()
    {
        PopulateReviewsList(2);
        _mockedReviewRepository = new Mock<IReviewRepository>();
        _reviewService = new ReviewService(_mockedReviewRepository.Object);
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
    public async Task GetAllReviews_ReturnsAllReviews_WhenCalled()
    {
        _mockedReviewRepository.Setup(x => x.GetAllReviews()).ReturnsAsync(_reviews);
        
        var reviews = await _reviewService.GetAllReviews();
        
        Assert.Equal(2, reviews.Count);
    }
    
    [Fact]
    public async Task GetReviewById_ReturnsReviewWithMatchingId_WhenGivenReviewId()
    {
        var expectedId = new Guid("00000000-0000-0000-0000-000000000000");
        _mockedReviewRepository.Setup(x => x.GetReviewById(Guid.Empty)).ReturnsAsync(_reviews[0]);


        var actualFilm = await _reviewService.GetReviewById(new Guid("00000000-0000-0000-0000-000000000000"));

        Assert.Equal(expectedId, actualFilm.Id);
    }
    
    [Fact]
    public async Task GetReviewById_ThrowsModelNotFoundException_WhenGivenIdOfReviewWhichDoesNotExist()
    {
        _mockedReviewRepository.Setup(x => x.GetReviewById(Guid.Parse("00000000-0000-0000-0000-000000000005"))).Throws(new ModelNotFoundException(Guid.Parse("00000000-0000-0000-0000-000000000005")));

        await Assert.ThrowsAsync<ModelNotFoundException>(() => _reviewService.GetReviewById(Guid.Parse("00000000-0000-0000-0000-000000000005")));
    }
    
    [Fact]
    public async Task CreateReview_PersistsNewlyCreatedReview_WhenCalled()
    {
        var newReviewId = Guid.NewGuid();  
        var newRating = Rating.From(10);

        _mockedReviewRepository
            .Setup(x => x.CreateReview(It.IsAny<Guid>(), It.IsAny<Review>()))
            .Callback((Guid id, Review review) =>
            {
                review.Id = newReviewId;
                _reviews.Add(review);
            });

        await _reviewService.CreateReview(newReviewId, new Review() { Rating = newRating });
        _mockedReviewRepository.Setup(x => x.GetReviewById(_reviews[2].Id)).ReturnsAsync(_reviews[2]);

        var reviewId = _reviews[2].Id;
        var reviewCount = _reviews.Count;
        var review = _reviews[2];

        Assert.Equal(3, reviewCount);
        Assert.Equal(reviewId, review.Id);
    }



    [Fact]
    public async Task UpdateReview_UpdatesReviewRating_WhenCalled()
    {
        var reviewId = _reviews[0].Id;
        _mockedReviewRepository.Setup(x => x.UpdateReview(reviewId, new Review() {Rating = Rating.From(10)})).Callback((Guid id, Review updatedReview) =>
        {
            var reviewToUpdate = _reviews.FirstOrDefault(va => va.Id == id);
            if (reviewToUpdate != null)
            {
                reviewToUpdate.Rating = updatedReview.Rating;
            }

        });
        await _reviewService.UpdateReview(reviewId, new Review() {Rating = Rating.From(10)});
        var reviewWithUpdatedName = _reviews[0];

        Assert.Equal(Rating.From(10), reviewWithUpdatedName.Rating);
    }

    [Fact]
    public async Task DeleteReview_RemovesReviewWithMatchingId_WhenGivenReviewId()
    {
        var reviewId = _reviews[0].Id;
        _mockedReviewRepository.Setup(x => x.DeleteReview(reviewId)).Callback(() => _reviews.Remove(_reviews[0]));
        
        await _reviewService.DeleteReview(reviewId);
        var reviewCount = _reviews.Count;
        
        Assert.Equal(1, reviewCount);
    }
}
