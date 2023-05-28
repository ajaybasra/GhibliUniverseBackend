using System.Text;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Domain.Models;

public record Review()
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