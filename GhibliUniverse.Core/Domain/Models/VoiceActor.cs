using System.Text;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Domain.Models;

public record VoiceActor()
{
    public Guid Id { get; init; }
    public ValidatedString Name { get; set; }

    public List<Film> Films { get; }  = new();

    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append(Name);
        return str.ToString();
    }
}