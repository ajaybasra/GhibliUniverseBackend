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
// filmUniverse.DeleteFilm(new Guid("00000000-0000-0000-0000-000000000000"));
// filmUniverse.DeleteFilm(new Guid("c89bf4c5-2f29-4ba0-b7bf-f31f00c0b1d6"));
// filmUniverse.CreateFilm("Batman Begins", "A man dresses up as a bat, and fights crime, his name is, vengeance.", "Christopher Nolan", "Hans Zimmer", 2005);
moviePersistence.WritingStep();

Console.WriteLine(filmUniverse.GetAllFilms().Count);

var s = filmUniverse.BuildFilmList();
Console.WriteLine(s);
