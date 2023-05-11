namespace GhibliUniverse.DataPersistence;

public class MoviePersistence
{
    private readonly FilmUniverse _filmUniverse;
    private const string filePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/movies.csv";

    public MoviePersistence(FilmUniverse filmUniverse)
    {
        _filmUniverse = filmUniverse;
    }

    public void ReadingStep()
    {
        ReadInFilmRecords();
        ClearFile();
    }

    public void WritingStep()
    {
        var allFilms = _filmUniverse.GetAllFilms();
        CreateFileHeader();
        if (allFilms.Count <= 0) return;
        foreach (var film in allFilms)
        {
            AddFilmRecord(film.Id, film.Title, film.Description, film.Director, film.Composer, film.ReleaseYear);
        }
    }
    private void CreateFileHeader()
    {
        using var file = new StreamWriter(filePath);
        file.WriteLine("Id" + ";" + "Title" + ";" + "Description" + ";" + "Director" + ";" + "Composer" + ";" + "Release Year");
    }
    
    private void AddFilmRecord(Guid id, string Title, string Description, string Director, string Composer, int ReleaseYear)
    {
        try
        {
            using var file = new StreamWriter(@filePath, true);
            file.WriteLine(id + ";"  + Title + ";" + EncodeCell(Description) + ";" + Director + ";" + Composer + ";" + ReleaseYear);
            file.Close();
        }  
        catch(Exception ex)  
        {  
            Console.Write(ex.Message);  
        } 
    }

    private void ReadInFilmRecords()
    {
        var lines = File.ReadLines(filePath).ToList();
        if (lines.Count <= 1)
        {
            return;
        }
        foreach (var line in lines.Skip(1))
        {
            var properties = line.Split(';');
            _filmUniverse.CreateFilm(properties[1], properties[2], properties[3], properties[4], int.Parse(properties[5]));
        }
        
    }

    void ClearFile()
    {
        File.WriteAllText(@filePath,string.Empty);
    }

    string EncodeCell(string value)
    {
        return value.Contains(",") ? $"\"{value}\"" : value;
    }
}