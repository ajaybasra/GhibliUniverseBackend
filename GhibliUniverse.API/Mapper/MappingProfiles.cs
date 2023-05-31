using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.API.Mapper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Film, FilmDTO>();
        CreateMap<FilmDTO, Film>();
        CreateMap<Review, ReviewDTO>();
        CreateMap<ReviewDTO, Review>();
        CreateMap<VoiceActor, VoiceActorDTO>();
        CreateMap<VoiceActorDTO, VoiceActor>();
    }
}