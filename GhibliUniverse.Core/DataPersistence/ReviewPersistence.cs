using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Core.DataPersistence;

public class ReviewPersistence 
{
    private readonly FileOperations _fileOperations;
    private const string OldFilmRatingsFilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/old-reviews.csv";
    private const string FilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/reviews.csv";

    public ReviewPersistence(FileOperations fileOperations)
    {
        _fileOperations = fileOperations;
    }
    
    public List<Review> ReadReviews()
    {
        if (!_fileOperations.FileExists(FilePath))
        {
            return new List<Review>();
        }
        var savedReviews = new List<Review>();
        using var reader = new StreamReader(FilePath);
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
        _fileOperations.CreateBackupCSVFile(FilePath, OldFilmRatingsFilePath);
        return savedReviews;
    }

    public void WriteReviews(List<Review> reviews)
    {
        CreateFileHeader();
        if (reviews.Count <= 0) return;
        foreach (var review in reviews)
        {
            WriteReview(review.Id, review.Rating, review.FilmId);
        }
    }
    
    private void CreateFileHeader()
    {
        using var file = new StreamWriter(FilePath);
        file.WriteLine("Id" + "," + "Rating" + "," + "Film Id");
    }
    
    private void WriteReview(Guid id, Rating rating, Guid filmId)
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
    
}