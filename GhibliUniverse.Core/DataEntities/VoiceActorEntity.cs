namespace GhibliUniverse.Core.DataEntities
{
    public class VoiceActorEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<FilmEntity> Films { get; set; } = new();
    }
}