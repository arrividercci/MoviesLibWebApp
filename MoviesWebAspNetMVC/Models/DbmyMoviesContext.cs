using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MoviesWebAspNetMVC.Models;

public partial class DbmyMoviesContext : DbContext
{
    public DbmyMoviesContext()
    {
    }

    public DbmyMoviesContext(DbContextOptions<DbmyMoviesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    public virtual DbSet<Recension> Recensions { get; set; }

    public virtual DbSet<UsersMarkedMovie> UsersMarkedMovies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasOne(d => d.Author).WithMany(p => p.Movies)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Movies_Authors");

            entity.HasOne(d => d.Country).WithMany(p => p.Movies)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Movies_Countries");

            entity.HasOne(d => d.Genre).WithMany(p => p.Movies)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Movies_Genres");
        });

        modelBuilder.Entity<Rate>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Recension>(entity =>
        {
            entity.HasOne(d => d.Movie).WithMany(p => p.Recensions)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Recensions_Movies");

            entity.HasOne(d => d.Rate).WithMany(p => p.Recensions)
                .HasForeignKey(d => d.RateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Recensions_Rates");
        });

        modelBuilder.Entity<UsersMarkedMovie>(entity =>
        {
            entity.HasOne(d => d.Recencion).WithMany(p => p.UsersMarkedMovies)
                .HasForeignKey(d => d.RecencionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UsersMarkedMovies_Recensions");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
