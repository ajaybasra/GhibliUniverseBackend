using System.Text;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Services;

public class ReviewService : IReviewService
{
    private readonly List<Film> _filmList;
    private readonly IFilmService _filmService;

    public ReviewService(IFilmService filmService)
    {
        _filmService = filmService;
        _filmList = filmService.GetAllFilms();
    }
    public List<Review> GetAllReviews()
    {
        var allReviews = _filmList.SelectMany(film => film.Reviews).ToList();
        return allReviews;
    }

    public Review GetReviewById(Guid reviewId)
    {
        var foundReview = _filmList
            .SelectMany(film => film.Reviews)
            .FirstOrDefault(review => review.Id == reviewId);
        
        if (foundReview == null)
        {
            throw new ModelNotFoundException(reviewId);
        }

        return foundReview;
    }

    public void CreateReview(Guid filmId, int rating)
    {
        try
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                Rating = Rating.From(rating),
                FilmId = filmId
            };
            var film = _filmService.GetFilmById(filmId);
            film.Reviews.Add(review);
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
        var reviewToUpdate = GetReviewById(reviewId);
        reviewToUpdate.Rating = Rating.From(rating);
    }

    public void DeleteReview(Guid reviewId)
    {
        try
        {
            var reviewToDelete = GetReviewById(reviewId);
            var filmId = reviewToDelete.FilmId;
            var filmToRemoveReviewFrom = _filmService.GetFilmById(filmId);
            filmToRemoveReviewFrom.Reviews.Remove(reviewToDelete);
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