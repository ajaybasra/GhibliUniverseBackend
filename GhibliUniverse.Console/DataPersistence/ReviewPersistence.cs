using GhibliUniverse.Console.Interfaces;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Console.DataPersistence;

public class ReviewPersistence : IPersistence
{
    private readonly IFilmService _filmService;
    private readonly IReviewService _reviewService;
    private readonly FileOperations _fileOperations;
    private const string OldFilmRatingsFilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/old-reviews.csv";
    private const string FilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/reviews.csv";

    public ReviewPersistence(IFilmService filmService, IReviewService reviewService, FileOperations fileOperations)
    {
        _filmService = filmService;
        _reviewService = reviewService;
        _fileOperations = fileOperations;
    }
    
    public void ReadingStep()
    {
        if (_fileOperations.FileExists(FilePath))
        {
            ReadInRecords();
            _fileOperations.CreateBackupCSVFile(FilePath, OldFilmRatingsFilePath);
        }
    }

    public void WritingStep()
    {
        var allReviews = new List<Review>();
        allReviews.AddRange(_reviewService.GetAllReviews());

        CreateFileHeader();
        if (allReviews.Count <= 0) return;
        foreach (var review in allReviews)
        {
            AddFilmRatingRecordFromFilmUniverseToCSV(review.Id, review.Rating, review.FilmId);
        }
    }
    
    private void CreateFileHeader()
    {
        using var file = new StreamWriter(FilePath);
        file.WriteLine("Id" + "," + "Rating" + "," + "Film Id");
    }
    
    private void AddFilmRatingRecordFromFilmUniverseToCSV(Guid id, Rating rating, Guid filmId)
    {
        try
        {
            using var file = new StreamWriter(FilePath, true);
            file.WriteLine(id + ","  + rating + "," + filmId);
            file.Close();
        }  
        catch(Exception ex)  
        {  
            System.Console.Write(ex.Message);  
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
        var filmRating = new Review()
        {
            Id = id,
            Rating = Rating.From(ratingAsInt),
            FilmId = filmId
        };

        var filmList = _filmService.GetAllFilms();
        filmList.First(film => film.Id == filmId).Reviews.Add(filmRating);
        
    }

}