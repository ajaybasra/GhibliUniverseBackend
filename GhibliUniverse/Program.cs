// See https://aka.ms/new-console-template for more information

using GhibliUniverse;

var filmUniverse = new FilmList();
var s = filmUniverse.BuildFilmList();
// Console.WriteLine(filmUniverse.GetAllFilms()[0].FilmId);
Console.WriteLine(s);