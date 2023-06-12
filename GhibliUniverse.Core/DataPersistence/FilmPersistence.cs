using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.DataPersistence;

public class FilmPersistence : IFilmPersistence
{
    private readonly IFileOperations _fileOperations;
    private static readonly string BaseDirectory = AppContext.BaseDirectory;
    // private static readonly string RootDirectory = Directory.GetParent(BaseDirectory).Parent.Parent.Parent.Parent.FullName;
    // private readonly string _oldFilmsFilePath = Path.Combine(RootDirectory, "GhibliUniverse.Core/DataPersistence/CSVData/old-films.csv");
    // private readonly string _filePath = Path.Combine(RootDirectory, "GhibliUniverse.Core/DataPersistence/CSVData/films.csv");
    private readonly string _oldFilmsFilePath =
        "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/GhibliUniverse.Core/DataPersistence/CSVData/old-films.csv";
    private readonly string _filePath =
        "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/GhibliUniverse.Core/DataPersistence/CSVData/films.csv";
    
    public FilmPersistence(IFileOperations fileOperations)
    {
        _fileOperations = fileOperations;
    }
    
    public List<Film> ReadFilms()
    {
        if (!_fileOperations.FileExists(_filePath))
        {
            return new List<Film>();
        }
        var savedFilms = new List<Film>();
        using var reader = new StreamReader(_filePath);
        var headerLine = reader.ReadLine();
        string currentLine;
        while ((currentLine = reader.ReadLine()) != null)
        {
            string[] fields = currentLine.Split(',');

            savedFilms.Add(new Film
            {
                Id = Guid.Parse(fields[0]),
                Title = ValidatedString.From(fields[1]),
                Description = ValidatedString.From(fields[2].Replace('*', ',')),
                Director = ValidatedString.From(fields[3]),
                Composer = ValidatedString.From(fields[4]),
                ReleaseYear = ReleaseYear.From(int.Parse(fields[5]))
            });
        }
        return savedFilms;
    }
    
    private void CreateFileHeader()
    {
        using var file = new StreamWriter(_filePath);
        file.WriteLine("Id" + "," + "Title" + "," + "Description" + "," + "Director" + "," + "Composer" + "," + "Release Year");
    }
    public void WriteFilms(List<Film> films)
    {
        _fileOperations.CreateBackupCSVFile(_filePath, _oldFilmsFilePath);
        CreateFileHeader();
        if (films.Count <= 0) return;
        foreach (var film in films)
        {
            WriteFilm(film.Id, film.Title, film.Description, film.Director, film.Composer, film.ReleaseYear);
        }
    }

    private void WriteFilm(Guid id, ValidatedString title, ValidatedString description, ValidatedString director, ValidatedString composer, ReleaseYear releaseYear)
    {
        try
        {
            using var file = new StreamWriter(_filePath, true);
            file.WriteLine(id + ","  + title + "," + description.ToString().Replace(',', '*') + "," + director + "," + composer + "," + releaseYear);
            file.Close();
        }  
        catch(Exception ex)  
        {
            Console.Write(ex.Message);
        } 
    }

}
