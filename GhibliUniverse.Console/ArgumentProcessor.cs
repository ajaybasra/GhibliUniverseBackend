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

    private async Task HandleGetAllFilms()
    {
        var allFilms = await _filmService.GetAllFilms();
        PrintModelEntries(allFilms);
    }

    private async Task HandleGetFilmById(string filmId)
    {
        try
        {
            _writer.WriteLine(await _filmService.GetFilmById(Guid.Parse(filmId)));
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

    private async Task HandleGetVoiceActorsByFilm(string filmId)
    {
        try
        {
            var voiceActors = await _filmService.GetVoiceActorsByFilm(Guid.Parse(filmId));
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

    private async Task HandleCreateFilm()
    {
        var releaseYear = int.Parse(GetListOfPropertiesNeededToCreateFilm()[4]);

        try
        {
            await _filmService.CreateFilm(GetListOfPropertiesNeededToCreateFilm()[0], GetListOfPropertiesNeededToCreateFilm()[1], GetListOfPropertiesNeededToCreateFilm()[2], GetListOfPropertiesNeededToCreateFilm()[3], releaseYear);
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

    private async Task HandleDeleteFilm(string filmId)
    {
        try
        {
            await _filmService.DeleteFilm(Guid.Parse(filmId));
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
    
    private async Task HandleAddVoiceActorToFilm(string filmId, string voiceActorId)
    {
        try
        {
            await _filmService.LinkVoiceActor(Guid.Parse(filmId), Guid.Parse(voiceActorId));
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
    
    private async Task HandleRemoveVoiceActorFromFilm(string filmId, string voiceActorId)
    {
        try
        {
            await _filmService.UnlinkVoiceActor(Guid.Parse(filmId), Guid.Parse(voiceActorId));
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

    private async Task HandleGetAllVoiceActors()
    {
        PrintModelEntries(await _voiceActorService.GetAllVoiceActors());
    }

    private async Task HandleGetVoiceActorById(string voiceActorId)
    {
        try
        {
            _writer.WriteLine(await _voiceActorService.GetVoiceActorById(Guid.Parse(voiceActorId)));
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

    private async Task HandleGetFilmsByVoiceActor(string voiceActorId)
    {
        try
        {
            var films = await _voiceActorService.GetFilmsByVoiceActor(Guid.Parse(voiceActorId));
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

    private async Task HandleCreateVoiceActor()
    {
        try
        {
            await _voiceActorService.CreateVoiceActor(_programArguments[2]);
        }
        catch (ArgumentException ae)
        {
            _writer.WriteLine(ae);
        }
    }

    private async Task HandleDeleteVoiceActor(string voiceActorId)
    {
        try
        {
            await _voiceActorService.DeleteVoiceActor(Guid.Parse(voiceActorId));
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

    private async Task HandleGetAllReviews()
    {
        var allReviews = await _reviewService.GetAllReviews();
        PrintModelEntries(allReviews);
    }

    private async Task HandleGetReviewById(string reviewId)
    {
        try
        {
            _writer.Write(await _reviewService.GetReviewById(Guid.Parse(reviewId)));
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

    private async Task HandleCreateReview(string filmId, string rating)
    {
        try
        {
            var ratingAsInt = int.Parse(rating);
            await _reviewService.CreateReview(Guid.Parse(filmId), ratingAsInt);
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

    private async Task HandleDeleteReview(string reviewId)
    {
        try
        {
            await _reviewService.DeleteReview(Guid.Parse(reviewId));
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