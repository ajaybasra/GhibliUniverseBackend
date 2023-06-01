using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;

namespace GhibliUniverse.Console;

public class ArgumentProcessor
{
    private readonly string[] _programArguments;
    private readonly IFilmService _filmService;
    private readonly IVoiceActorService _voiceActorService;
    private readonly IReviewService _reviewService;

    public ArgumentProcessor(ICommandLine commandLine, IFilmService filmService, IReviewService reviewService, IVoiceActorService voiceActorService)
    {
        _programArguments = commandLine.GetCommandLineArgs();
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
            case "add-voice-actor-to-film":
                HandleAddVoiceActorToFilm(_programArguments[2], _programArguments[3]);
                break;
            case "remove-voice-actor-from-film":
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
                System.Console.WriteLine("Invalid command(s)!");
                break;
        }
    }

    private void HandleGetAllFilms()
    {
        PrintModelEntries(_filmService.GetAllFilms());
    }

    private void HandleGetFilmById(string filmId)
    {
        try
        {
            System.Console.WriteLine(_filmService.GetFilmById(Guid.Parse(filmId)));
        }
        catch (FormatException fe)
        {
            System.Console.WriteLine(fe);
        }
        catch (ModelNotFoundException e)
        {
            System.Console.WriteLine(e);
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
            System.Console.WriteLine(fe);
        }
        catch (ModelNotFoundException e)
        {
            System.Console.WriteLine(e);
        }
    }

    private void HandleCreateFilm()
    {
        var releaseYear = int.Parse(GetListOfPropertiesNeededToCreateFilm()[4]);

        _filmService.CreateFilm(GetListOfPropertiesNeededToCreateFilm()[0], GetListOfPropertiesNeededToCreateFilm()[1], GetListOfPropertiesNeededToCreateFilm()[2], GetListOfPropertiesNeededToCreateFilm()[3], releaseYear);
    }

    private void HandleDeleteFilm(string filmId)
    {
        try
        {
            _filmService.DeleteFilm(Guid.Parse(filmId));
        }
        catch (FormatException fe)
        {
            System.Console.WriteLine(fe);
        }
        catch (ModelNotFoundException e)
        {
            System.Console.WriteLine(e);
        }
    }
    
    private void HandleAddVoiceActorToFilm(string filmId, string voiceActorId)
    {
        try
        {
            var voiceActor = _voiceActorService.GetVoiceActorById(Guid.Parse(voiceActorId));
            _filmService.AddVoiceActor(Guid.Parse(filmId), voiceActor);
        }
        catch (FormatException fe)
        {
            System.Console.WriteLine(fe);
        }
        catch (ModelNotFoundException e)
        {
            System.Console.WriteLine(e);
        }
    }
    
    private void HandleRemoveVoiceActorFromFilm(string filmId, string voiceActorId)
    {
        try
        {
            var voiceActor = _voiceActorService.GetVoiceActorById(Guid.Parse(voiceActorId));
            _filmService.RemoveVoiceActor(Guid.Parse(filmId), voiceActor);
        }
        catch (FormatException fe)
        {
            System.Console.WriteLine(fe);
        }
        catch (ModelNotFoundException e)
        {
            System.Console.WriteLine(e);
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
            System.Console.WriteLine(_voiceActorService.GetVoiceActorById(Guid.Parse(voiceActorId)));
        }
        catch (FormatException fe)
        {
            System.Console.WriteLine(fe);
        }
        catch (ModelNotFoundException e)
        {
            System.Console.WriteLine(e);
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
            System.Console.WriteLine(fe);
        }
        catch (ModelNotFoundException e)
        {
            System.Console.WriteLine(e);
        }
    }

    private void HandleCreateVoiceActor()
    {
        _voiceActorService.CreateVoiceActor(_programArguments[2]);
    }

    private void HandleDeleteVoiceActor(string voiceActorId)
    {
        try
        {
            _voiceActorService.DeleteVoiceActor(Guid.Parse(voiceActorId));
        }
        catch (FormatException fe)
        {
            System.Console.WriteLine(fe);
        }
        catch (ModelNotFoundException e)
        {
            System.Console.WriteLine(e);
        }
    }

    private void HandleGetAllReviews()
    {
        try
        {
            PrintModelEntries(_reviewService.GetAllReviews());
        }
        catch (FormatException fe)
        {
            System.Console.WriteLine(fe);
        }
    }

    private void HandleGetReviewById(string reviewId)
    {
        try
        {
            System.Console.Write(_reviewService.GetReviewById(Guid.Parse(reviewId)));
        }
        catch (FormatException fe)
        {
            System.Console.WriteLine(fe);
        }
        catch (ModelNotFoundException e)
        {
            System.Console.WriteLine(e);
        }
    }

    private void HandleCreateReview(string filmId, string rating)
    {
        try
        {
            var ratingAsInt = int.Parse(rating);
            _reviewService.CreateReview(Guid.Parse(filmId), ratingAsInt);
        }
        catch (Rating.RatingOutOfRangeException e)
        {
            System.Console.WriteLine(e);
        }
        catch (FormatException fe)
        {
            System.Console.WriteLine(fe);
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
            System.Console.WriteLine(fe);
        }
        catch (ModelNotFoundException e)
        {
            System.Console.WriteLine(e);
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
            System.Console.WriteLine(item);
        }
    }
}