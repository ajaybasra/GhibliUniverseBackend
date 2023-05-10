// See https://aka.ms/new-console-template for more information

using GhibliUniverse;
using GhibliUniverse.Interfaces;

var filmUniverse = new FilmUniverse();
var commandLine = new CommandLine();
var argumentProcessor = new ArgumentProcessor(commandLine, filmUniverse);
argumentProcessor.Process();
// var s = filmUniverse.BuildFilmList();
// Console.WriteLine(s);
// var fp = "/Users/Ajay.Basra/Repos/Katas/GhibliUniverse/ohhhhh.txt";
// var file = new StreamWriter(@fp, true);