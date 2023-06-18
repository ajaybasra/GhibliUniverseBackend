// using AutoMapper;
// using GhibliUniverse.API.DTOs;
// using GhibliUniverse.API.Mapper;
// using GhibliUniverse.Core.Domain.Models;
// using GhibliUniverse.Core.Domain.Models.Exceptions;
// using GhibliUniverse.Core.Domain.ValueObjects;
// using GhibliUniverse.Core.Services;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
//
// namespace GhibliUniverse.Core.Tests.ControllerTests;
//
// public class VoiceActorControllerTests
// {
//     private readonly Mock<IVoiceActorService> _mockedVoiceActorService = new();
//     private readonly MappingProfiles _mappingProfiles;
//     private readonly MapperConfiguration _mapperConfiguration;
//     private readonly Mapper _mapper;
//
//     private static readonly Film Film = new Film()
//     {
//         Id = Guid.Empty,
//         Title = ValidatedString.From("Test"),
//         Description = ValidatedString.From("Test"),
//         Director = ValidatedString.From("Test"),
//         Composer = ValidatedString.From("Test"),
//         ReleaseYear = ReleaseYear.From(2000)
//     };
//     
//     private readonly VoiceActor _voiceActor1 = new()
//     {
//         Id = Guid.Empty,
//         Name = ValidatedString.From("John Doe"),
//         Films = new List<Film> {Film}
//     };
//
//     private readonly VoiceActor _voiceActor2 = new()
//     {
//         Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
//         Name = ValidatedString.From("John Cena")
//     };
//
//     private readonly List<VoiceActor> _voiceActors;
//
//     public VoiceActorControllerTests()
//     {
//         _mappingProfiles = new MappingProfiles();
//         _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(_mappingProfiles));
//         _mapper = new Mapper(_mapperConfiguration);
//         _voiceActors = new List<VoiceActor> { _voiceActor1, _voiceActor2 };
//     }
//     
//     [Fact]
//     private void GetAllVoiceActors_ReturnsListOfVoiceActorResponseDTOAnd200StatusCode_WhenCalled()
//     {
//         _mockedVoiceActorService.Setup(x => x.GetAllVoiceActors()).Returns(_voiceActors);
//         var voiceActorController = ControllerFactory.GenerateVoiceActorController(_mockedVoiceActorService.Object, _mapper);
//         var expected = new List<VoiceActorResponseDTO>()
//         {
//             new()
//             {
//                 Id = Guid.Empty,
//                 Name = ValidatedString.From("John Doe")
//             },
//             new()
//             {
//                 Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
//                 Name = ValidatedString.From("John Cena")
//             }
//         };
//         
//         var result = voiceActorController.GetAllVoiceActors() as ObjectResult;
//         
//         Assert.Equal(200, result.StatusCode);
//         Assert.Equal(expected, result.Value);
//     }
//     
//     [Fact]
//     private void GetVoiceActorById_ReturnsVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidId()
//     {
//         _mockedVoiceActorService.Setup(x => x.GetVoiceActorById(It.IsAny<Guid>())).Returns(_voiceActor1);
//         var voiceActorController = ControllerFactory.GenerateVoiceActorController(_mockedVoiceActorService.Object, _mapper);
//         var expected = new VoiceActorResponseDTO()
//         {
//             Id = Guid.Empty,
//             Name = ValidatedString.From("John Doe") 
//         };
//
//         var result = voiceActorController.GetVoiceActorById(Guid.Empty) as ObjectResult;
//         
//         Assert.Equal(200, result.StatusCode);
//         Assert.Equal(expected, result.Value);
//     }
//     
//     [Fact]
//     public void GetVoiceActorById_Returns404StatusCodeWithGuid_WhenGivenNonExistentId()
//     {
//         _mockedVoiceActorService.Setup(x => x.GetVoiceActorById(It.IsAny<Guid>())).Throws(new ModelNotFoundException(Guid.Parse("04000000-0000-0000-0000-000000000001")));
//         var voiceActorController = ControllerFactory.GenerateVoiceActorController(_mockedVoiceActorService.Object, _mapper);
//         var expected = "No voice actor found with the following id: 04000000-0000-0000-0000-000000000001";
//         
//         
//         var result = voiceActorController.GetVoiceActorById(Guid.Parse("04000000-0000-0000-0000-000000000001")) as ObjectResult;
//     
//         Assert.Equal(404, result.StatusCode);
//         Assert.Equal(expected, result.Value);
//     }
//     
//     [Fact]
//     public void GetFilmsByVoiceActor_ReturnsListOfFilmResponseDTOAnd200StatusCode_WhenGivenValidId()  
//     {
//         _mockedVoiceActorService.Setup(x => x.GetFilmsByVoiceActor(It.IsAny<Guid>())).Returns(_voiceActor1.Films);
//         var voiceActorController = ControllerFactory.GenerateVoiceActorController(_mockedVoiceActorService.Object, _mapper);
//         var film = new FilmResponseDTO()
//         {
//             Id = Guid.Empty,
//             Title = ValidatedString.From("Test"),
//             Description = ValidatedString.From("Test"),
//             Director = ValidatedString.From("Test"),
//             Composer = ValidatedString.From("Test"),
//             ReleaseYear = ReleaseYear.From(2000)
//         };
//         var expected = new List<FilmResponseDTO> { film };
//         
//         var result =
//             voiceActorController.GetFilmsByVoiceActor(Guid.Empty) as ObjectResult;
//         
//         Assert.Equal(200, result.StatusCode);
//         Assert.Equal(expected, result.Value);
//     }
//     
//     [Fact]
//     public void GetFilmsByVoiceActor_Returns404StatusCodeWithGuid_WhenGivenNonExistentId()  
//     {
//         _mockedVoiceActorService.Setup(x => x.GetFilmsByVoiceActor(It.IsAny<Guid>())).Throws(new ModelNotFoundException(Guid.Empty));
//         var voiceActorController = ControllerFactory.GenerateVoiceActorController(_mockedVoiceActorService.Object, _mapper);
//         var expected = "No voice actor found with the following id: 00000000-0000-0000-0000-000000000000";
//         
//         var result = voiceActorController.GetFilmsByVoiceActor(Guid.Empty) as ObjectResult;
//         
//         Assert.Equal(404, result.StatusCode);
//         Assert.Equal(expected, result.Value);
//     }
//
//     [Fact]
//     public void CreateVoiceActor_ReturnsVoiceActorResponseDTOAndStatusCode_WhenGivenValidInput()
//     {
//         _mockedVoiceActorService.Setup(x => x.CreateVoiceActor(It.IsAny<string>())).Returns(_voiceActor1);
//         var voiceActorController = ControllerFactory.GenerateVoiceActorController(_mockedVoiceActorService.Object, _mapper);
//         var expected = new VoiceActorResponseDTO()
//         {
//             Id = Guid.Empty,
//             Name = ValidatedString.From("John Doe")
//         };
//         var voiceActorRequestDTO = new VoiceActorRequestDTO()
//         {
//             Name = "John Doe"
//         };
//         
//         var result = voiceActorController.CreateVoiceActor(voiceActorRequestDTO) as ObjectResult;
//         
//         Assert.Equal(200, result.StatusCode);
//         Assert.Equal(expected, result.Value);
//     }
//     
//     [Fact]
//     public void CreateVoiceActor_Returns400StatusCode_WhenGivenInvalidInput()
//     {
//         _mockedVoiceActorService.Setup(x => x.CreateVoiceActor(It.IsAny<string>())).Throws(new ArgumentException());
//         var voiceActorController = ControllerFactory.GenerateVoiceActorController(_mockedVoiceActorService.Object, _mapper);
//         var voiceActorRequestDTO = new VoiceActorRequestDTO()
//         {
//             Name = "",
//         };
//         
//         var result = voiceActorController.CreateVoiceActor(voiceActorRequestDTO) as ObjectResult;
//         
//         Assert.Equal(400, result.StatusCode);
//     }
//     
//     [Fact]
//     public void UpdateVoiceActor_ReturnsVoiceActorResponseDTOAnd200StatusCode_WhenGivenValidInput()
//     {
//         _mockedVoiceActorService.Setup(x => x.UpdateVoiceActor(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_voiceActor1);
//         var voiceActorController = ControllerFactory.GenerateVoiceActorController(_mockedVoiceActorService.Object, _mapper);
//         var expected = new VoiceActorResponseDTO()
//         {
//             Id = Guid.Empty,
//             Name = ValidatedString.From("John Doe")
//         };
//         var voiceActorRequestDto = new VoiceActorRequestDTO()
//         {
//             Name = "John Doe"
//         };
//         
//         var result = voiceActorController.UpdateVoiceActor(Guid.Empty, voiceActorRequestDto) as ObjectResult;
//         
//         Assert.Equal(200, result.StatusCode);
//         Assert.Equal(expected, result.Value);
//         
//     }
//     
//     [Fact]
//     public void UpdateVoiceActor_Returns400StatusCode_WhenGivenInvalidInput()
//     {
//         _mockedVoiceActorService.Setup(x => x.UpdateVoiceActor(It.IsAny<Guid>(), It.IsAny<string>())).Throws(new ArgumentException());
//         var voiceActorController = ControllerFactory.GenerateVoiceActorController(_mockedVoiceActorService.Object, _mapper);
//         var voiceActorRequestDto = new VoiceActorRequestDTO()
//         {
//             Name = ""
//         };
//         
//         var result = voiceActorController.UpdateVoiceActor(Guid.Empty, voiceActorRequestDto) as ObjectResult;
//         
//         Assert.Equal(400, result.StatusCode);
//         
//     }
//     
//     [Fact]
//     public void DeleteVoiceActor_Returns200StatusCode_WhenGivenValidId()
//     {
//         var voiceActorController = ControllerFactory.GenerateVoiceActorController(_mockedVoiceActorService.Object, _mapper);
//         
//         var result = voiceActorController.DeleteVoiceActor(Guid.Empty) as ObjectResult;
//         
//         Assert.Equal(200, result.StatusCode);
//     }
//     
//     [Fact]
//     public void DeleteVoiceActor_Returns404StatusCode_WhenGivenNonExistentId()
//     {
//         _mockedVoiceActorService.Setup(x => x.DeleteVoiceActor(It.IsAny<Guid>()))
//             .Throws(new ModelNotFoundException(It.IsAny<Guid>()));
//         var voiceActorController = ControllerFactory.GenerateVoiceActorController(_mockedVoiceActorService.Object, _mapper);
//         var expected = "No voice actor found with the following id: 00000000-0000-0000-0000-000000000000";
//
//         var result = voiceActorController.DeleteVoiceActor(Guid.Empty) as ObjectResult;
//         
//         Assert.Equal(404, result.StatusCode);
//         Assert.Equal(expected, result.Value);
//     }
// }