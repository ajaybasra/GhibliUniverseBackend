// See https://aka.ms/new-console-template for more information

using AutoMapper;
using GhibliUniverse.Console;
using GhibliUniverse.Core;
using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.Repository;
using GhibliUniverse.Core.Repository.MappingProfiles;
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
services.AddAutoMapper(typeof(MappingProfiles));

var serviceProvider = services.BuildServiceProvider();
using var scope = serviceProvider.CreateScope();

var filmService = scope.ServiceProvider.GetRequiredService<IFilmService>();
var voiceActorService = scope.ServiceProvider.GetRequiredService<IVoiceActorService>();
var reviewService = scope.ServiceProvider.GetRequiredService<IReviewService>();
var commandLine = scope.ServiceProvider.GetRequiredService<ICommandLine>();
var consoleWriter = scope.ServiceProvider.GetRequiredService<IWriter>();

var argumentProcessor = new ArgumentProcessor(commandLine, consoleWriter, filmService, reviewService, voiceActorService);

var g = await filmService.GetAllFilms();
// consoleWriter.WriteLine(g[0]);