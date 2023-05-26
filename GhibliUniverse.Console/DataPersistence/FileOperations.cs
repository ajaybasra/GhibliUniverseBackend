namespace GhibliUniverse.Console.DataPersistence;

public class FileOperations
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

    
