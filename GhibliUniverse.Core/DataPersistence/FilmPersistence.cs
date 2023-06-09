using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Core.DataPersistence;

public class FilmPersistence : IFilmPersistence
{
    private readonly IFileOperations _fileOperations;
    static string currentPath = Directory.GetCurrentDirectory();
    private string OldFilmsFilePath = Path.Combine(currentPath, "CSVData/old-films.csv");
    private string FilePath = Path.Combine(currentPath, "CSVData/films.csv");

    public FilmPersistence(IFileOperations fileOperations)
    {
        _fileOperations = fileOperations;
    }
    
    public List<Film> ReadFilms()
    {
        if (!_fileOperations.FileExists(FilePath))
        {
            return new List<Film>();
        }
        var savedFilms = new List<Film>();
        using var reader = new StreamReader(FilePath);
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
        using var file = new StreamWriter(FilePath);
        file.WriteLine("Id" + "," + "Title" + "," + "Description" + "," + "Director" + "," + "Composer" + "," + "Release Year");
    }
    public void WriteFilms(List<Film> films)
    {
        _fileOperations.CreateBackupCSVFile(FilePath, OldFilmsFilePath);
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
            using var file = new StreamWriter(FilePath, true);
            file.WriteLine(id + ","  + title + "," + description.ToString().Replace(',', '*') + "," + director + "," + composer + "," + releaseYear);
            file.Close();
        }  
        catch(Exception ex)  
        {
            Console.Write(ex.Message);
        } 
    }

}
