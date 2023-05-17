using GhibliUniverse.Interfaces;

namespace GhibliUniverse.DataPersistence;

public class VoiceActorPersistence : IPersistence
{
    private readonly FilmUniverse _filmUniverse;
    private const string OldVoiceActorsFilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/old-voice-actors.csv";
    private const string FilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/voice-actors.csv";

    public VoiceActorPersistence(FilmUniverse filmUniverse)
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
        var allVoiceActors = _filmUniverse.GetAllVoiceActors();
        CreateFileHeader();
        if (allVoiceActors.Count <= 0) return;
        foreach (var voiceActor in allVoiceActors)
        {
            AddVoiceActorRecordFromFilmUniverseToCSV(voiceActor.Id, voiceActor.FirstName, voiceActor.LastName);
        }
    }
    
    private void CreateFileHeader()
    {
        using var file = new StreamWriter(FilePath);
        file.WriteLine("Id" + "," + "First Name" + "," + "Last Name");
    }
    
    private void AddVoiceActorRecordFromFilmUniverseToCSV(Guid id, string firstName, string lastName)
    {
        try
        {
            using var file = new StreamWriter(FilePath, true);
            file.WriteLine(id + ","  + firstName + "," + lastName);
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
            .ForEach(properties => AddFilmFromCSVToVoiceActorList(new Guid(properties[0]),properties[1], properties[2]));
    }
    
    private void AddFilmFromCSVToVoiceActorList(Guid id, string firstName, string lastName)
    {
        var voiceActor = new VoiceActor()
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName
        };
        
        _filmUniverse.AddVoiceActor(voiceActor);
    }

    private void CreateBackupCSVFile()
    {
        var lines = File.ReadAllLines(FilePath);
        File.WriteAllLines(OldVoiceActorsFilePath, lines);
    }

    public bool FileExists()
    {
        return File.Exists(FilePath);
    }
}