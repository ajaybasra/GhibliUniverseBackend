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
            case "get-all-films":
                ShowFilms(_filmUniverse.GetAllFilms());
                break;
            case "get-film-by-id":
                Console.WriteLine(_filmUniverse.GetFilmById(new Guid(_programArguments[2])));
                break;
            case "get-films-filtered-by-property":
                ShowFilms(_filmUniverse.GetFilmsFilteredByProperty(_programArguments[2], GetFilterValuesSeperatedBySpaces()));
                break;
            case "create-film":
                var releaseYear = int.Parse(GetListOfPropertiesNeededToCreateFilm()[4]);

                _filmUniverse.CreateFilm(GetListOfPropertiesNeededToCreateFilm()[0], GetListOfPropertiesNeededToCreateFilm()[1], GetListOfPropertiesNeededToCreateFilm()[2], GetListOfPropertiesNeededToCreateFilm()[3], releaseYear);
                break;
            case "delete-film":
                _filmUniverse.DeleteFilm(new Guid(_programArguments[2]));
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