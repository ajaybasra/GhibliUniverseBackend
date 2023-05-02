// See https://aka.ms/new-console-template for more information

using GhibliUniverse;

Console.WriteLine("Hello, World!");
var filmUniverse = new FilmUniverse();
var s = filmUniverse.BuildFilmUniverse();
Console.WriteLine(s);