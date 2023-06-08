namespace GhibliUniverse.Core.DataPersistence;

public interface IFileOperations
{
    void CreateBackupCSVFile(string latestFilePath, string oldFilePath);
    bool FileExists(string filePath);
}