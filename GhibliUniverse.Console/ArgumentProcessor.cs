using GhibliUniverse.Console.Interfaces;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;

namespace GhibliUniverse.Console;

public class ArgumentProcessor
{
    private readonly string[] _programArguments;
    private readonly FilmUniverse _filmUniverse;

    public ArgumentProcessor(ICommandLine commandLine, FilmUniverse filmUniverse)
    {
        _programArguments = commandLine.GetCommandLineArgs();
        _filmUniverse = filmUniverse;
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
            case "get-films-filtered-by-property":
                HandleGetFilmsFilteredByProperty(_programArguments[2]);
                break;
            case "create-film":
                HandleCreateFilm();
                break;
            case "delete-film":
                HandleDeleteFilm(_programArguments[2]);
                break;
            case "get-all-voice-actors":
                HandleGetAllVoiceActors();
                break;
            case "get-voice-actor-by-id":
                HandleGetVoiceActorById(_programArguments[2]);
                break;
            case "create-voice-actor":
                HandleCreateVoiceActor();
                break;
            case "delete-voice-actor":
                HandleDeleteVoiceActor(_programArguments[2]);
                break;
            case "add-voice-actor-to-film":
                HandleAddVoiceActorToFilm(_programArguments[2], _programArguments[3]);
                break;
            case "remove-voice-actor-from-film":
                HandleRemoveVoiceActorFromFilm(_programArguments[2], _programArguments[3]);
                break;
            case "get-all-film-ratings":
                HandleGetAllFilmRatings(_programArguments[2]);
                break;
            case "get-film-rating-by-id":
                HandleGetFilmRatingById(_programArguments[2], _programArguments[3]);
                break;
            case "create-film-rating":
                HandleCreateFilmRating(_programArguments[2], _programArguments[3]);
                break;
            case "delete-film-rating":
                HandleDeleteFilmRating(_programArguments[2], _programArguments[3]);
                break;
        }
    }

    private void HandleGetAllFilms()
    {
        ShowFilms(_filmUniverse.GetAllFilms());
    }

    private void HandleGetFilmById(string filmId)
    {
        try
        {
            System.Console.WriteLine(_filmUniverse.GetFilmById(new Guid(filmId)));
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

    private void HandleGetFilmsFilteredByProperty(string propertyName)
    {
        try
        {
            ShowFilms(_filmUniverse.GetFilmsFilteredByProperty(propertyName, GetFilterValuesSeperatedBySpaces()));
        }
        catch (ArgumentException ae)
        {
            System.Console.WriteLine(ae);
        }

    }

    private void HandleCreateFilm()
    {
        var releaseYear = int.Parse(GetListOfPropertiesNeededToCreateFilm()[4]);

        _filmUniverse.CreateFilm(GetListOfPropertiesNeededToCreateFilm()[0], GetListOfPropertiesNeededToCreateFilm()[1], GetListOfPropertiesNeededToCreateFilm()[2], GetListOfPropertiesNeededToCreateFilm()[3], releaseYear);
    }

    private void HandleDeleteFilm(string filmId)
    {
        try
        {
            _filmUniverse.DeleteFilm(new Guid(filmId));
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
        ShowVoiceActors(_filmUniverse.GetAllVoiceActors());
    }

    private void HandleGetVoiceActorById(string voiceActorId)
    {
        try
        {
            System.Console.WriteLine(_filmUniverse.GetVoiceActorById(new Guid(voiceActorId)));
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
        _filmUniverse.CreateVoiceActor(_programArguments[2]);
    }

    private void HandleDeleteVoiceActor(string voiceActorId)
    {
        try
        {
            _filmUniverse.DeleteVoiceActor(new Guid(voiceActorId));
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
            var film = _filmUniverse.GetFilmById(new Guid(filmId));
            var voiceActor = _filmUniverse.GetVoiceActorById(new Guid(voiceActorId));
            film.AddVoiceActor(voiceActor);
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
            var film = _filmUniverse.GetFilmById(new Guid(filmId));
            var voiceActor = _filmUniverse.GetVoiceActorById(new Guid(voiceActorId));

            film.RemoveVoiceActor(voiceActor);
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

    private void HandleGetAllFilmRatings(string filmId)
    {
        try
        {
            ShowFilmRatings(_filmUniverse.GetAllFilmRatings(new Guid(filmId)));
        }
        catch (FormatException fe)
        {
            System.Console.WriteLine(fe);
        }
    }

    private void HandleGetFilmRatingById(string filmId, string filmRatingId)
    {
        try
        {
            System.Console.Write(_filmUniverse.GetFilmRatingById(new Guid(filmId), new Guid(filmRatingId)));
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

    private void HandleCreateFilmRating(string rating, string filmId)
    {
        try
        {
            var ratingAsInt = int.Parse(rating);
            _filmUniverse.CreateFilmRating(ratingAsInt, new Guid(filmId));
        }
        catch (FormatException fe)
        {
            System.Console.WriteLine(fe);
        }
    }

    private void HandleDeleteFilmRating(string filmId, string filmRatingId)
    {
        try
        {
            _filmUniverse.DeleteFilmRating(new Guid(filmId), new Guid(filmRatingId));
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
    private string GetFilterValuesSeperatedBySpaces()
    {
        var rawFilterValues = new List<string>();
        rawFilterValues.AddRange(_programArguments.Skip(3).Select(arg => arg));
        var filterValuesSeperatedBySpaces = string.Join(" ", rawFilterValues.Where(s => !String.IsNullOrEmpty(s)));
        return filterValuesSeperatedBySpaces;
    }
    
    private List<string> GetListOfPropertiesNeededToCreateFilm()
    {
        var rawFilmProperties = new List<string>();
        rawFilmProperties.AddRange(_programArguments.Skip(2).Select(arg => arg));
        var filmPropertiesSeperatedBySpaces = string.Join(" ", rawFilmProperties.Where(s => !String.IsNullOrEmpty(s)));
        var listOfProperties = filmPropertiesSeperatedBySpaces.Split("-");
        return listOfProperties.ToList();

    }
    
    private void ShowFilms(List<Film> films)
    {
        films.ForEach(System.Console.WriteLine);
    }

    private void ShowVoiceActors(List<VoiceActor> voiceActors)
    {
        voiceActors.ForEach(System.Console.WriteLine);
    }

    private void ShowFilmRatings(List<FilmRating> filmRatings)
    {
        filmRatings.ForEach(System.Console.WriteLine);
    }
}