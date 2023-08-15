using System.Text;
using GhibliUniverse.Core.DataEntities;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Domain.Models;

public record Film(FilmEntity FilmEntity)
{
    public Guid Id => FilmEntity.Id;
    
    public FilmInfo FilmInfo { get; } = new()
    {
        Title = ValidatedString.From(FilmEntity.Title),
        Description = ValidatedString.From(FilmEntity.Description),
        Director = ValidatedString.From(FilmEntity.Director),
        Composer = ValidatedString.From(FilmEntity.Composer),
        ReleaseYear = ReleaseYear.From(FilmEntity.ReleaseYear),
        VoiceActors = FilmEntity.VoiceActors.Select(v => new VoiceActor(v)).ToList(),
        Reviews = FilmEntity.Reviews.Select(r => new Review(r)).ToList()
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
        str.Append(string.Join(",", FilmEntity.VoiceActors));
        str.Append(']');
        str.Append(",Film Ratings:");
        str.Append('[');
        str.Append(string.Join(",", FilmEntity.Reviews));
        str.Append(']');
        str.Append(']');
        return str.ToString();
    }
}