using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Core.DataPersistence;

public class FilmVoiceActorPersistence 
{
    private readonly FileOperations _fileOperations;
    private readonly FilmPersistence _filmPersistence;
    private readonly VoiceActorPersistence _voiceActorPersistence;
    private const string OldFilmVoiceActorFilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/old-film-and-voice-actor-ids.csv";
    private const string FilePath = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/CSVData/film-and-voice-actor-ids.csv";

    public FilmVoiceActorPersistence(FileOperations fileOperations, FilmPersistence filmPersistence, VoiceActorPersistence voiceActorPersistence)
    {
        _fileOperations = fileOperations;
        _filmPersistence = filmPersistence;
        _voiceActorPersistence = voiceActorPersistence;
    }
    
    public List<(Guid, Guid)> ReadFilmVoiceActorData()
    {
        var savedFilms = _filmPersistence.ReadFilms();
        var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();

        if (!_fileOperations.FileExists(FilePath))
        {
            return new List<(Guid, Guid)>();
        }
        var savedFilmVoiceActorData = new List<(Guid, Guid)>();
        using var reader = new StreamReader(FilePath);
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

    public bool FilmOrVoiceActorHasBeenDeleted(List<Film> films)
    {
        var savedFilms = _filmPersistence.ReadFilms();
        var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();

        var voiceActors = films.SelectMany(f => f.VoiceActors).ToList();

        return films.Count != savedFilms.Count || voiceActors.Count != savedVoiceActors.Count;
    }
    public void WriteFilmVoiceActors(List<Film> films)
    {
        _fileOperations.CreateBackupCSVFile(FilePath, OldFilmVoiceActorFilePath); //backup before overwriting
        CreateFileHeader();
        //
        // if (FilmOrVoiceActorHasBeenDeleted(films))
        // {
        //     var savedFilms = _filmPersistence.ReadFilms();
        //     var savedVoiceActors = _voiceActorPersistence.ReadVoiceActors();
        //     
        //     var voiceActors = films.SelectMany(f => f.VoiceActors).ToList();
        //     
        //     var removedFilms = films.Except(savedFilms);
        //     var removedVoiceActors = films.SelectMany(f => f.VoiceActors).ToList()
        //         .Except(savedVoiceActors);
        //     films.RemoveAll(f => removedFilms.Contains(f));
        //     voiceActors.RemoveAll(v => removedVoiceActors.Contains(v));
        //
        // }
        if (films.Count <= 0) return;
        foreach (var film in films)
        {
            foreach (var voiceActor in film.VoiceActors)
            {
                WriteFilmVoiceActor(film.Id, voiceActor.Id);
            }
        }
    }

    private void CreateFileHeader()
    {
        using var file = new StreamWriter(FilePath); 
        file.WriteLine("FilmId" + "," + "VoiceActorId");
    }
        
    private void WriteFilmVoiceActor(Guid filmId, Guid voiceActorId)
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
    
    // private void AddVoiceActorsFromCSVToMatchingFilm(Guid filmId, Guid voiceActorId)
    // {
    //     var film = _filmService.GetFilmById(filmId);
    //     var voiceActor = _voiceActorService.GetVoiceActorById(voiceActorId);
    //     
    //     film.AddVoiceActor(voiceActor);
    // }

}