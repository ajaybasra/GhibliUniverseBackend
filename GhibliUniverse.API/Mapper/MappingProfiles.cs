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
        CreateMap<Film, FilmRequestDTO>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
            .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director.Value))
            .ForMember(dest => dest.Composer, opt => opt.MapFrom(src => src.Composer.Value))
            .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.ReleaseYear.Value));
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
        CreateMap<VoiceActor, VoiceActorRequestDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));
        CreateMap<VoiceActorResponseDTO, VoiceActor>();
        CreateMap<VoiceActorRequestDTO, Film>();
    }
}
 