namespace GhibliUniverse.Core.Domain.Models;

public record FilmReviewInfo()
{
    public Guid FilmId { get; set; }
    public double AverageRating { get; set; }
    public int NumberOfRatings { get; set; }
};