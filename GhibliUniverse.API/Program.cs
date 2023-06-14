using GhibliUniverse.Core.Context;
using GhibliUniverse.Core.DataPersistence;
using GhibliUniverse.Core.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// builder.Services.AddDbContext<GhibliUniverseContext>(options =>
// {
//     options.UseNpgsql(Configuration.GetDbConnectionString());
//
// });
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IFilmService, FilmService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IVoiceActorService, VoiceActorService>();
builder.Services.AddScoped<IFileOperations, FileOperations>();
builder.Services.AddScoped<IFilmPersistence, FilmPersistence>();
builder.Services.AddScoped<IReviewPersistence, ReviewPersistence>();
builder.Services.AddScoped<IVoiceActorPersistence, VoiceActorPersistence>();
builder.Services.AddScoped<IFilmVoiceActorPersistence, FilmVoiceActorPersistence>();
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