// See https://aka.ms/new-console-template for more information

using GhibliUniverse;
using GhibliUniverse.DataPersistence;
using GhibliUniverse.Interfaces;

var filmUniverse = new FilmUniverse();
var commandLine = new CommandLine();
var argumentProcessor = new ArgumentProcessor(commandLine, filmUniverse);
var filmPersistence = new FilmPersistence(filmUniverse);
var voiceActorPersistence = new VoiceActorPersistence(filmUniverse);
var filmRatingPersistence = new FilmRatingPersistence(filmUniverse);
if (filmPersistence.FileExists())
{
    filmPersistence.ReadingStep();
}

if (voiceActorPersistence.FileExists())
{
    voiceActorPersistence.ReadingStep();
}

if (filmRatingPersistence.FileExists())
{
    filmRatingPersistence.ReadingStep();
}

// filmUniverse.PopulateFilmsList(1);
// argumentProcessor.Process();
// filmUniverse.PopulateFilmsList(1);
// filmUniverse.DeleteFilm(new Guid("00000000-0000-0000-0000-000000000000"));
// filmUniverse.DeleteFilm(new Guid("14be95a1-a589-40ed-bd39-f018007fa274"));
// filmUniverse.CreateFilm("Batman Begins", "A man dresses up as a bat, and fights crime, his name is, vengeance.", "Christopher Nolan", "Hans Zimmer", 2005);
// // filmUniverse.CreateVoiceActor("Henry", "Dot");
// filmUniverse.DeleteVoiceActor(new Guid("31ba7e7e-f86c-46f7-b974-0ac4dda56978"));
filmPersistence.WritingStep();
voiceActorPersistence.WritingStep();
filmRatingPersistence.WritingStep();

Console.WriteLine(filmUniverse.GetAllFilms().Count);
var s = filmUniverse.BuildFilmList();
Console.WriteLine(s);
// var j = filmUniverse.BuildVoiceActorList();
// Console.WriteLine(j);
