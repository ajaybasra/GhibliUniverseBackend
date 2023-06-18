// See https://aka.ms/new-console-template for more information

using GhibliUniverse.Console;
using GhibliUniverse.Core;
using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Repository;
using GhibliUniverse.Core.Services;
using GhibliUniverse.Core.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDbContext<GhibliUniverseContext>(options =>
{
    options.UseNpgsql(Configuration.GetDbConnectionString());
});
var serviceProvider = services.BuildServiceProvider();

var commandLine = new CommandLine();
var consoleWriter = new ConsoleWriter();
var fileOperations = new FileOperations();
var filmPersistence = new FilmPersistence(fileOperations);
var reviewPersistence = new ReviewPersistence(fileOperations);
var voiceActorPersistence = new VoiceActorPersistence(fileOperations);
var filmVoiceActorPersistence = new FilmVoiceActorPersistence(fileOperations, filmPersistence, voiceActorPersistence);
var voiceActorService = new VoiceActorService(voiceActorPersistence);
var filmService = new FilmService(filmPersistence, reviewPersistence, voiceActorPersistence, filmVoiceActorPersistence);
var reviewService = new ReviewService(reviewPersistence);
var argumentProcessor = new ArgumentProcessor(commandLine, consoleWriter, filmService, reviewService, voiceActorService);

// var f = new Film
// {
//     Title = ValidatedString.From("aetman"),
//     Description = ValidatedString.From("Bruce"),
//     Director = ValidatedString.From("Chris"),
//     Composer = ValidatedString.From("Hans"),
//     ReleaseYear = ReleaseYear.From(2009)
//
// };
// // filmService.UpdateFilm(Guid.Parse("d0d540cf-404f-4688-8a58-57396523a424"), f);
// // argumentProcessor.Process();
// var va = voiceActorService.GetVoiceActorById(Guid.Parse("69b23314-3866-4b42-bc6f-392a4af190a1"));
// var vb = voiceActorService.GetVoiceActorById(Guid.Parse("e200aaf0-8655-4d22-96e4-21b16da714d4"));
// filmService.UnlinkVoiceActor(Guid.Parse("00000000-0000-0000-0000-000000000000"),vb.Id);
// filmService.LinkVoiceActor(Guid.Parse("00000000-0000-0000-0000-000000000000"),Guid.Parse("e200aaf0-8655-4d22-96e4-21b16da714d4"));
// filmService.DeleteFilm(Guid.Parse("d5e9fe11-e973-47e7-822c-ff24d53c4b89"));
// filmService.LinkVoiceActor();
var g = filmService.BuildFilmList();
Console.WriteLine(g);
var y = voiceActorService.BuildVoiceActorList();
Console.WriteLine(y);
var x = reviewService.BuildReviewList();
Console.WriteLine(x);
var root = Directory.GetCurrentDirectory();
consoleWriter.WriteLine(Configuration.GetDbConnectionString());