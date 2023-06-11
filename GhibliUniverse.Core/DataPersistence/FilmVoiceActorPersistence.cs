using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Core.DataPersistence;

public class FilmVoiceActorPersistence : IFilmVoiceActorPersistence
{
    private readonly IFileOperations _fileOperations;
    private readonly IFilmPersistence _filmPersistence;
    private readonly IVoiceActorPersistence _voiceActorPersistence;
    private static readonly string BaseDirectory = AppContext.BaseDirectory;
    private static readonly string RootDirectory = Directory.GetParent(BaseDirectory).Parent.Parent.Parent.Parent.FullName;
    private readonly string _oldFilmVoiceActorFilePath = Path.Combine(RootDirectory, "GhibliUniverse.Core/DataPersistence/CSVData/old-film-and-voice-actor-ids.csv");
    private readonly string _filePath = Path.Combine(RootDirectory, "GhibliUniverse.Core/DataPersistence/CSVData/film-and-voice-actor-ids.csv");
    
    public FilmVoiceActorPersistence(IFileOperations fileOperations, IFilmPersistence filmPersistence, IVoiceActorPersistence voiceActorPersistence)
    {
        _fileOperations = fileOperations;
        _filmPersistence = filmPersistence;
        _voiceActorPersistence = voiceActorPersistence;
    }
    
    public List<(Guid, Guid)> ReadFilmVoiceActorData()
    {
        var savedFilms = _filmPersistence.ReadFilms();
        var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();

        if (!_fileOperations.FileExists(_filePath))
        {
            return new List<(Guid, Guid)>();
        }
        var savedFilmVoiceActorData = new List<(Guid, Guid)>();
        using var reader = new StreamReader(_filePath);
        var headerLine = reader.ReadLine();
        string currentLine;
        while ((currentLine = reader.ReadLine()) != null)
        {
            string[] fields = currentLine.Split(',');
            if (savedFilms.Any(f => f.Id == Guid.Parse(fields[0])) && savedVoiceActors.Any(v => v.Id == Guid.Parse(fields[1])))
            {
                savedFilmVoiceActorData.Add((Guid.Parse(fields[0]), Guid.Parse(fields[1])));

            }
        }

        return savedFilmVoiceActorData;
    }

    private void CreateFileHeader()
    {
        using var file = new StreamWriter(_filePath); 
        file.WriteLine("FilmId" + "," + "VoiceActorId");
    }
    public void WriteFilmVoiceActors(List<Film> films)
    {
        _fileOperations.CreateBackupCSVFile(_filePath, _oldFilmVoiceActorFilePath);  
        CreateFileHeader();
        if (films.Count <= 0) return;
        foreach (var film in films)
        {
            foreach (var voiceActor in film.VoiceActors)
            {
                WriteFilmVoiceActor(film.Id, voiceActor.Id);
            }
        }
    }
    
    private void WriteFilmVoiceActor(Guid filmId, Guid voiceActorId)
    {
        try
        {
            using var file = new StreamWriter(_filePath, true);
            file.WriteLine(filmId + "," + voiceActorId);
            file.Close();
        }  
        catch(Exception ex)  
        {  
            Console.Write(ex.Message);  
        } 
    }
}