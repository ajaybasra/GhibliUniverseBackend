using System.Text;

namespace GhibliUniverse.Core.Domain.Models
{
    public class Film
    {
        public Guid Id => FilmInfo.Id;
        
        public FilmInfo FilmInfo { get; set; } = new();  

        public FilmReviewInfo FilmReviewInfo { get; set; } = new();  
    
        public Film(FilmInfo filmInfo)
        {
            FilmInfo = filmInfo;
        }
        
        public Film(FilmInfo filmInfo, FilmReviewInfo filmReviewInfo)
        {
            FilmInfo = filmInfo;
            FilmReviewInfo = filmReviewInfo;
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append('[');
            str.Append($"Title:{FilmInfo.Title},");
            str.Append($"Description:{FilmInfo.Description},");
            str.Append($"Director:{FilmInfo.Director},");
            str.Append($"Composer:{FilmInfo.Composer},");
            str.Append($"Release Year:{FilmInfo.ReleaseYear},");
            str.Append("Voice Actors:");
            str.Append('[');
            str.Append(string.Join(",", FilmInfo.VoiceActors));
            str.Append(']');
            str.Append(",Film Ratings:");
            str.Append('[');
            str.Append(string.Join(",", FilmInfo.Reviews));
            str.Append(']');
            str.Append(']');
            return str.ToString();
        }
    }
}