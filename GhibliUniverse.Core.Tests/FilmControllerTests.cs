using AutoMapper;
using GhibliUniverse.API.Controllers;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.API.Mapper;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GhibliUniverse.Core.Tests;

public class FilmControllerTests
{
    private readonly Mock<IFilmService> _mockedFilmService = new Mock<IFilmService>();
    private readonly MappingProfiles _mappingProfiles;
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly Mapper _mapper;

    private readonly Film _film1 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Title = ValidatedString.From("Spirited Away"),
        Description =
            ValidatedString.From(
                "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts."),
        Director = ValidatedString.From("Hayao Miyazaki"), Composer = ValidatedString.From("Joe Hisaishi"),
        ReleaseYear = ReleaseYear.From(2001)
    };

    private readonly Film _film2 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Title = ValidatedString.From("My Neighbor Totoro"),
        Description =
            ValidatedString.From(
                "Mei and Satsuki shift to a new house to be closer to their mother who is in the hospital. They soon become friends with Totoro, a giant rabbit-like creature who is a spirit."),
        Director = ValidatedString.From("Hayao Miyazaki"), Composer = ValidatedString.From("Joe Hisaishi"),
        ReleaseYear = ReleaseYear.From(1988)
    };

    private readonly List<Film> _films;

    public FilmControllerTests()
    {
        _mappingProfiles = new MappingProfiles();
        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(_mappingProfiles));
        _mapper = new Mapper(_mapperConfiguration);
        _films = new List<Film>() { _film1, _film2 };
    }

    [Fact]
    private void GetAllFilms_ReturnsListOfFilmResponseDTOAnd200StatusCode_WhenCalled()
    {
        _mockedFilmService.Setup(x => x.GetAllFilms()).Returns(_films);
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var expected = new List<FilmResponseDTO>()
        {
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Title = ValidatedString.From("Spirited Away"),
                Description =
                    ValidatedString.From(
                        "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts."),
                Director = ValidatedString.From("Hayao Miyazaki"), Composer = ValidatedString.From("Joe Hisaishi"),
                ReleaseYear = ReleaseYear.From(2001)
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Title = ValidatedString.From("My Neighbor Totoro"),
                Description =
                    ValidatedString.From(
                        "Mei and Satsuki shift to a new house to be closer to their mother who is in the hospital. They soon become friends with Totoro, a giant rabbit-like creature who is a spirit."),
                Director = ValidatedString.From("Hayao Miyazaki"), Composer = ValidatedString.From("Joe Hisaishi"),
                ReleaseYear = ReleaseYear.From(1988)
            }
        };
        
        var result = filmController.GetAllFilms() as OkObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        // Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    private void GetFilmById_ReturnsFilmResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        _mockedFilmService.Setup(x => x.GetFilmById(It.IsAny<Guid>())).Returns(_film1);

        var expected = new FilmResponseDTO()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            Title = ValidatedString.From("Spirited Away"),
            Description = ValidatedString.From(
                "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts."),
            Director = ValidatedString.From("Hayao Miyazaki"),
            Composer = ValidatedString.From("Joe Hisaishi"),
            ReleaseYear = ReleaseYear.From(2001)
        };

        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result = filmController.GetFilmById(Guid.Parse("00000000-0000-0000-0000-000000000001")) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void GetFilmById_Returns404CodeWithGuid_WhenGivenNonExistentId()
    {
        _mockedFilmService.Setup(x => x.GetFilmById(It.IsAny<Guid>())).Throws(new ModelNotFoundException(Guid.Parse("04000000-0000-0000-0000-000000000001")));
        var expected = "No film found with the following id: 04000000-0000-0000-0000-000000000001";
        
        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result = filmController.GetFilmById(Guid.Parse("04000000-0000-0000-0000-000000000001")) as ObjectResult;
    
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void GetVoiceActorsByFilm_Returns200StatusCode_WhenGivenValidId() //tidy this up
    {
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result =
            filmController.GetVoiceActorsByFilm(Guid.NewGuid()) as OkObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void CreateFilm_Returns200StatusCode_WhenGivenValidInput()
    {
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);

        FilmRequestDTO filmRequestDto = new FilmRequestDTO()
            { Title = "test", Description = "test", Director = "test", Composer = "test", ReleaseYear = 2000 };
        var result = filmController.CreateFilm(filmRequestDto) as OkObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        
    }
}