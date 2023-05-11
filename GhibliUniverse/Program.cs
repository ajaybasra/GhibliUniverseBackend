// See https://aka.ms/new-console-template for more information

using GhibliUniverse;
using GhibliUniverse.DataPersistence;
using GhibliUniverse.Interfaces;

var filmUniverse = new FilmUniverse();
var commandLine = new CommandLine();
var argumentProcessor = new ArgumentProcessor(commandLine, filmUniverse);
// argumentProcessor.Process();
var moviePersistence = new MoviePersistence(filmUniverse);
moviePersistence.ReadingStep();
// filmUniverse.DeleteFilm(new Guid("6d635055-aa42-440b-af20-d8541ab68a14"));
// filmUniverse.CreateFilm("a","a","a","a",2002);

moviePersistence.WritingStep();
Console.WriteLine(filmUniverse.GetAllFilms().Count);

// var s = filmUniverse.BuildFilmList();
// Console.WriteLine(s);
