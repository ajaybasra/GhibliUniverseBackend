using GhibliUniverse.Core.DataEntities;
using GhibliUniverse.Core.Domain.Models;
using GhibliUniverse.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GhibliUniverse.Core.Context;

public class GhibliUniverseContext : DbContext
{
    public GhibliUniverseContext(DbContextOptions<GhibliUniverseContext> options) : base(options)
    {
        
    }
    
    public DbSet<FilmEntity> Films { get; set; }
    public DbSet<VoiceActorEntity> VoiceActors { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FilmEntity>()
            .HasMany(f => f.VoiceActors)
            .WithMany(v => v.Films)
            .UsingEntity(join => join.ToTable("FilmVoiceActor"));

        modelBuilder.Entity<ReviewEntity>()
            .HasOne(r => r.Film)
            .WithMany(f => f.Reviews)
            .HasForeignKey(r => r.FilmId)
            .IsRequired() // review cannot exist without film
            .OnDelete(DeleteBehavior.Cascade); // when a film is deleted, so are associated reviews
    }
}