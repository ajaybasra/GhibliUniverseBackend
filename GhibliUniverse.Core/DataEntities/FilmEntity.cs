namespace GhibliUniverse.Core.DataEntities
{
    public class FilmEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string Composer { get; set; }
        public int ReleaseYear { get; set; }

        public List<VoiceActorEntity> VoiceActors { get; set; } = new();
        public List<ReviewEntity> Reviews { get; set; } = new();
    }
}