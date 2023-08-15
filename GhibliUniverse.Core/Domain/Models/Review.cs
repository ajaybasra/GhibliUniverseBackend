using System.Text;
using GhibliUniverse.Core.DataEntities;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Domain.Models;

public record Review(ReviewEntity ReviewEntity)
{
    public Guid Id => ReviewEntity.Id;
    public Rating Rating => Rating.From(ReviewEntity.Rating);
    
    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append(Rating);
        return str.ToString();
    }
}