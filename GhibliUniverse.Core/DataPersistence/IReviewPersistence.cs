using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.Core.DataPersistence;

public interface IReviewPersistence
{
    List<Review> ReadReviews();
    void WriteReviews(List<Review> reviews);
}