using GhibliUniverse.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GhibliUniverse.Core.Context;

public class GhibliUniverseContext : DbContext
{
    public GhibliUniverseContext(DbContextOptions<GhibliUniverseContext> options) : base(options)
    {
        
    }
    
    public DbSet<Film> Films { get; set; }
    public DbSet<VoiceActor> VoiceActors { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Film)
            .WithMany(f => f.Reviews)
            .HasForeignKey(r => r.FilmId)
            .IsRequired() // review cannot exist without film
            .OnDelete(DeleteBehavior.Cascade); // when a film is deleted, so are associated reviews
        
    }
}
