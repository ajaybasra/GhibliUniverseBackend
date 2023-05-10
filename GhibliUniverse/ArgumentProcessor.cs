using GhibliUniverse.Interfaces;

namespace GhibliUniverse;

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
            case "gaf":
                ShowFilms(_filmUniverse.GetAllFilms());
                break;
            case "gfbi":
                Console.WriteLine(_filmUniverse.GetFilmById(new Guid(_programArguments[2])));
                break;
            case "gffbp":
                ShowFilms(_filmUniverse.GetFilmsFilteredByProperty(_programArguments[2], GetFilterValuesSeperatedBySpaces()));
                break;
            case "cf":
                var releaseYear = int.Parse(GetListOfPropertiesNeededToCreateFilm()[4]);
                var voiceActors = new List<VoiceActor>();
                var filmRatings = new List<FilmRating>();
                _filmUniverse.CreateFilm(GetListOfPropertiesNeededToCreateFilm()[0], GetListOfPropertiesNeededToCreateFilm()[1], GetListOfPropertiesNeededToCreateFilm()[2], GetListOfPropertiesNeededToCreateFilm()[3], releaseYear, voiceActors, filmRatings);
                ShowFilms(_filmUniverse.GetAllFilms());
                break;
            case "df":
                _filmUniverse.DeleteFilm(new Guid(_programArguments[2]));
                ShowFilms(_filmUniverse.GetAllFilms());
                break;
        }
    }

    private string GetFilterValuesSeperatedBySpaces()
    {
        var rawFilterValues = new List<string>();
        rawFilterValues.AddRange(_programArguments.Skip(3).Select(arg => arg));
        var filterValuesSeperatedBySpaces = string.Join(" ", rawFilterValues.Where(s => !String.IsNullOrEmpty(s)));
        return filterValuesSeperatedBySpaces;
    }

    private void ShowFilms(List<Film> films)
    {
        films.ForEach(Console.WriteLine);
    }

    private List<string> GetListOfPropertiesNeededToCreateFilm()
    {
        var rawFilmProperties = new List<string>();
        rawFilmProperties.AddRange(_programArguments.Skip(2).Select(arg => arg));
        var filmPropertiesSeperatedBySpaces = string.Join(" ", rawFilmProperties.Where(s => !String.IsNullOrEmpty(s)));
        var listOfProperties = filmPropertiesSeperatedBySpaces.Split("-");
        return listOfProperties.ToList();

    }
}