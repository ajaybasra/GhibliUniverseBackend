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
var filmVoiceActorPersistence = new FilmVoiceActorPersistence(filmUniverse);

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

if (filmVoiceActorPersistence.FileExists())
{
    filmVoiceActorPersistence.ReadingStep();
}

// filmUniverse.PopulateFilmsList(1);
// argumentProcessor.Process();
// filmUniverse.PopulateFilmsList(2);
// filmUniverse.DeleteFilm(new Guid("00000000-0000-0000-0000-000000000000"));
// filmUniverse.DeleteFilm(new Guid("14be95a1-a589-40ed-bd39-f018007fa274"));
// filmUniverse.CreateFilm("Batman Begins", "A man dresses up as a bat, and fights crime, his name is, vengeance.", "Christopher Nolan", "Hans Zimmer", 1929);
// // filmUniverse.CreateVoiceActor("Henry", "Dot");
// filmUniverse.DeleteFilmRating(new Guid("00000000-0000-0000-0000-000000000000"), new Guid("62b20cb0-ae09-4156-a831-06065aa44e78"));
// filmUniverse.DeleteVoiceActor(new Guid("31ba7e7e-f86c-46f7-b974-0ac4dda56978"));
// filmUniverse.DeleteVoiceActor(new Guid("c2f6fcbb-71e2-4379-b817-a4cb3582ea51"));
filmPersistence.WritingStep();
voiceActorPersistence.WritingStep();
filmRatingPersistence.WritingStep();
filmVoiceActorPersistence.WritingStep();
// filmUniverse.CreateFilmRating(11, new Guid("00000000-0000-0000-0000-000000000000"));
Console.WriteLine(filmUniverse.GetAllFilms().Count);
var s = filmUniverse.BuildFilmList();
Console.WriteLine(s);
