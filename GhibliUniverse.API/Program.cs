using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Repository;
using GhibliUniverse.Core.Services;
using GhibliUniverse.Core.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// var root = Directory.GetCurrentDirectory();
// var localEnvironmentFilePath = Path.Combine(root, ".env.local");
// if (File.Exists(localEnvironmentFilePath))
// {
//     DotEnv.Load(localEnvironmentFilePath);
// }

builder.Services.AddControllers();
builder.Services.AddDbContext<GhibliUniverseContext>(options =>
{
    options.UseNpgsql(Configuration.GetDbConnectionString());

});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IFilmService, FilmService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IVoiceActorService, VoiceActorService>();
builder.Services.AddScoped<IFilmRepository, FilmRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IVoiceActorRepository, VoiceActorRepository>();
// builder.Services.AddScoped<IFilmRepository, FilmRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public abstract partial class Program
{
}