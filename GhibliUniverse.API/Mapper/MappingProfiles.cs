using AutoMapper;
using GhibliUniverse.API.DTOs;
using GhibliUniverse.Core.Domain.Models;

namespace GhibliUniverse.API.Mapper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Film, FilmResponseDTO>();
        CreateMap<FilmResponseDTO, Film>();
        CreateMap<FilmRequestDTO, Film>();
        CreateMap<Review, ReviewResponseDTO>();
        CreateMap<ReviewResponseDTO, Review>();
        CreateMap<VoiceActor, VoiceActorResponseDTO>();
        CreateMap<VoiceActorResponseDTO, VoiceActor>();
    }
}