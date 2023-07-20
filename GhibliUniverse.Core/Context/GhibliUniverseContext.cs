using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
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
        modelBuilder.Entity<Film>()
            .HasMany(f => f.VoiceActors)
            .WithMany(v => v.Films)
            .UsingEntity(join => join.ToTable("FilmVoiceActor"));
        
        modelBuilder.Entity<Film>() // mapping value objects as complex types
            .OwnsOne(f => f.Title);
    
        modelBuilder.Entity<Film>()
            .OwnsOne(f => f.Description);
    
        modelBuilder.Entity<Film>()
            .OwnsOne(f => f.Director);
    
        modelBuilder.Entity<Film>()
            .OwnsOne(f => f.Composer);
    
        modelBuilder.Entity<Film>()
            .OwnsOne(f => f.ReleaseYear);
        
        modelBuilder.Entity<VoiceActor>()
            .OwnsOne(va => va.Name);
        
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Film)
            .WithMany(f => f.Reviews)
            .HasForeignKey(r => r.FilmId)
            .IsRequired() // review cannot exist without film
            .OnDelete(DeleteBehavior.Cascade); // when a film is deleted, so are associated reviews
        
        modelBuilder.Entity<Review>()
            .OwnsOne(r => r.Rating);
        
    }
}
