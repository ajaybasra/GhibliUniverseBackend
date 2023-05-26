using System.Text;
using GhibliUniverse.WebAPI.Domain.Models.ValueObjects;

namespace GhibliUniverse.WebAPI.Domain.Models;

public record FilmRating()
{
    public Guid Id { get; init; }
    public Rating Rating { get; set; }
    
    public Guid FilmId { get; set; }
    public Film Film { get; set; } = null!;
    
    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append(Rating);
        return str.ToString();
    }
}