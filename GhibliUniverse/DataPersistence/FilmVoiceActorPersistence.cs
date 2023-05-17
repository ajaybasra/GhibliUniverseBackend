using GhibliUniverse.Interfaces;

namespace GhibliUniverse.DataPersistence;

public class FilmVoiceActorPersistence : IPersistence
{
    private readonly FilmUniverse _filmUniverse;
    private const string OldFilmVoiceActorFilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/old-film-and-voice-actor-ids.csv";
    private const string FilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/film-and-voice-actor-ids.csv";

    public FilmVoiceActorPersistence(FilmUniverse filmUniverse)
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
        var allFilms = _filmUniverse.GetAllFilms();
        CreateFileHeader();
        if (allFilms.Count <= 0) return;
        foreach (var film in allFilms)
        {
            foreach (var voiceActor in film.VoiceActors)
            {
                AddFilmIdsAndVoiceActorIdsToCSV(film.Id, voiceActor.Id);
            }
        }
    }

    private void CreateFileHeader()
    {
        using var file = new StreamWriter(FilePath); 
        file.WriteLine("FilmId" + "," + "VoiceActorId");
    }
        
    private void AddFilmIdsAndVoiceActorIdsToCSV(Guid filmId, Guid voiceActorId)
    {
        try
        {
            using var file = new StreamWriter(FilePath, true);
            file.WriteLine(filmId + "," + voiceActorId);
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
            .ForEach(properties => AddVoiceActorsFromCSVToMatchingFilm(new Guid(properties[0]),new Guid(properties[1])));
        
    }
    private void AddVoiceActorsFromCSVToMatchingFilm(Guid filmId, Guid voiceActorId)
    {
        var film = _filmUniverse.GetFilmById(filmId);
        var voiceActor = _filmUniverse.GetVoiceActorById(voiceActorId);
        
        film.AddVoiceActor(voiceActor);
    }

    private void CreateBackupCSVFile()
    {
        var lines = File.ReadAllLines(FilePath);
        File.WriteAllLines(OldFilmVoiceActorFilePath, lines);
    }

    public bool FileExists()
    {
        return File.Exists(FilePath);
    }
}