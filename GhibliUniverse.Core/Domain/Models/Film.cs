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

}