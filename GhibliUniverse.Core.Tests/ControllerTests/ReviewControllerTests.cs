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

public class ReviewControllerTests
{
    private readonly Mock<IReviewService> _mockedReviewService = new();
    private readonly MappingProfiles _mappingProfiles;
    private readonly MapperConfiguration _mapperConfiguration;
    private readonly Mapper _mapper;

    private readonly Review _review1 = new()
    {
        Id = Guid.Empty,
        Rating = Rating.From(10),
        FilmId = Guid.Empty
    };
    
    
    private readonly Review _review2 = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Rating = Rating.From(9),
        FilmId = Guid.Parse("00000000-0000-0000-0000-000000000001")
    };
    
    private readonly List<Review> _reviews;

    public ReviewControllerTests()
    {
        _mappingProfiles = new MappingProfiles();
        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(_mappingProfiles));
        _mapper = new Mapper(_mapperConfiguration);
        _reviews = new List<Review>() { _review1, _review2 };
    }
    
    [Fact]
    private void GetAllReviews_ReturnsListOfReviewResponseDTOAnd200StatusCode_WhenCalled()
    {
        _mockedReviewService.Setup(x => x.GetAllReviews()).Returns(_reviews);
        var reviewController = ControllerFactory.GenerateReviewController(_mockedReviewService.Object, _mapper);
        var expected = new List<ReviewResponseDTO>()
        {
            new()
            {
                Id = Guid.Empty,
                Rating = Rating.From(10),
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Rating = Rating.From(9),
            }
        };
        
        var result = reviewController.GetAllReviews() as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    private void GetReviewById_ReturnsReviewResponseDTOAnd200StatusCode_WhenGivenValidId()
    {
        _mockedReviewService.Setup(x => x.GetReviewById(It.IsAny<Guid>())).Returns(_review1);
        var expected = new ReviewResponseDTO()
        {
            Id = Guid.Empty,
            Rating = Rating.From(10) 
        };

        var reviewController = ControllerFactory.GenerateReviewController(_mockedReviewService.Object, _mapper);
        var result = reviewController.GetReviewById(Guid.Empty) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public void GetFilmById_Returns404StatusCodeWithGuid_WhenGivenNonExistentId()
    {
        _mockedReviewService.Setup(x => x.GetReviewById(It.IsAny<Guid>())).Throws(new ModelNotFoundException(Guid.Parse("04000000-0000-0000-0000-000000000001")));
        var expected = "No review found with the following id: 04000000-0000-0000-0000-000000000001";
        
        
        var reviewController = ControllerFactory.GenerateReviewController(_mockedReviewService.Object, _mapper);
        var result = reviewController.GetReviewById(Guid.Parse("04000000-0000-0000-0000-000000000001")) as ObjectResult;
    
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public void CreateReview_ReturnsReviewResponseDTOAnd200StatusCode_WhenGivenValidInput()
    {
        _mockedReviewService.Setup(x => x.CreateReview(It.IsAny<Guid>(), It.IsAny<int>())).Returns(_review1);
        var expected = new ReviewResponseDTO()
        {
            Id = Guid.Empty,
            Rating = Rating.From(10)
        };
        
        var reviewController = ControllerFactory.GenerateReviewController(_mockedReviewService.Object, _mapper);

        ReviewRequestDTO reviewRequestDto = new ReviewRequestDTO()
            { rating = 10 };
        var result = reviewController.CreateReview(Guid.Empty, reviewRequestDto) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expected, result.Value);
        
    }
    
    [Fact]
    public void CreateReview_Returns404StatusCode_WhenGivenNonExistentFilmId()
    {
        _mockedReviewService.Setup(x => x.CreateReview(It.IsAny<Guid>(), It.IsAny<int>())).Throws(new ModelNotFoundException(It.IsAny<Guid>()));
        var expected = "No film found with the following id: 00000000-0000-0000-0000-000000000000";
        
        var reviewController = ControllerFactory.GenerateReviewController(_mockedReviewService.Object, _mapper);
        var reviewRequestDTO = new ReviewRequestDTO()
        {
            rating = 10,
        };
        var result = reviewController.CreateReview(Guid.Empty, reviewRequestDTO) as ObjectResult;
        
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public void UpdateReview_ReturnsReviewResponseDTOAnd200StatusCode_WhenGivenValidInput()
    {
        _mockedReviewService.Setup(x => x.UpdateReview(It.IsAny<Guid>(), It.IsAny<int>())).Returns(_review1);
        
        var reviewController = ControllerFactory.GenerateReviewController(_mockedReviewService.Object, _mapper);
        var expected = new ReviewResponseDTO()
        {
            Rating = Rating.From(10)
        };

        ReviewRequestDTO reviewRequestDto = new ReviewRequestDTO()
        {
            rating = 10
        };
        var result = reviewController.UpdateReview(Guid.Empty, reviewRequestDto) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expected, result.Value);
        
    }
    
    [Fact]
    public void UpdateReview_Returns400StatusCode_WhenGivenInvalidInput()
    {
        _mockedReviewService.Setup(x => x.UpdateReview(It.IsAny<Guid>(), It.IsAny<int>())).Throws(new Rating.RatingOutOfRangeException(It.IsAny<int>()));
        var expected = "Rating must be between 1 and 10 inclusive. Current rating: 0.";

        var reviewController = ControllerFactory.GenerateReviewController(_mockedReviewService.Object, _mapper);

        ReviewRequestDTO reviewRequestDto = new ReviewRequestDTO()
        {
            rating = 0
        };
        var result = reviewController.UpdateReview(Guid.Empty, reviewRequestDto) as ObjectResult;
        
        Assert.Equal(400, result.StatusCode);
        Assert.Equal(expected, result.Value);
        
    }
    
    [Fact]
    public void DeleteReview_Returns200StatusCode_WhenGivenValidId()
    {
        var reviewController = ControllerFactory.GenerateReviewController(_mockedReviewService.Object, _mapper);
        var result = reviewController.DeleteReview(Guid.Empty) as ObjectResult;
        
        Assert.Equal(200, result.StatusCode);
    }
    
    [Fact]
    public void DeleteReview_Returns404StatusCode_WhenGivenNonExistentId()
    {
        _mockedReviewService.Setup(x => x.DeleteReview(It.IsAny<Guid>()))
            .Throws(new ModelNotFoundException(It.IsAny<Guid>()));
        var expected = "No review found with the following id: 00000000-0000-0000-0000-000000000000";
        
        var reviewController = ControllerFactory.GenerateReviewController(_mockedReviewService.Object, _mapper);
        var result = reviewController.DeleteReview(Guid.Empty) as ObjectResult;
        
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expected, result.Value);
    }
}