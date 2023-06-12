using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Core.DataPersistence;

public class VoiceActorPersistence : IVoiceActorPersistence
{
    private readonly IFileOperations _fileOperations;
    private static readonly string BaseDirectory = AppContext.BaseDirectory;
    // private static readonly string RootDirectory = Directory.GetParent(BaseDirectory).Parent.Parent.Parent.Parent.FullName;
    // private readonly string _oldVoiceActorsFilePath = Path.Combine(RootDirectory, "GhibliUniverse.Core/DataPersistence/CSVData/old-voice-actors.csv");
    // private readonly string _filePath = Path.Combine(RootDirectory, "GhibliUniverse.Core/DataPersistence/CSVData/voice-actors.csv");
    private readonly string _oldVoiceActorsFilePath =
        "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/GhibliUniverse.Core/DataPersistence/CSVData/old-voice-actors.csv";
    private readonly string _filePath =
        "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/GhibliUniverse.Core/DataPersistence/CSVData/voice-actors.csv";

    public VoiceActorPersistence(IFileOperations fileOperations)
    {
        _fileOperations = fileOperations;
    }
    public List<VoiceActor> ReadVoiceActors()
    {
        if (!_fileOperations.FileExists(_filePath))
        {
            return new List<VoiceActor>();
        }
        var savedVoiceActors = new List<VoiceActor>();
        using var reader = new StreamReader(_filePath);
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
        using var file = new StreamWriter(_filePath);
        file.WriteLine("Id" + "," + "Name");
    }
    
    public void WriteVoiceActors(List<VoiceActor> voiceActors)
    {
        _fileOperations.CreateBackupCSVFile(_filePath, _oldVoiceActorsFilePath);
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
            using var file = new StreamWriter(_filePath, true);
            file.WriteLine(id + ","  + name);
            file.Close();
        }  
        catch(Exception ex)  
        {  
            Console.Write(ex.Message);  
        } 
    }

}