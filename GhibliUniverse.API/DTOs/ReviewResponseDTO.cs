using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.API.DTOs;

public record ReviewResponseDTO()
{
    public Guid Id { get; init; }
    public Rating Rating { get; set; }
}