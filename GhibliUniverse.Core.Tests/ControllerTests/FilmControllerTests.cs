using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.API.Mapper;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.Models.Exceptions;
using GhibliUniverse.Core.Domain.ValueObjects;
using GhibliUniverse.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GhibliUniverse.Core.Tests.ControllerTests;

public class FilmControllerTests
{
    private readonly Mock<IFilmService> _mockedFilmService = new();
    private readonly MappingProfiles _mappingProfiles;
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly Mapper _mapper;

    private static readonly VoiceActor VoiceActor = new()
    {
        Id = Guid.Empty,
        Name = ValidatedString.From("John Doe")
    };
    private readonly Film _film1 = new()
    {
        Id = Guid.Empty, Title = ValidatedString.From("Spirited Away"),
        Description =
            ValidatedString.From(
                "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts."),
        Director = ValidatedString.From("Hayao Miyazaki"), 
        Composer = ValidatedString.From("Joe Hisaishi"),
        ReleaseYear = ReleaseYear.From(2001),
        VoiceActors = new List<VoiceActor> {VoiceActor}
    };

    private readonly Film _film2 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Title = ValidatedString.From("My Neighbor Totoro"),
        Description =
            ValidatedString.From(
                "Mei and Satsuki shift to a new house to be closer to their mother who is in the hospital. They soon become friends with Totoro, a giant rabbit-like creature who is a spirit."),
        Director = ValidatedString.From("Hayao Miyazaki"), 
        Composer = ValidatedString.From("Joe Hisaishi"),
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
                Id = Guid.Empty, Title = ValidatedString.From("Spirited Away"),
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
        
        var result = filmController.GetAllFilms() as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    private void GetFilmById_ReturnsFilmResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        _mockedFilmService.Setup(x => x.GetFilmById(It.IsAny<Guid>())).Returns(_film1);
        var expected = new FilmResponseDTO()
        {
            Id = Guid.Empty,
            Title = ValidatedString.From("Spirited Away"),
            Description = ValidatedString.From(
                "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts."),
            Director = ValidatedString.From("Hayao Miyazaki"),
            Composer = ValidatedString.From("Joe Hisaishi"),
            ReleaseYear = ReleaseYear.From(2001)
        };

        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result = filmController.GetFilmById(Guid.Empty) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void GetFilmById_Returns404StatusCodeWithGuid_WhenGivenNonExistentId()
    {
        _mockedFilmService.Setup(x => x.GetFilmById(It.IsAny<Guid>())).Throws(new ModelNotFoundException(Guid.Parse("04000000-0000-0000-0000-000000000001")));
        var expected = "No film found with the following id: 04000000-0000-0000-0000-000000000001";
        
        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result = filmController.GetFilmById(Guid.Parse("04000000-0000-0000-0000-000000000001")) as ObjectResult;
    
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void GetVoiceActorsByFilm_ReturnsListOfVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidId()  
    {
        _mockedFilmService.Setup(x => x.GetVoiceActorsByFilm(It.IsAny<Guid>())).Returns(_film1.VoiceActors);
        var voiceActor = new VoiceActorResponseDTO()
        {
            Id = Guid.Empty,
            Name = ValidatedString.From("John Doe")
        };
        var expectedVoiceActorResponseDTOList = new List<VoiceActorResponseDTO> { voiceActor };
        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result =
            filmController.GetVoiceActorsByFilm(Guid.Empty) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expectedVoiceActorResponseDTOList, result.Value);
    }
    
    [Fact]
    public void GetVoiceActorsByFilm_Returns404StatusCodeWithGuide_WhenGivenNonExistentId()  
    {
        _mockedFilmService.Setup(x => x.GetVoiceActorsByFilm(It.IsAny<Guid>())).Throws(new ModelNotFoundException(Guid.Empty));
        var expected = "No film found with the following id: 00000000-0000-0000-0000-000000000000";

        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result =
            filmController.GetVoiceActorsByFilm(Guid.Empty) as ObjectResult;
        
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    

    [Fact]
    public void CreateFilm_ReturnsFilmResponseDTOAnd200StatusCode_WhenGivenValidInput()
    {
        _mockedFilmService.Setup(x => x.CreateFilm(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<int>())).Returns(_film1);
        var expected = new FilmResponseDTO()
        {
            Id = Guid.Empty,
            Title = ValidatedString.From("Spirited Away"),
            Description = ValidatedString.From(
                "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts."),
            Director = ValidatedString.From("Hayao Miyazaki"),
            Composer = ValidatedString.From("Joe Hisaishi"),
            ReleaseYear = ReleaseYear.From(2001)
        };
        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);

        FilmRequestDTO filmRequestDto = new FilmRequestDTO()
            { Title = "test", Description = "test", Director = "test", Composer = "test", ReleaseYear = 2000 };
        var result = filmController.CreateFilm(filmRequestDto) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expected, result.Value);
        
    }

    [Fact]
    public void CreateFilm_Returns400StatusCode_WhenGivenInvalidInput()
    {
        _mockedFilmService.Setup(x => x.CreateFilm(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<int>())).Throws(new ArgumentException());
        var expected = "Value does not fall within the expected range.";
        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var filmRequestDTO = new FilmRequestDTO()
        {
            Title = "",
            Description = "",
            Director = "",
            Composer = "",
            ReleaseYear = 2000
        };
        var result = filmController.CreateFilm(filmRequestDTO) as ObjectResult;
        
        Assert.Equal(400, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void LinkVoiceActor_Returns200StatusCode_WhenGivenValidId()
    {
        var expected = "Successfully linked voice actor";                                                                           
        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result = filmController.LinkVoiceActor(Guid.Empty,
            Guid.Empty) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public void LinkVoiceActor_Returns404StatusCode_WhenGivenInvalidId()
    {
        var expected = "The model you are trying to perform an operation on does not exist. Model Id: 00000000-0000-0000-0000-000000000000";
        _mockedFilmService.Setup(x => x.LinkVoiceActor(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Throws(new ModelNotFoundException(It.IsAny<Guid>()));
        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result = filmController.LinkVoiceActor(Guid.Empty,
            Guid.Empty) as ObjectResult;
        
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public void UnlinkVoiceActor_Returns200StatusCode_WhenGivenValidId()
    {
        var expected = "Successfully unlinked voice actor";
        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result = filmController.UnlinkVoiceActor(Guid.Empty,
            Guid.Empty) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public void UnlinkVoiceActor_Returns404StatusCode_WhenGivenInvalidId()
    {
        var expected = "The model you are trying to perform an operation on does not exist. Model Id: 00000000-0000-0000-0000-000000000000";
        _mockedFilmService.Setup(x => x.UnlinkVoiceActor(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Throws(new ModelNotFoundException(It.IsAny<Guid>()));
        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result = filmController.UnlinkVoiceActor(Guid.Empty,
            Guid.Empty) as ObjectResult;
        
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public void UpdateFilm_ReturnsFilmResponseDTOAnd200StatusCode_WhenGivenValidInput()
    {
        _mockedFilmService.Setup(x => x.UpdateFilm(It.IsAny<Guid>(), It.IsAny<Film>())).Returns(_film1);
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var expected = new FilmResponseDTO()
        {
            Id = Guid.Empty,
            Title = ValidatedString.From("Spirited Away"),
            Description = ValidatedString.From(
                "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches and spirits, a world where humans are changed into beasts."),
            Director = ValidatedString.From("Hayao Miyazaki"),
            Composer = ValidatedString.From("Joe Hisaishi"),
            ReleaseYear = ReleaseYear.From(2001)
        };

        FilmRequestDTO filmRequestDto = new FilmRequestDTO()
            { Title = "test", Description = "test", Director = "test", Composer = "test", ReleaseYear = 2000 };
        var result = filmController.UpdateFilm(Guid.Empty, filmRequestDto) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expected, result.Value);
        
    }
    
    [Fact]
    public void UpdateFilm_Returns400StatusCode_WhenGivenInvalidInput()
    {
        _mockedFilmService.Setup(x => x.UpdateFilm(It.IsAny<Guid>(),It.IsAny<Film>())).Throws(new ArgumentException());
        var expected = "Value cannot be null or empty";
        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var filmRequestDTO = new FilmRequestDTO()
        {
            Title = "",
            Description = "",
            Director = "",
            Composer = "",
            ReleaseYear = 2000
        };
        var result = filmController.UpdateFilm(Guid.Empty, filmRequestDTO) as ObjectResult;
        
        Assert.Equal(400, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void DeleteFilm_Returns200StatusCode_WhenGivenValidId()
    {
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result = filmController.DeleteFilm(Guid.Empty) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void DeleteFilm_Returns404StatusCode_WhenGivenNonExistentId()
    {
        _mockedFilmService.Setup(x => x.DeleteFilm(It.IsAny<Guid>()))
            .Throws(new ModelNotFoundException(It.IsAny<Guid>()));
        var expected = "No film found with the following id: 00000000-0000-0000-0000-000000000000";
        
        var filmController = ControllerFactory.GenerateFilmController(_mockedFilmService.Object, _mapper);
        var result = filmController.DeleteFilm(Guid.Empty) as ObjectResult;
        
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expected, result.Value);
        
    }
}