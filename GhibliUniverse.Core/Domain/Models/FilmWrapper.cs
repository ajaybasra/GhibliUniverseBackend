using System.Text;

namespace GhibliUniverse.Core.Domain.Models;

public record FilmWrapper(Film Film)
{
    public Guid Id => Film.Id;
    
    public FilmInfo FilmInfo { get; } = new()
    {
        Title = Film.Title,
        Description = Film.Description,
        Director = Film.Director,
        Composer = Film.Composer,
        ReleaseYear = Film.ReleaseYear
    };

    public FilmReviewInfo FilmReviewInfo { get; init; } = new();
    
    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append('[');
        str.Append($"Title:{FilmInfo.Title},");
        str.Append($"Description:{FilmInfo.Description},");
        str.Append($"Director:{FilmInfo.Director},");
        str.Append($"Composer:{FilmInfo.Composer},");
        str.Append($"Release Year:{FilmInfo.ReleaseYear},");
        str.Append("Voice Actors:");
        str.Append('[');
        str.Append(string.Join(",", Film.VoiceActors));
        str.Append(']');
        str.Append(",Film Ratings:");
        str.Append('[');
        str.Append(string.Join(",", Film.Reviews));
        str.Append(']');
        str.Append(']');
        return str.ToString();
    }
}