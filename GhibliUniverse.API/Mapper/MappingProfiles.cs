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
            .ForPath(dest => dest.Title, opt => opt.MapFrom(src => src.FilmInfo.Title.Value))
            .ForPath(dest => dest.Description, opt => opt.MapFrom(src => src.FilmInfo.Description.Value))
            .ForPath(dest => dest.Director, opt => opt.MapFrom(src => src.FilmInfo.Director.Value))
            .ForPath(dest => dest.Composer, opt => opt.MapFrom(src => src.FilmInfo.Composer.Value))
            .ForPath(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.FilmInfo.ReleaseYear.Value))
            .AfterMap((src, dest) => {
                dest.FilmReviewInfo = src.FilmReviewInfo;
            });
        
        CreateMap<FilmRequestDTO, FilmInfo>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => ValidatedString.From(src.Title)))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => ValidatedString.From(src.Description)))
            .ForMember(dest => dest.Director, opt => opt.MapFrom(src => ValidatedString.From(src.Director)))
            .ForMember(dest => dest.Composer, opt => opt.MapFrom(src => ValidatedString.From(src.Composer)))
            .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => ReleaseYear.From(src.ReleaseYear)));
        
        CreateMap<FilmRequestDTO, Film>()
            .ConstructUsing((src, context) => new Film(
                context.Mapper.Map<FilmInfo>(src)));
        
        CreateMap<Review, ReviewResponseDTO>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.Value));
        
        CreateMap<Review, ReviewRequestDTO>()
            .ForMember(dest => dest.rating, opt => opt.MapFrom(src => src.Rating.Value));
        
        CreateMap<ReviewRequestDTO, Review>();
        
        CreateMap<VoiceActor, VoiceActorResponseDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));
        
        CreateMap<VoiceActorFilm, FilmResponseDTO>()
            .ForPath(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Value))
            .ForPath(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
            .ForPath(dest => dest.Director, opt => opt.MapFrom(src => src.Director.Value))
            .ForPath(dest => dest.Composer, opt => opt.MapFrom(src => src.Composer.Value))
            .ForPath(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.ReleaseYear.Value));
        
        CreateMap<VoiceActorFilm, VoiceActorFilmResponseDTO>()
            .ForPath(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Value))
            .ForPath(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
            .ForPath(dest => dest.Director, opt => opt.MapFrom(src => src.Director.Value))
            .ForPath(dest => dest.Composer, opt => opt.MapFrom(src => src.Composer.Value))
            .ForPath(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.ReleaseYear.Value));
        // CreateMap<VoiceActor, VoiceActorRequestDTO>()
        //     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));
        // CreateMap<VoiceActorResponseDTO, VoiceActor>();
        CreateMap<VoiceActorRequestDTO, VoiceActor>();
    }
}
 