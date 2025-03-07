﻿using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public virtual DbSet<Game> Games { get; set; }
    public virtual DbSet<Board> Boards { get; set; }
    public virtual DbSet<Package> Packages { get; set; }
    public virtual DbSet<Winner> Winners { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<ManualPayment> ManualPayments { get; set; }


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
            entity.Property(e => e.OfflineBoards).HasDefaultValue(0);
            entity.Property(e => e.OfflinePlayers).HasDefaultValue(0);
            entity.Property(e => e.OfflineWinningBoards).HasDefaultValue(0);
            entity.Property(e => e.OfflineWinningPlayers).HasDefaultValue(0);
            entity.Property(e => e.OnlineBoards).HasDefaultValue(0);
            entity.Property(e => e.OnlinePlayers).HasDefaultValue(0);
            entity.Property(e => e.OnlineWinningBoards).HasDefaultValue(0);
            entity.Property(e => e.OnlineWinningPlayers).HasDefaultValue(0);
        });

        modelBuilder.Entity<ManualPayment>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("ManualPayment_pkey");

            entity.Property(e => e.TransactionId).ValueGeneratedNever();

            entity.HasOne(d => d.Transaction).WithOne(p => p.ManualPayment).HasConstraintName("ManualPayment_TransactionId_fkey");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("Packages_pkey");

            entity.Property(e => e.PackageId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("Transactions_pkey");

            entity.Property(e => e.TransactionId).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValueSql("'Pending'::character varying");
            entity.Property(e => e.TransactionDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions).HasConstraintName("Transactions_UserId_fkey");
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
