using GhibliUniverse.API.DTOs;
using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.API.Mapper;

public static class CustomMapper
{
    public static FilmWithReviewInfo MapToFilmWithReviewInfo(Film film)
    {
        var filmWithReviewInfo = new FilmWithReviewInfo
        {
            Id = film.Id,
            Title = film.Title,
            Description = film.Description,
            Director = film.Director,
            Composer = film.Composer,
            ReleaseYear = film.ReleaseYear,
            FilmReviewInfo =
            {
                AverageRating = CalculateAverageRating(film.Reviews),
                NumberOfRatings = film.Reviews.Count
            }
        };

        return filmWithReviewInfo;
    }

    private static double CalculateAverageRating(List<Review> reviews)
    {
        if (reviews.Count == 0)
        {
            return 0; 
        }

        double totalRating = reviews.Sum(review => review.Rating.Value);
        return totalRating / reviews.Count;
    }
}