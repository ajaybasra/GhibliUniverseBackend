using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.API.DTOs;

public record ReviewDTO()
{
    public Guid Id { get; init; }
    public Rating Rating { get; set; }
}