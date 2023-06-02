namespace GhibliUniverse.API.DTOs;

public record FilmPostDTO()
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Director { get; set; }
    public string Composer { get; set; }
    public int ReleaseYear { get; set; } 
}