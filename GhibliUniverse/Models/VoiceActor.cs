using System.Text;

namespace GhibliUniverse;

public record VoiceActor()
{
    public Guid VoiceActorId { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public Guid FilmId { get; set; }
    
    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append(FirstName + " " + LastName);
        return str.ToString();
    }
}