using GhibliUniverse.Interfaces;

namespace GhibliUniverse.DataPersistence;

public class FilmRatingPersistence : IPersistence
{
    private readonly FilmUniverse _filmUniverse;
    private const string OldFilmRatingsFilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/old-film-ratings.csv";
    private const string FilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/film-ratings.csv";

    public FilmRatingPersistence(FilmUniverse filmUniverse)
    {
        _filmUniverse = filmUniverse;
    }
    
    public void ReadingStep()
    {
        ReadInRecords();
        CreateBackupCSVFile();
    }

    public void WritingStep()
    {
        var allFilmRatings = new List<FilmRating>();
        var allFilms = _filmUniverse.GetAllFilms();
        foreach (var film in allFilms)
        {
            allFilmRatings.AddRange(_filmUniverse.GetAllFilmRatings(film.Id));
        }

        CreateFileHeader();
        if (allFilmRatings.Count <= 0) return;
        foreach (var filmRating in allFilmRatings)
        {
            AddFilmRatingRecordFromFilmUniverseToCSV(filmRating.Id, filmRating.Rating, filmRating.FilmId);
        }
    }
    
    private void CreateFileHeader()
    {
        using var file = new StreamWriter(FilePath);
        file.WriteLine("Id" + "," + "Rating" + "," + "Film Id");
    }
    
    private void AddFilmRatingRecordFromFilmUniverseToCSV(Guid id, int rating, Guid filmId)
    {
        try
        {
            using var file = new StreamWriter(FilePath, true);
            file.WriteLine(id + ","  + rating + "," + filmId);
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
            .ForEach(properties => AddFilmRatingsFromCSVToMatchingFilms(new Guid(properties[0]),properties[1], new Guid(properties[2])));
    }
    
    private void AddFilmRatingsFromCSVToMatchingFilms(Guid id, string rating, Guid filmId)
    {
        var ratingAsInt = int.Parse(rating);
        var filmRating = new FilmRating()
        {
            Id = id,
            Rating = ratingAsInt,
            FilmId = filmId
        };

        var filmList = _filmUniverse.GetAllFilms();
        filmList.First(film => film.Id == filmId).FilmRatings.Add(filmRating);
        
    }

    private void CreateBackupCSVFile()
    {
        var lines = File.ReadAllLines(FilePath);
        File.WriteAllLines(OldFilmRatingsFilePath, lines);
    }

    public bool FileExists()
    {
        return File.Exists(FilePath);
    }
}