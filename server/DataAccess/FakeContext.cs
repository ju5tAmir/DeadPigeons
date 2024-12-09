using System;
using System.Collections.Generic;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public partial class FakeContext : DbContext
{
    public FakeContext(DbContextOptions<FakeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<MobilePayPayment> MobilePayPayments { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<Preference> Preferences { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Winner> Winners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsAutoPlay).HasDefaultValue(false);
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.BoardId).HasName("Boards_pkey");

            entity.Property(e => e.BoardId).ValueGeneratedNever();
            entity.Property(e => e.PlayDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

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

        modelBuilder.Entity<MobilePayPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("MobilePayPayments_pkey");

            entity.Property(e => e.PaymentId).ValueGeneratedNever();

            entity.HasOne(d => d.Transaction).WithMany(p => p.MobilePayPayments).HasConstraintName("MobilePayPayments_TransactionId_fkey");
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

            entity.HasOne(d => d.Player).WithMany(p => p.Winners).HasConstraintName("Winners_PlayerId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
