namespace GhibliUniverse;

public record Film()
{
    public int FilmId { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Director { get; set; }
    public string Composer { get; set; }
    public int ReleaseYear { get; set; }
    public int Score { get; set; }
}