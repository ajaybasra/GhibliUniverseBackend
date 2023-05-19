using System.Text;
using System.Text.Unicode;
using GhibliUniverse.Interfaces;
using GhibliUniverse.ValueObjects;

namespace GhibliUniverse.DataPersistence;

public class FilmPersistence : IPersistence
{
    private readonly FilmUniverse _filmUniverse;
    private readonly FileOperations _fileOperations;
    private const string OldFilmsFilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/old-films.csv";
    private const string FilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/films.csv";

    public FilmPersistence(FilmUniverse filmUniverse, FileOperations fileOperations)
    {
        _filmUniverse = filmUniverse;
        _fileOperations = fileOperations;
    }

    public void ReadingStep()
    {
        if (_fileOperations.FileExists(FilePath))
        {
            ReadInRecords();
            _fileOperations.CreateBackupCSVFile(FilePath, OldFilmsFilePath);
        }
    }

    public void WritingStep()
    {
        var allFilms = _filmUniverse.GetAllFilms();
        CreateFileHeader();
        if (allFilms.Count <= 0) return;
        foreach (var film in allFilms)
        {
            AddFilmRecordFromFilmUniverseToCSV(film.Id, film.Title, film.Description, film.Director, film.Composer, film.ReleaseYear);
        }
    }
    
    private void CreateFileHeader()
    {
        using var file = new StreamWriter(FilePath);
        file.WriteLine("Id" + "," + "Title" + "," + "Description" + "," + "Director" + "," + "Composer" + "," + "Release Year");
    }
    
    private void AddFilmRecordFromFilmUniverseToCSV(Guid id, string title, string description, string director, string composer, ReleaseYear releaseYear)
    {
        try
        {
            using var file = new StreamWriter(FilePath, true);
            file.WriteLine(id + ","  + title + "," + description.Replace(',', '*') + "," + director + "," + composer + "," + releaseYear);
            file.Close();
        }  
        catch(Exception ex)  
        {  
            Console.Write(ex.Message);  
        } 
    }
    private void ReadInRecords()
    {
        var lines = File.ReadLines(FilePath).ToList();
        if (lines.Count <= 1)
        {
            return;
        }
        lines.Skip(1)
            .Select(line => line.Split(','))
            .ToList()
            .ForEach(properties => AddFilmFromCSVToFilmList(new Guid(properties[0]),properties[1], properties[2].Replace('*', ','), properties[3], properties[4], ReleaseYear.From(int.Parse(properties[5]))));
        
    }
    private void AddFilmFromCSVToFilmList(Guid id, string title, string description, string director, string composer, ReleaseYear releaseYear)
    {
        var film = new Film()
        {
            Id = id,
            Title = title,
            Description = description,
            Director = director,
            Composer = composer,
            ReleaseYear = releaseYear
        };
        
        _filmUniverse.AddFilm(film);
    }

}
