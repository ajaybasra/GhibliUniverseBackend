using System.Text;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Domain.Models;

public record Film()
{
    public Guid Id { get; set; }
    public ValidatedString Title { get; set; }
    public ValidatedString Description { get; set; }
    public ValidatedString Director { get; set; }
    public ValidatedString Composer { get; set; }
    public ReleaseYear ReleaseYear { get; set; } 
    
    public List<VoiceActor> VoiceActors { get; set; } = new(); 
    public List<Review> Reviews { get; set; } = new();
    
    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append('[');
        str.Append($"Title:{Title},");
        str.Append($"Description:{Description},");
        str.Append($"Director:{Director},");
        str.Append($"Composer:{Composer},");
        str.Append($"Release Year:{ReleaseYear},");
        str.Append("Voice Actors:");
        str.Append('[');
        str.Append(string.Join(",", VoiceActors));
        str.Append(']');
        str.Append(",Film Ratings:");
        str.Append('[');
        str.Append(string.Join(",", Reviews));
        str.Append(']');
        str.Append(']');
        return str.ToString();
    }

}