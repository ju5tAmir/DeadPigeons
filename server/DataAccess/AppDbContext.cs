using DataAccess.Entities;
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
        

        
        base.OnModelCreating(modelBuilder);
    }

    // public virtual DbSet<Preference> Preferences { get; set; }

}
