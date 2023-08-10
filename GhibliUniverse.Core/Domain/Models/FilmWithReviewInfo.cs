using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Domain.Models;

public record FilmWithReviewInfo()
{
    public Guid Id { get; set; }
    public ValidatedString Title { get; set; }
    public ValidatedString Description { get; set; }
    public ValidatedString Director { get; set; }
    public ValidatedString Composer { get; set; }
    public ReleaseYear ReleaseYear { get; set; }

    public FilmReviewInfo FilmReviewInfo { get; set; } = new FilmReviewInfo();
}