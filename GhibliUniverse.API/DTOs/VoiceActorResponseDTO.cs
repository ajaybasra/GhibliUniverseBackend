using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.API.DTOs;

public record VoiceActorResponseDTO()
{
    public Guid Id { get; init; }
    public ValidatedString Name { get; set; }
}