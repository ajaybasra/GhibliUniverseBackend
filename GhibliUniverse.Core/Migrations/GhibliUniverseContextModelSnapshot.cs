﻿// <auto-generated />
using System;
using GhibliUniverse.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GhibliUniverse.Core.Migrations
{
    [DbContext(typeof(GhibliUniverseContext))]
    partial class GhibliUniverseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FilmVoiceActor", b =>
                {
                    b.Property<Guid>("FilmsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("VoiceActorsId")
                        .HasColumnType("uuid");

                    b.HasKey("FilmsId", "VoiceActorsId");

                    b.HasIndex("VoiceActorsId");

                    b.ToTable("FilmVoiceActor", (string)null);
                });

            modelBuilder.Entity("GhibliUniverse.Core.Domain.Models.Film", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Films");
                });

            modelBuilder.Entity("GhibliUniverse.Core.Domain.Models.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FilmId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("GhibliUniverse.Core.Domain.Models.VoiceActor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("VoiceActors");
                });

            modelBuilder.Entity("FilmVoiceActor", b =>
                {
                    b.HasOne("GhibliUniverse.Core.Domain.Models.Film", null)
                        .WithMany()
                        .HasForeignKey("FilmsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GhibliUniverse.Core.Domain.Models.VoiceActor", null)
                        .WithMany()
                        .HasForeignKey("VoiceActorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GhibliUniverse.Core.Domain.Models.Film", b =>
                {
                    b.OwnsOne("GhibliUniverse.Core.Domain.ValueObjects.ValidatedString", "Composer", b1 =>
                        {
                            b1.Property<Guid>("FilmId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .HasColumnType("text");

                            b1.HasKey("FilmId");

                            b1.ToTable("Films");

                            b1.WithOwner()
                                .HasForeignKey("FilmId");
                        });

                    b.OwnsOne("GhibliUniverse.Core.Domain.ValueObjects.ValidatedString", "Description", b1 =>
                        {
                            b1.Property<Guid>("FilmId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .HasColumnType("text");

                            b1.HasKey("FilmId");

                            b1.ToTable("Films");

                            b1.WithOwner()
                                .HasForeignKey("FilmId");
                        });

                    b.OwnsOne("GhibliUniverse.Core.Domain.ValueObjects.ValidatedString", "Director", b1 =>
                        {
                            b1.Property<Guid>("FilmId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .HasColumnType("text");

                            b1.HasKey("FilmId");

                            b1.ToTable("Films");

                            b1.WithOwner()
                                .HasForeignKey("FilmId");
                        });

                    b.OwnsOne("GhibliUniverse.Core.Domain.ValueObjects.ValidatedString", "Title", b1 =>
                        {
                            b1.Property<Guid>("FilmId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .HasColumnType("text");

                            b1.HasKey("FilmId");

                            b1.ToTable("Films");

                            b1.WithOwner()
                                .HasForeignKey("FilmId");
                        });

                    b.OwnsOne("GhibliUniverse.Core.Domain.ValueObjects.ReleaseYear", "ReleaseYear", b1 =>
                        {
                            b1.Property<Guid>("FilmId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Value")
                                .HasColumnType("integer");

                            b1.HasKey("FilmId");

                            b1.ToTable("Films");

                            b1.WithOwner()
                                .HasForeignKey("FilmId");
                        });

                    b.Navigation("Composer")
                        .IsRequired();

                    b.Navigation("Description")
                        .IsRequired();

                    b.Navigation("Director")
                        .IsRequired();

                    b.Navigation("ReleaseYear")
                        .IsRequired();

                    b.Navigation("Title")
                        .IsRequired();
                });

            modelBuilder.Entity("GhibliUniverse.Core.Domain.Models.Review", b =>
                {
                    b.HasOne("GhibliUniverse.Core.Domain.Models.Film", "Film")
                        .WithMany("Reviews")
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("GhibliUniverse.Core.Domain.ValueObjects.Rating", "Rating", b1 =>
                        {
                            b1.Property<Guid>("ReviewId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Value")
                                .HasColumnType("integer");

                            b1.HasKey("ReviewId");

                            b1.ToTable("Reviews");

                            b1.WithOwner()
                                .HasForeignKey("ReviewId");
                        });

                    b.Navigation("Film");

                    b.Navigation("Rating")
                        .IsRequired();
                });

            modelBuilder.Entity("GhibliUniverse.Core.Domain.Models.VoiceActor", b =>
                {
                    b.OwnsOne("GhibliUniverse.Core.Domain.ValueObjects.ValidatedString", "Name", b1 =>
                        {
                            b1.Property<Guid>("VoiceActorId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .HasColumnType("text");

                            b1.HasKey("VoiceActorId");

                            b1.ToTable("VoiceActors");

                            b1.WithOwner()
                                .HasForeignKey("VoiceActorId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("GhibliUniverse.Core.Domain.Models.Film", b =>
                {
                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
