using GhibliUniverse.Console.Interfaces;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Console.DataPersistence;

public class VoiceActorPersistence : IPersistence
{
    private readonly IVoiceActorService _voiceActorService;
    private readonly FileOperations _fileOperations;
    private const string OldVoiceActorsFilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/old-voice-actors.csv";
    private const string FilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/voice-actors.csv";

    public VoiceActorPersistence(IVoiceActorService voiceActorService, FileOperations fileOperations)
    {
        _voiceActorService = voiceActorService;
        _fileOperations = fileOperations;
    }
    public void ReadingStep()
    {
        if (_fileOperations.FileExists(FilePath))
        {
            ReadInRecords();
            _fileOperations.CreateBackupCSVFile(FilePath, OldVoiceActorsFilePath);
        }
    }

    public void WritingStep()
    {
        var allVoiceActors = _voiceActorService.GetAllVoiceActors();
        CreateFileHeader();
        if (allVoiceActors.Count <= 0) return;
        foreach (var voiceActor in allVoiceActors)
        {
            AddVoiceActorRecordFromFilmUniverseToCSV(voiceActor.Id, voiceActor.Name);
        }
    }
    
    private void CreateFileHeader()
    {
        using var file = new StreamWriter(FilePath);
        file.WriteLine("Id" + "," + "Name");
    }
    
    private void AddVoiceActorRecordFromFilmUniverseToCSV(Guid id, ValidatedString name)
    {
        try
        {
            using var file = new StreamWriter(FilePath, true);
            file.WriteLine(id + ","  + name);
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
            .ForEach(properties => AddFilmFromCSVToVoiceActorList(new Guid(properties[0]),properties[1]));
    }
    
    private void AddFilmFromCSVToVoiceActorList(Guid id, string name)
    {
        var voiceActor = new VoiceActor()
        {
            Id = id,
            Name = ValidatedString.From(name)
        };
        
        _voiceActorService.AddVoiceActor(voiceActor);
    }

}