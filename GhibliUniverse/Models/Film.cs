using System.Text;

namespace GhibliUniverse;

public record Film()
{
    public Guid FilmId { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Director { get; set; }
    public string Composer { get; set; }
    public int ReleaseYear { get; set; }
    public List<VoiceActor> VoiceActors { get; set; }
    public List<FilmRating> FilmRatings { get; set; }
    
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
        str.Append(string.Join(",", FilmRatings));
        str.Append(']');
        return str.ToString();
    }

}