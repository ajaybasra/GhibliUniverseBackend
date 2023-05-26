using System.Text;
using GhibliUniverse.WebAPI.Domain.Models.ValueObjects;

namespace GhibliUniverse.WebAPI.Domain.Models;

public record Film()
{
    public Guid Id { get; init; }
    public ValidatedString Title { get; set; }
    public ValidatedString Description { get; set; }
    public ValidatedString Director { get; set; }
    public ValidatedString Composer { get; set; }
    public ReleaseYear ReleaseYear { get; set; } 
    
    public List<VoiceActor> VoiceActors { get; } = new(); 
    public List<FilmRating> FilmRatings { get; set; } = new();

    public void AddVoiceActor(VoiceActor voiceActor)
    {
        if (!VoiceActors.Contains(voiceActor))
        {
            VoiceActors.Add(voiceActor);
            voiceActor.AddFilm(this);
        }
    }

    public void RemoveVoiceActor(VoiceActor voiceActor)
    {
        if (VoiceActors.Contains(voiceActor))
        {
            VoiceActors.Remove(voiceActor);
            voiceActor.RemoveFilm(this);
        }
    }
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
        str.Append(']');
        return str.ToString();
    }

}