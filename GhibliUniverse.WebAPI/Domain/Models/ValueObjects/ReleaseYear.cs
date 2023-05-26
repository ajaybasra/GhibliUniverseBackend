using ValueOf;

namespace GhibliUniverse.WebAPI.Domain.Models.ValueObjects;

public class ReleaseYear : ValueOf<int, ReleaseYear>
{
    private const int OldestReleaseYear = 1984;
    
    protected override void Validate()
    {
        if (Value.ToString().Length != 4)
        {
            throw new NotFourCharactersException(Value);
        }
        
        if (Value < 1984)
        {
            throw new ReleaseYearLessThanOldestReleaseYearException(Value);
        }
    }

    public class ReleaseYearLessThanOldestReleaseYearException : Exception
    {
        public ReleaseYearLessThanOldestReleaseYearException(int releaseYear) :
            base($"Release year cannot be earlier than {OldestReleaseYear}. Current release year: {releaseYear}")
        {
            
        }
    }

    public class NotFourCharactersException : Exception
    {
        public NotFourCharactersException(int releaseYear) : base(
            $"Release year must be four characters to be valid. Current release year: {releaseYear}")
        {
            
        }

    }
}