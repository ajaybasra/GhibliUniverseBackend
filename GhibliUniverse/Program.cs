// See https://aka.ms/new-console-template for more information

using GhibliUniverse;
using GhibliUniverse.DataPersistence;
using GhibliUniverse.Interfaces;

var filmUniverse = new FilmUniverse();
var commandLine = new CommandLine();
var argumentProcessor = new ArgumentProcessor(commandLine, filmUniverse);
var moviePersistence = new FilmPersistence(filmUniverse);
moviePersistence.ReadingStep();
// filmUniverse.PopulateFilmsList(1);
// argumentProcessor.Process();
// filmUniverse.PopulateFilmsList(1);
// filmUniverse.DeleteFilm(new Guid("396c8970-906c-4bc0-b77e-d34317fac0e6"));
// filmUniverse.CreateFilm("Batman Begins", "A man dresses up as a bat; and fights crime yo.", "Christopher Nolan", "Hans Zimmer", 2005);
moviePersistence.WritingStep();

Console.WriteLine(filmUniverse.GetAllFilms().Count);

var s = filmUniverse.BuildFilmList();
Console.WriteLine(s);
var va = new VoiceActor();
Console.WriteLine(va.Films);