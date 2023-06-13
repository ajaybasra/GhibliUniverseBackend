using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Core.DataPersistence;

public class ReviewPersistence : IReviewPersistence
{
    private readonly IFileOperations _fileOperations;
    private static readonly string WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
    private readonly string _oldFilmRatingsFilePath =  "/DataPersistence/CSVData/old-reviews.csv";
    private readonly string _filePath =  "/DataPersistence/CSVData/reviews.csv";
    
    public ReviewPersistence(IFileOperations fileOperations)
    {
        _fileOperations = fileOperations;
    }
    
    public List<Review> ReadReviews()
    {
        if (!_fileOperations.FileExists(_filePath))
        {
            return new List<Review>();
        }
        var savedReviews = new List<Review>();
        using var reader = new StreamReader(_filePath);
        var headerLine = reader.ReadLine();
        string currentLine;
        while ((currentLine = reader.ReadLine()) != null)
        {
            string[] fields = currentLine.Split(',');
            
            var review = new Review
            {
                Id = Guid.Parse(fields[0]),
                Rating = Rating.From(int.Parse(fields[1])),
                FilmId = Guid.Parse(fields[2])
            };
            savedReviews.Add(review);
        }
        _fileOperations.CreateBackupCSVFile(_filePath, _oldFilmRatingsFilePath);
        return savedReviews;
    }

    private void CreateFileHeader()
    {
        using var file = new StreamWriter(_filePath);
        file.WriteLine("Id" + "," + "Rating" + "," + "Film Id");
    }
    
    public void WriteReviews(List<Review> reviews)
    {
        _fileOperations.CreateBackupCSVFile(_filePath, _oldFilmRatingsFilePath);
        CreateFileHeader();
        if (reviews.Count <= 0) return;
        foreach (var review in reviews)
        {
            WriteReview(review.Id, review.Rating, review.FilmId);
        }
    }

    private void WriteReview(Guid id, Rating rating, Guid filmId)
    {
        try
        {
            using var file = new StreamWriter(_filePath, true);
            file.WriteLine(id + ","  + rating + "," + filmId);
            file.Close();
        }  
        catch(Exception ex)  
        {  
            Console.Write(ex.Message);  
        } 
    }
    
}