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

    public async void Process()
    {
        switch (_programArguments[1])
        {           
            case "get-all-films":
                await HandleGetAllFilms();
                break;
            case "get-film-by-id":
                await HandleGetFilmById(_programArguments[2]);
                break;
            case "get-voice-actors-by-film":
                await HandleGetVoiceActorsByFilm(_programArguments[2]);
                break;
            case "create-film":
                HandleCreateFilm();
                break;
            case "delete-film":
                await HandleDeleteFilm(_programArguments[2]);
                break;
            case "link-voice-actor-to-film":
                await HandleAddVoiceActorToFilm(_programArguments[2], _programArguments[3]);
                break;
            case "unlink-voice-actor-from-film":
                await HandleRemoveVoiceActorFromFilm(_programArguments[2], _programArguments[3]);
                break;
            case "get-all-voice-actors":
                await HandleGetAllVoiceActors();
                break;
            case "get-voice-actor-by-id":
                await HandleGetVoiceActorById(_programArguments[2]);
                break;
            case "get-films-by-voice-actor":
                await HandleGetFilmsByVoiceActor(_programArguments[2]);
                break;
            case "create-voice-actor":
                await HandleCreateVoiceActor();
                break;
            case "delete-voice-actor":
                await HandleDeleteVoiceActor(_programArguments[2]);
                break;
            case "get-all-reviews":
                await HandleGetAllReviews();
                break;
            case "get-review-by-id":
                await HandleGetReviewById(_programArguments[2]);
                break;
            case "create-review":
                await HandleCreateReview(_programArguments[2], _programArguments[3]);
                break;
            case "delete-review":
                await HandleDeleteReview(_programArguments[2]);
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
            var filmInfo = new FilmInfo()
            {
                Title = ValidatedString.From(GetListOfPropertiesNeededToCreateFilm()[0]),
                Description = ValidatedString.From(GetListOfPropertiesNeededToCreateFilm()[1]),
                Director = ValidatedString.From(GetListOfPropertiesNeededToCreateFilm()[2]),
                Composer = ValidatedString.From(GetListOfPropertiesNeededToCreateFilm()[3]),
                ReleaseYear = ReleaseYear.From(releaseYear)
            };
            await _filmService.CreateFilm(new Film(filmInfo));
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
            var voiceActor = new VoiceActor()
            {
                Name = ValidatedString.From(_programArguments[2])
            };
            await _voiceActorService.CreateVoiceActor(voiceActor);
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
            var review = new Review()
            {
                Rating = Rating.From(int.Parse(rating))
            };
            await _reviewService.CreateReview(Guid.Parse(filmId), review);
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