using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.API.Mapper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Film, FilmResponseDTO>();
        CreateMap<FilmResponseDTO, Film>();
        CreateMap<FilmRequestDTO, Film>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => ValidatedString.From(src.Title)))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => ValidatedString.From(src.Description)))
            .ForMember(dest => dest.Director, opt => opt.MapFrom(src => ValidatedString.From(src.Director)))
            .ForMember(dest => dest.Composer, opt => opt.MapFrom(src => ValidatedString.From(src.Composer)))
            .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => ReleaseYear.From(src.ReleaseYear)));
        CreateMap<Review, ReviewResponseDTO>();
        CreateMap<ReviewResponseDTO, Review>();
        CreateMap<ReviewRequestDTO, Review>();
        CreateMap<VoiceActor, VoiceActorResponseDTO>();
        CreateMap<VoiceActorResponseDTO, VoiceActor>();
        CreateMap<VoiceActorRequestDTO, Film>();
    }
}
 