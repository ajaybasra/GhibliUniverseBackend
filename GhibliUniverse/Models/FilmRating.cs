using System.Text;

namespace GhibliUniverse;

public record FilmRating()
{
    public Guid FilmRatingId { get; init; }
    public int Rating { get; set; }
    
    public Guid FilmId { get; set; }
    
    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append(Rating);
        return str.ToString();
    }
}