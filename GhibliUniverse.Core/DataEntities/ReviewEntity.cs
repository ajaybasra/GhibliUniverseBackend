namespace GhibliUniverse.Core.DataEntities;

public record ReviewEntity()
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    
    public Guid FilmId { get; set; }
    public FilmEntity Film { get; set; } = null!;
}