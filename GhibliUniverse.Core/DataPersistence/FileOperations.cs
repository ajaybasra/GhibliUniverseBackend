namespace GhibliUniverse.Core.DataPersistence;

public class FileOperations : IFileOperations
{
    public void CreateBackupCSVFile(string latestFilePath, string oldFilePath)
    {
        var lines = File.ReadAllLines(latestFilePath);
        File.WriteAllLines(oldFilePath, lines);
    }

    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }
}

    
