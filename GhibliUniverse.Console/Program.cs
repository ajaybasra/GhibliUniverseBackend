// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using GhibliUniverse.Console;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Services;

var commandLine = new CommandLine();
var fileOperations = new FileOperations();
var filmPersistence = new FilmPersistence(fileOperations);
var reviewPersistence = new ReviewPersistence(fileOperations);
var voiceActorPersistence = new VoiceActorPersistence(fileOperations);
var filmVoiceActorPersistence = new FilmVoiceActorPersistence(fileOperations, filmPersistence, voiceActorPersistence);
var voiceActorService = new VoiceActorService(voiceActorPersistence);
var filmService = new FilmService(filmPersistence, reviewPersistence, voiceActorPersistence, filmVoiceActorPersistence);
var reviewService = new ReviewService(reviewPersistence);
var argumentProcessor = new ArgumentProcessor(commandLine, filmService, reviewService, voiceActorService);

// argumentProcessor.Process();

// var va = voiceActorService.GetVoiceActorById(Guid.Parse("69b23314-3866-4b42-bc6f-392a4af190a1"));
// filmService.AddVoiceActor(Guid.Parse("00000000-0000-0000-0000-000000000000"),va);
// filmService.AddVoiceActor(Guid.Parse("d5e9fe11-e973-47e7-822c-ff24d53c4b89"),va);


var g = filmService.BuildFilmList();
Console.WriteLine(g);
var y = voiceActorService.BuildVoiceActorList();
Console.WriteLine(y);
var x = reviewService.BuildReviewList();
Console.WriteLine(x);
