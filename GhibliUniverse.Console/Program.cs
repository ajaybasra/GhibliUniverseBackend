// See https://aka.ms/new-console-template for more information

using GhibliUniverse;
using GhibliUniverse.DataPersistence;

var filmUniverse = new FilmUniverse();
var commandLine = new CommandLine();
var argumentProcessor = new ArgumentProcessor(commandLine, filmUniverse);
var fileOperations = new FileOperations();
var filmPersistence = new FilmPersistence(filmUniverse, fileOperations);
var voiceActorPersistence = new VoiceActorPersistence(filmUniverse, fileOperations);
var filmRatingPersistence = new FilmRatingPersistence(filmUniverse, fileOperations);
var filmVoiceActorPersistence = new FilmVoiceActorPersistence(filmUniverse, fileOperations);

filmPersistence.ReadingStep();
voiceActorPersistence.ReadingStep();
filmRatingPersistence.ReadingStep();
filmVoiceActorPersistence.ReadingStep();


// filmUniverse.PopulateFilmsList(1);
// argumentProcessor.Process();
// filmUniverse.PopulateFilmsList(2);
// filmUniverse.DeleteFilm(new Guid("00000000-0000-0000-0000-000000000000"));
// filmUniverse.DeleteFilm(new Guid("11111111-1111-1111-1111-111111111111"));
// filmUniverse.DeleteFilm(new Guid("14be95a1-a589-40ed-bd39-f018007fa274"));
// filmUniverse.CreateFilm("Batman Begins", "A man dresses up as; a bat, and fights crime, his name is, vengeance.", "Christopher Nolan", "Hans Zimmer", 1999);
// // filmUniverse.CreateVoiceActor("Henry", "Dot");
// filmUniverse.DeleteFilmRating(new Guid("00000000-0000-0000-0000-000000000000"), new Guid("62b20cb0-ae09-4156-a831-06065aa44e78"));
// filmUniverse.DeleteVoiceActor(new Guid("aeca3ca7-d5e2-43ac-ab91-77bf529000dc"));
// filmUniverse.DeleteVoiceActor(new Guid("c2f6fcbb-71e2-4379-b817-a4cb3582ea51"));
// filmUniverse.DeleteFilmRating(new Guid("00000000-0000-0000-0000-000000000000"), new Guid("1ddac2db-cb70-4fd8-b002-5dc21a96bbd5"));
filmPersistence.WritingStep();
voiceActorPersistence.WritingStep();
filmRatingPersistence.WritingStep();
filmVoiceActorPersistence.WritingStep();
// try
// {
//     filmUniverse.CreateFilm("", "A man dresses up as; a bat, and fights crime, his name is, vengeance.", "Christopher Nolan", "Hans Zimmer", 1999);
//
// }
// catch (ModelNotFoundException e)
// {
//     Console.WriteLine(e);
// }
var s = filmUniverse.BuildFilmList();
Console.WriteLine(s);

