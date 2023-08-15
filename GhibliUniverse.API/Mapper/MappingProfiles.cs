using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.API.Mapper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Film, FilmResponseDTO>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.FilmInfo.Title.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.FilmInfo.Description.Value))
            .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.FilmInfo.Director.Value))
            .ForMember(dest => dest.Composer, opt => opt.MapFrom(src => src.FilmInfo.Composer.Value))
            .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.FilmInfo.ReleaseYear.Value));
        CreateMap<Film, FilmRequestDTO>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.FilmInfo.Title.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.FilmInfo.Description.Value))
            .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.FilmInfo.Director.Value))
            .ForMember(dest => dest.Composer, opt => opt.MapFrom(src => src.FilmInfo.Composer.Value))
            .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.FilmInfo.ReleaseYear.Value));
        CreateMap<FilmResponseDTO, Film>();
        CreateMap<FilmRequestDTO, Film>()
            .ForMember(dest => dest.FilmInfo.Title, opt => opt.MapFrom(src => ValidatedString.From(src.Title)))
            .ForMember(dest => dest.FilmInfo.Description, opt => opt.MapFrom(src => ValidatedString.From(src.Description)))
            .ForMember(dest => dest.FilmInfo.Director, opt => opt.MapFrom(src => ValidatedString.From(src.Director)))
            .ForMember(dest => dest.FilmInfo.Composer, opt => opt.MapFrom(src => ValidatedString.From(src.Composer)))
            .ForMember(dest => dest.FilmInfo.ReleaseYear, opt => opt.MapFrom(src => ReleaseYear.From(src.ReleaseYear)));
        CreateMap<Review, ReviewResponseDTO>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.Value));
        CreateMap<Review, ReviewRequestDTO>()
            .ForMember(dest => dest.rating, opt => opt.MapFrom(src => src.Rating.Value));
        CreateMap<ReviewResponseDTO, Review>();
        CreateMap<ReviewRequestDTO, Review>();
        CreateMap<VoiceActor, VoiceActorResponseDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));
        CreateMap<VoiceActor, VoiceActorRequestDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));
        CreateMap<VoiceActorResponseDTO, VoiceActor>();
        CreateMap<VoiceActorRequestDTO, Film>();
    }
}
 