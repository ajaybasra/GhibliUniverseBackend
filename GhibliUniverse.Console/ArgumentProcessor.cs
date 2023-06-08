using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Console;

public class ArgumentProcessor
{
    private readonly string[] _programArguments;
    private readonly IWriter _writer;
    private readonly IFilmService _filmService;
    private readonly IVoiceActorService _voiceActorService;
    private readonly IReviewService _reviewService;

    public ArgumentProcessor(ICommandLine commandLine, IWriter writer, IFilmService filmService, IReviewService reviewService, IVoiceActorService voiceActorService)
    {
        _programArguments = commandLine.GetCommandLineArgs();
        _writer = writer;
        _filmService = filmService;
        _reviewService = reviewService;
        _voiceActorService = voiceActorService;
    }

    public void Process()
    {
        switch (_programArguments[1])
        {           
            case "get-all-films":
                HandleGetAllFilms();
                break;
            case "get-film-by-id":
                HandleGetFilmById(_programArguments[2]);
                break;
            case "get-voice-actors-by-film":
                HandleGetVoiceActorsByFilm(_programArguments[2]);
                break;
            case "create-film":
                HandleCreateFilm();
                break;
            case "delete-film":
                HandleDeleteFilm(_programArguments[2]);
                break;
            case "link-voice-actor-to-film":
                HandleAddVoiceActorToFilm(_programArguments[2], _programArguments[3]);
                break;
            case "unlink-voice-actor-from-film":
                HandleRemoveVoiceActorFromFilm(_programArguments[2], _programArguments[3]);
                break;
            case "get-all-voice-actors":
                HandleGetAllVoiceActors();
                break;
            case "get-voice-actor-by-id":
                HandleGetVoiceActorById(_programArguments[2]);
                break;
            case "get-films-by-voice-actor":
                HandleGetFilmsByVoiceActor(_programArguments[2]);
                break;
            case "create-voice-actor":
                HandleCreateVoiceActor();
                break;
            case "delete-voice-actor":
                HandleDeleteVoiceActor(_programArguments[2]);
                break;
            case "get-all-reviews":
                HandleGetAllReviews();
                break;
            case "get-review-by-id":
                HandleGetReviewById(_programArguments[2]);
                break;
            case "create-review":
                HandleCreateReview(_programArguments[2], _programArguments[3]);
                break;
            case "delete-review":
                HandleDeleteReview(_programArguments[2]);
                break;
            default:
                _writer.WriteLine("Invalid command(s)!");
                break;
        }
    }

    private void HandleGetAllFilms()
    {
        var allFilms = _filmService.GetAllFilms();
        PrintModelEntries(allFilms);
    }

    private void HandleGetFilmById(string filmId)
    {
        try
        {
            _writer.WriteLine(_filmService.GetFilmById(Guid.Parse(filmId)));
        }
        catch (FormatException fe)
        {
            _writer.WriteLine(fe.Message);
        }
        catch (ModelNotFoundException e)
        {
            _writer.WriteLine(e.Message);
        }
    }

    private void HandleGetVoiceActorsByFilm(string filmId)
    {
        try
        {
            var voiceActors = _filmService.GetVoiceActorsByFilm(Guid.Parse(filmId));
            PrintModelEntries(voiceActors);
        }
        catch (FormatException fe)
        {
            _writer.WriteLine(fe.Message);
        }
        catch (ModelNotFoundException e)
        {
            _writer.WriteLine(e.Message);
        }
    }

    private void HandleCreateFilm()
    {
        var releaseYear = int.Parse(GetListOfPropertiesNeededToCreateFilm()[4]);

        try
        {
            _filmService.CreateFilm(GetListOfPropertiesNeededToCreateFilm()[0], GetListOfPropertiesNeededToCreateFilm()[1], GetListOfPropertiesNeededToCreateFilm()[2], GetListOfPropertiesNeededToCreateFilm()[3], releaseYear);
        }
        catch (ReleaseYear.NotFourCharactersException e)
        {
            _writer.WriteLine(e);
        }
        catch (ReleaseYear.ReleaseYearLessThanOldestReleaseYearException e)
        {
            _writer.WriteLine(e);
        }
        catch (ArgumentException ae)
        {
            _writer.WriteLine(ae);
        }
    }

    private void HandleDeleteFilm(string filmId)
    {
        try
        {
            _filmService.DeleteFilm(Guid.Parse(filmId));
        }
        catch (FormatException fe)
        {
            _writer.WriteLine(fe.Message);
        }
        catch (ModelNotFoundException e)
        {
            _writer.WriteLine(e.Message);
        }
    }
    
    private void HandleAddVoiceActorToFilm(string filmId, string voiceActorId)
    {
        try
        {
            _filmService.LinkVoiceActor(Guid.Parse(filmId), Guid.Parse(voiceActorId));
        }
        catch (FormatException fe)
        {
            _writer.WriteLine(fe.Message);
        }
        catch (ModelNotFoundException e)
        {
            _writer.WriteLine(e.Message);
        }
    }
    
    private void HandleRemoveVoiceActorFromFilm(string filmId, string voiceActorId)
    {
        try
        {
            _filmService.UnlinkVoiceActor(Guid.Parse(filmId), Guid.Parse(voiceActorId));
        }
        catch (FormatException fe)
        {
            _writer.WriteLine(fe.Message);
        }
        catch (ModelNotFoundException e)
        {
            _writer.WriteLine(e.Message);
        }
    }

    private void HandleGetAllVoiceActors()
    {
        PrintModelEntries(_voiceActorService.GetAllVoiceActors());
    }

    private void HandleGetVoiceActorById(string voiceActorId)
    {
        try
        {
            _writer.WriteLine(_voiceActorService.GetVoiceActorById(Guid.Parse(voiceActorId)));
        }
        catch (FormatException fe)
        {
            _writer.WriteLine(fe.Message);
        }
        catch (ModelNotFoundException e)
        {
            _writer.WriteLine(e.Message);
        }
    }

    private void HandleGetFilmsByVoiceActor(string voiceActorId)
    {
        try
        {
            var films = _voiceActorService.GetFilmsByVoiceActor(Guid.Parse(voiceActorId));
            PrintModelEntries(films);
        }
        catch (FormatException fe)
        {
            _writer.WriteLine(fe.Message);
        }
        catch (ModelNotFoundException e)
        {
            _writer.WriteLine(e.Message);
        }
    }

    private void HandleCreateVoiceActor()
    {
        try
        {
            _voiceActorService.CreateVoiceActor(_programArguments[2]);
        }
        catch (ArgumentException ae)
        {
            _writer.WriteLine(ae);
        }
    }

    private void HandleDeleteVoiceActor(string voiceActorId)
    {
        try
        {
            _voiceActorService.DeleteVoiceActor(Guid.Parse(voiceActorId));
        }
        catch (FormatException fe)
        {
            _writer.WriteLine(fe.Message);
        }
        catch (ModelNotFoundException e)
        {
            _writer.WriteLine(e.Message);
        }
    }

    private void HandleGetAllReviews()
    {
        PrintModelEntries(_reviewService.GetAllReviews());
    }

    private void HandleGetReviewById(string reviewId)
    {
        try
        {
            _writer.Write(_reviewService.GetReviewById(Guid.Parse(reviewId)));
        }
        catch (FormatException fe)
        {
            _writer.WriteLine(fe.Message);
        }
        catch (ModelNotFoundException e)
        {
            _writer.WriteLine(e.Message);
        }
    }

    private void HandleCreateReview(string filmId, string rating)
    {
        try
        {
            var ratingAsInt = int.Parse(rating);
            _reviewService.CreateReview(Guid.Parse(filmId), ratingAsInt);
        }
        catch (ModelNotFoundException e)
        {
            _writer.WriteLine(e.Message);
        }
        catch (Rating.RatingOutOfRangeException e)
        {
            _writer.WriteLine(e.Message);
        }
        catch (FormatException fe)
        {
            _writer.WriteLine(fe.Message);
        }
    }

    private void HandleDeleteReview(string reviewId)
    {
        try
        {
            _reviewService.DeleteReview(Guid.Parse(reviewId));
        }
        catch (FormatException fe)
        {
            _writer.WriteLine(fe.Message);
        }
        catch (ModelNotFoundException e)
        {
            _writer.WriteLine(e.Message);
        }
    }

    private List<string> GetListOfPropertiesNeededToCreateFilm()
    {
        var rawFilmProperties = new List<string>();
        rawFilmProperties.AddRange(_programArguments.Skip(2).Select(arg => arg));
        var filmPropertiesSeperatedBySpaces = string.Join(" ", rawFilmProperties.Where(s => !String.IsNullOrEmpty(s)));
        var listOfProperties = filmPropertiesSeperatedBySpaces.Split("-");
        return listOfProperties.ToList();

    }

    private void PrintModelEntries<T>(List<T> list)
    {
        foreach (var item in list)
        {
            _writer.WriteLine(item);
        }
    }
}