// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using GhibliUniverse.Console;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Services;

var filmUniverse = new FilmUniverse();
var commandLine = new CommandLine();
var argumentProcessor = new ArgumentProcessor(commandLine, filmUniverse);
var fileOperations = new FileOperations();
var filmPersistence = new FilmPersistence(fileOperations);
var reviewPersistence = new ReviewPersistence(fileOperations);
var voiceActorPersistence = new VoiceActorPersistence(fileOperations);
var filmVoiceActorPersistence = new FilmVoiceActorPersistence(fileOperations, filmPersistence, voiceActorPersistence);
var voiceActorService = new VoiceActorService(voiceActorPersistence);
var filmService = new FilmService(filmPersistence, reviewPersistence, voiceActorPersistence, filmVoiceActorPersistence);
var reviewService = new ReviewService(filmService, reviewPersistence);
// filmPersistence.ReadingStep();
// filmService.PopulateFilmsList(1);
// voiceActorService.PopulateVoiceActorsList(1);
// reviewService.CreateReview(Guid.Parse("00000000-0000-0000-0000-000000000000"), 10);
// voiceActorService.DeleteVoiceActor(Guid.Parse("00000000-0000-0000-0000-000000000000"));
// filmService.DeleteFilm(Guid.Parse("fd6a421e-aaa1-4dbb-8283-125d4fe3413e"));
// filmService.DeleteFilm(Guid.Parse("11111111-1111-1111-1111-111111111111"));
// voiceActorPersistence.ReadingStep();
// voiceActorService.DeleteVoiceActor(Guid.Parse("e887ab07-2370-44aa-905e-74e4b685f4f6"));
// voiceActorService.CreateVoiceActor("John Swag");
// filmService.AddVoiceActor(Guid.Parse("00000000-0000-0000-0000-000000000000"), voiceActorService.GetAllVoiceActors()[0]);
// voiceActorService.DeleteVoiceActor(Guid.rParse("00000000-0000-0000-0000-000000000000"));
// var mov = filmService.GetAllFilms();
// reviewService.CreateReview(Guid.Parse("00000000-0000-0000-0000-000000000000"), 7);
// filmService.DeleteFilm(Guid.Parse("00000000-0000-0000-0000-000000000000"));
// reviewService.DeleteReview(Guid.Parse("856ef3fb-7ff6-4174-9e33-9d5afe0c2efb"));
// filmVoiceActorPersistence.ReadingStep();
// // argumentProcessor.Process();
// filmService.CreateFilm("Batman Begins", "A man dresses up as; a bat, and fights crime, his name is, vengeance.", "Christopher Nolan", "Hans Zimmer", 1999);
// voiceActorPersistence.WriteVoiceActors();
// // reviewPersistence.WriteVoiceActors();
// filmVoiceActorPersistence.WriteVoiceActors();

var g = filmService.BuildFilmList();
Console.WriteLine(g);
var y = voiceActorService.BuildVoiceActorList();
Console.WriteLine(y);
var x = reviewService.BuildReviewList();
Console.WriteLine(x);
