using System.Text;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Domain.Models
{
    public class VoiceActor
    {
        public Guid Id { get; set; }
        public ValidatedString Name { get; set; }

        public List<VoiceActorFilm> Films { get; set; } = new();

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Name);
            return str.ToString();
        }
    }
}