using System;
using System.Text;
using GhibliUniverse.Core.Domain.ValueObjects;

namespace GhibliUniverse.Core.Domain.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public Rating Rating { get; set; }
        
        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Rating);
            return str.ToString();
        }
    }
}