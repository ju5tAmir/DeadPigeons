﻿using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public virtual DbSet<Preference> Preferences { get; set; }
    public virtual DbSet<Game> Games { get; set; }
    public virtual DbSet<Board> Boards { get; set; }
    public virtual DbSet<Package> Packages { get; set; }
    public virtual DbSet<Winner> Winners { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            // Define properties for the User entity
            entity.Property(e => e.FirstName)
                .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .HasMaxLength(100);

            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            // RegistrationDate will default to current UTC time when a new User is created
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")  
                .HasColumnName("RegistrationDate");

            entity.Property(e => e.IsActive)
                .HasColumnType("boolean")
                .HasDefaultValue(true);

            entity.Property(e => e.IsAutoPlay)
                .HasColumnType("boolean")
                .HasDefaultValue(false);
        });
        

        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.BoardId).HasName("Boards_pkey");

            entity.Property(e => e.BoardId).ValueGeneratedNever();
            entity.Property(e => e.PlayDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Game).WithMany(p => p.Boards).HasConstraintName("Boards_GameId_fkey");

            entity.HasOne(d => d.Package).WithMany(p => p.Boards).HasConstraintName("Boards_PackageId_fkey");

            entity.HasOne(d => d.Player).WithMany(p => p.Boards).HasConstraintName("Boards_PlayerId_fkey");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("Games_pkey");

            entity.Property(e => e.GameId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("Packages_pkey");

            entity.Property(e => e.PackageId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Preference>(entity =>
        {
            entity.HasKey(e => e.UserSettingsId).HasName("Preferences_pkey");

            entity.Property(e => e.IfBalanceIsNegative).HasDefaultValue(true);
            entity.Property(e => e.IfPlayerWon).HasDefaultValue(true);

            entity.HasOne(d => d.User).WithOne(p => p.Preference).HasConstraintName("Preferences_UserId_fkey");
        });

        modelBuilder.Entity<Winner>(entity =>
        {
            entity.HasKey(e => e.WinningRecordId).HasName("Winners_pkey");

            entity.Property(e => e.WinningRecordId).ValueGeneratedNever();

            entity.HasOne(d => d.Board).WithMany(p => p.Winners).HasConstraintName("Winners_BoardId_fkey");

            entity.HasOne(d => d.Game).WithMany(p => p.Winners).HasConstraintName("Winners_GameId_fkey");

            entity.HasOne(d => d.Player).WithMany(p => p.Winners).HasConstraintName("Winners_PlayerId_fkey");
        });
        
        base.OnModelCreating(modelBuilder);
    }

    // public virtual DbSet<Preference> Preferences { get; set; }

}
