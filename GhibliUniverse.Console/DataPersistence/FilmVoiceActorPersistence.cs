using GhibliUniverse.Console.Interfaces;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Console.DataPersistence;

public class FilmVoiceActorPersistence : IPersistence
{
    private readonly FilmService _filmService;
    private readonly VoiceActorService _voiceActorService;
    private readonly FileOperations _fileOperations;
    private const string OldFilmVoiceActorFilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/old-film-and-voice-actor-ids.csv";
    private const string FilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/film-and-voice-actor-ids.csv";

    public FilmVoiceActorPersistence(FilmService filmService, VoiceActorService voiceActorService, FileOperations fileOperations)
    {
        _filmService = filmService;
        _voiceActorService = voiceActorService;
        _fileOperations = fileOperations;
    }
    
    public void ReadingStep()
    {
        if (_fileOperations.FileExists(FilePath))
        {
            ReadInRecords();
            _fileOperations.CreateBackupCSVFile(FilePath, OldFilmVoiceActorFilePath);
        }
    }

    public void WritingStep()
    {
        var allFilms = _filmService.GetAllFilms();
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
            .ForEach(properties => AddVoiceActorsFromCSVToMatchingFilm(new Guid(properties[0]),new Guid(properties[1])));
        
    }
    private void AddVoiceActorsFromCSVToMatchingFilm(Guid filmId, Guid voiceActorId)
    {
        var film = _filmService.GetFilmById(filmId);
        var voiceActor = _voiceActorService.GetVoiceActorById(voiceActorId);
        
        film.AddVoiceActor(voiceActor);
    }

}