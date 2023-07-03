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
services.AddScoped<IFilmRepository, FilmRepository>();
services.AddScoped<IVoiceActorRepository, VoiceActorRepository>();
services.AddScoped<IReviewRepository, ReviewRepository>();
services.AddScoped<IFilmService, FilmService>();
services.AddScoped<IVoiceActorService, VoiceActorService>();
services.AddScoped<IReviewService, ReviewService>();
services.AddScoped<ICommandLine, CommandLine>();
services.AddScoped<IWriter, ConsoleWriter>();

var serviceProvider = services.BuildServiceProvider();
using var scope = serviceProvider.CreateScope();

var filmService = scope.ServiceProvider.GetRequiredService<IFilmService>();
var voiceActorService = scope.ServiceProvider.GetRequiredService<IVoiceActorService>();
var reviewService = scope.ServiceProvider.GetRequiredService<IReviewService>();
var commandLine = scope.ServiceProvider.GetRequiredService<ICommandLine>();
var consoleWriter = scope.ServiceProvider.GetRequiredService<IWriter>();

var argumentProcessor = new ArgumentProcessor(commandLine, consoleWriter, filmService, reviewService, voiceActorService);
// reviewService.DeleteReview(Guid.Parse("1fbc6079-9f7d-417f-92a4-843d007edcb4"));
// reviewService.CreateReview(Guid.Parse("1af40df1-a634-445b-be1f-0b5e671d1c16"), 10);
// voiceActorService.CreateVoiceActor("John Doe");
// film.LinkVoiceActor(Guid.Parse("0af40df1-a634-445b-be1f-0b5e671d1c16"), Guid.Parse("6408242e-8358-40c2-b3ad-5cb66dc68f89"));
// film.DeleteFilm(Guid.Parse("ad25f890-3050-46f4-951f-786c563c8ff9"));
// film.CreateFilm("test", "test", "test", "test", 2002);
// Console.WriteLine(voice.GetFilmsByVoiceActor(Guid.Parse("151148f4-48fc-4729-86fa-8c3071cfa02d")).Count);
// Console.WriteLine(voice.GetFilmsByVoiceActor());
// voice.DeleteVoiceActor(Guid.Parse("151148f4-48fc-4729-86fa-8c3071cfa02d"));
// filmService.UnlinkVoiceActor(Guid.Parse("0af40df1-a634-445b-be1f-0b5e671d1c16"),Guid.Parse("6408242e-8358-40c2-b3ad-5cb66dc68f89"));
// filmService.UnlinkVoiceActor(Guid.Parse("0af40df1-a634-445b-be1f-0b5e671d1c16"),Guid.Parse("b9c73f57-5c61-4e70-9f14-d1eeed144ab6"));
// voiceActorService.DeleteVoiceActor(Guid.Parse("6a427101-4c08-4b64-a230-a7eeb1d32ec8"));

var g = await filmService.GetAllFilms();
Console.WriteLine(g[0]);
// var v = await voiceActorService.GetAllVoiceActors();
// Console.WriteLine(v[0]);
// var y = voiceActorService.BuildVoiceActorList();
// // Console.WriteLine(y);
// var x = reviewService.BuildReviewList();
// // Console.WriteLine(x);
// var root = Directory.GetCurrentDirectory();
argumentProcessor.Process();
