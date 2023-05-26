using ValueOf;

namespace GhibliUniverse.Core.Domain.Models.ValueObjects;

public class Rating : ValueOf<int, Rating>
{
    private const int MinimumRating = 1;
    private const int MaximumRating = 10;

    protected override void Validate()
    {
        if (Value < 1 || Value > 10)
        {
            throw new RatingOutOfRangeException(Value);
        }
    }

    public class RatingOutOfRangeException : Exception
    {
        public RatingOutOfRangeException(int rating) :
            base($"Rating must be between {MinimumRating} and {MaximumRating} inclusive. Current rating: {rating}.")
        {
            
        }
    }
}

