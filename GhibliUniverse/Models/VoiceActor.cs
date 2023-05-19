using System.Text;

namespace GhibliUniverse;

public record VoiceActor()
{
    public Guid Id { get; init; }
    public string Name { get; set; }

    public List<Film> Films { get; }  = new();

    public void AddFilm(Film film)
    {
        if (!Films.Contains(film))
        {
            Films.Add(film);
        }
    }

    public void RemoveFilm(Film film)
    {
        if (Films.Contains(film))
        {
            Films.Remove(film);
        }
    }
    
    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append(Name);
        return str.ToString();
    }
}