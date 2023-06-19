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
using var scope = serviceProvider.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<GhibliUniverseContext>();
var filmRepository = new FilmRepository(context);
var voiceActorRepository = new VoiceActorRepository(context);
var reviewRepository = new ReviewRepository(context);
var filmService = new FilmService(filmRepository);
var voiceActorService = new VoiceActorService(voiceActorRepository);
var reviewService = new ReviewService(reviewRepository);
// review.CreateReview(Guid.Parse("ad25f890-3050-46f4-951f-786c563c8ff9"), 10);
// voice.CreateVoiceActor("John Cena");
// film.LinkVoiceActor(Guid.Parse("0af40df1-a634-445b-be1f-0b5e671d1c16"), Guid.Parse("6408242e-8358-40c2-b3ad-5cb66dc68f89"));
// film.DeleteFilm(Guid.Parse("ad25f890-3050-46f4-951f-786c563c8ff9"));
// film.CreateFilm("test", "test", "test", "test", 2002);
// Console.WriteLine(voice.GetFilmsByVoiceActor(Guid.Parse("151148f4-48fc-4729-86fa-8c3071cfa02d")).Count);
// Console.WriteLine(voice.GetFilmsByVoiceActor());
// voice.DeleteVoiceActor(Guid.Parse("151148f4-48fc-4729-86fa-8c3071cfa02d"));
Console.WriteLine(filmService.GetAllFilms()[0]);

var commandLine = new CommandLine();
var consoleWriter = new ConsoleWriter();
var fileOperations = new FileOperations();
var filmPersistence = new FilmPersistence(fileOperations);
var reviewPersistence = new ReviewPersistence(fileOperations);
var voiceActorPersistence = new VoiceActorPersistence(fileOperations);
var filmVoiceActorPersistence = new FilmVoiceActorPersistence(fileOperations, filmPersistence, voiceActorPersistence);
// var filmService = new FilmService(filmPersistence, reviewPersistence, voiceActorPersistence, filmVoiceActorPersistence);
var argumentProcessor = new ArgumentProcessor(commandLine, consoleWriter, filmService, reviewService, voiceActorService);
argumentProcessor.Process();
var g = filmService.BuildFilmList();
Console.WriteLine(g);
var y = voiceActorService.BuildVoiceActorList();
// Console.WriteLine(y);
var x = reviewService.BuildReviewList();
// Console.WriteLine(x);
var root = Directory.GetCurrentDirectory();
