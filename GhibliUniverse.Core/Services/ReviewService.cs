using System.Text;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewPersistence _reviewPersistence;

    public ReviewService(IReviewPersistence reviewPersistence)
    {
        _reviewPersistence = reviewPersistence;
    }
    public List<Review> GetAllReviews()
    {
        return _reviewPersistence.ReadReviews();
    }

    public Review GetReviewById(Guid reviewId)
    {
        var savedReviews = _reviewPersistence.ReadReviews();
        var foundReview = savedReviews.FirstOrDefault(r => r.Id == reviewId);

        if (foundReview == null)
        {
            throw new ModelNotFoundException(reviewId);
        }

        return foundReview;
    }

    public void CreateReview(Guid filmId, int rating)
    {
        var savedReviews = _reviewPersistence.ReadReviews();
        try
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                Rating = Rating.From(rating),
                FilmId = filmId
            };
            savedReviews.Add(review);
            _reviewPersistence.WriteReviews(savedReviews);
        }
        catch (ModelNotFoundException e)
        {
            Console.WriteLine(e);
        }
        catch (Rating.RatingOutOfRangeException e)
        {
            Console.WriteLine(e);
        }
    }

    public void UpdateReview(Guid reviewId, int rating)
    {
        var savedReviews = _reviewPersistence.ReadReviews();
        var reviewToUpdate = GetReviewById(reviewId);
        reviewToUpdate.Rating = Rating.From(rating);
        savedReviews = savedReviews.Select(r => r.Id == reviewId ? reviewToUpdate : r).ToList();
        _reviewPersistence.WriteReviews(savedReviews);
    }
    public void DeleteReview(Guid reviewId)
    {
        var savedReviews = _reviewPersistence.ReadReviews();
        try
        {
            var reviewToDelete = GetReviewById(reviewId);
            // var filmId = reviewToDelete.FilmId;
            // var filmToRemoveReviewFrom = _filmService.GetFilmById(filmId);
            // filmToRemoveReviewFrom.Reviews.Remove(reviewToDelete);
            savedReviews.Remove(reviewToDelete);
            _reviewPersistence.WriteReviews(savedReviews);
        }
        catch (ModelNotFoundException e)
        {
            Console.WriteLine(e);
        }
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