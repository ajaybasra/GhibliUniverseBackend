using AutoMapper;
using GhibliUniverse.Core.DataEntities;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using System.Collections.Generic;

namespace GhibliUniverse.Core.Repository.MappingProfiles
{
    public class MappingProfiles : Profile
    { 
        public MappingProfiles()
        {
            CreateMap<FilmEntity, VoiceActorFilm>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => ValidatedString.From(src.Title)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => ValidatedString.From(src.Description)))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => ValidatedString.From(src.Director)))
                .ForMember(dest => dest.Composer, opt => opt.MapFrom(src => ValidatedString.From(src.Composer)))
                .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => ReleaseYear.From(src.ReleaseYear)));

            CreateMap<VoiceActorEntity, VoiceActor>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => ValidatedString.From(src.Name)));
                // .ForMember(dest => dest.Films, opt => opt.MapFrom(src => src.Films));

            CreateMap<ReviewEntity, Review>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => Rating.From(src.Rating)));
            
            CreateMap<FilmEntity, FilmInfo>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => ValidatedString.From(src.Title)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => ValidatedString.From(src.Description)))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => ValidatedString.From(src.Director)))
                .ForMember(dest => dest.Composer, opt => opt.MapFrom(src => ValidatedString.From(src.Composer)))
                .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => ReleaseYear.From(src.ReleaseYear)))
                .ForMember(dest => dest.VoiceActors, opt => opt.MapFrom(src => src.VoiceActors))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));
            
            CreateMap<FilmEntity, Film>()
                .ConstructUsing((src, context) => new Film(
                    context.Mapper.Map<FilmInfo>(src)));
        }
    }
}