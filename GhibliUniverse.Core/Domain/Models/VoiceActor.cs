using System.Text;
using GhibliUniverse.Core.DataEntities;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Domain.Models;

public record VoiceActor(VoiceActorEntity VoiceActorEntity)
{
    public Guid Id => VoiceActorEntity.Id;
    public ValidatedString Name => ValidatedString.From(VoiceActorEntity.Name);

    public List<Film> Films { get; set; }  = VoiceActorEntity.Films.Select(f => new Film(f)).ToList();

    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append(Name);
        return str.ToString();
    }
}