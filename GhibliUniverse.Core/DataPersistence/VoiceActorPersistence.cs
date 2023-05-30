using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Core.DataPersistence;

public class VoiceActorPersistence : IVoiceActorPersistence
{
    private readonly FileOperations _fileOperations;
    private const string OldVoiceActorsFilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/old-voice-actors.csv";
    private const string FilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/voice-actors.csv";

    public VoiceActorPersistence(FileOperations fileOperations)
    {
        _fileOperations = fileOperations;
    }
    public List<VoiceActor> ReadVoiceActors()
    {
        if (!_fileOperations.FileExists(FilePath))
        {
            return new List<VoiceActor>();
        }
        var savedVoiceActors = new List<VoiceActor>();
        using var reader = new StreamReader(FilePath);
        var headerLine = reader.ReadLine();
        string currentLine;
        while ((currentLine = reader.ReadLine()) != null)
        {
            string[] fields = currentLine.Split(',');

            savedVoiceActors.Add(new VoiceActor
            {
                Id = Guid.Parse(fields[0]),
                Name = ValidatedString.From(fields[1])
            });
        }
        return savedVoiceActors;
    }
    
    private void CreateFileHeader()
    {
        using var file = new StreamWriter(FilePath);
        file.WriteLine("Id" + "," + "Name");
    }
    
    public void WriteVoiceActors(List<VoiceActor> voiceActors)
    {
        _fileOperations.CreateBackupCSVFile(FilePath, OldVoiceActorsFilePath);
        CreateFileHeader();
        if (voiceActors.Count <= 0) return;
        foreach (var voiceActor in voiceActors)
        {
            WriteVoiceActor(voiceActor.Id, voiceActor.Name);
        }
    }
    
    private void WriteVoiceActor(Guid id, ValidatedString name)
    {
        try
        {
            using var file = new StreamWriter(FilePath, true);
            file.WriteLine(id + ","  + name);
            file.Close();
        }  
        catch(Exception ex)  
        {  
            Console.Write(ex.Message);  
        } 
    }

}