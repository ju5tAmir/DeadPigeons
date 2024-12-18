using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Service;

public class DbSeeder
{
    private readonly ILogger<DbSeeder> logger;
    private readonly AppDbContext context;
    private readonly UserManager<DataAccess.Entities.User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public DbSeeder(
        ILogger<DbSeeder> logger,
        AppDbContext context,
        UserManager<DataAccess.Entities.User> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        this.logger = logger;
        this.context = context;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await context.Database.EnsureCreatedAsync();

        await CreateRoles(Role.Admin, Role.Player);
        await CreateUser(firstName: "John", lastName: "Doe", email: "admin@example.com", username: "admin", password: "S3cret!", role: Role.Admin, phone: "123", isActive:true, isAutoplay:false, registrationDate:DateTime.UtcNow);
        await CreateUser(firstName: "Neo", lastName: "Liza", email: "player1@example.com", username: "player1", password: "S3cret!", role: Role.Player, phone: "1234", isActive:true, isAutoplay:false, registrationDate:DateTime.UtcNow);
        await CreateUser(firstName: "Mads", lastName: "Christian", email: "player2@example.com", username: "player2", password: "S3cret!", role: Role.Player, phone: "5678", isActive:true, isAutoplay:false, registrationDate:DateTime.UtcNow);
        
        await SeedPackages();
        
        await context.SaveChangesAsync();
    }

    private async Task CreateRoles(params string[] roles)
    {
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    async Task CreateUser(string firstName, string lastName, string email, string username, string? phone, string password, string role, bool isActive, bool isAutoplay, DateTime registrationDate)
    {
        if (await userManager.FindByNameAsync(username) != null) return;
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            UserName = username,
            Email = email,
            EmailConfirmed = true,
            PhoneNumber = phone,
            PhoneNumberConfirmed = false,
            IsActive = isActive,
            IsAutoPlay = isActive,
            RegistrationDate = registrationDate
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                logger.LogWarning("{Code}: {Description}", error.Code, error.Description);
            }
        }
        await userManager.AddToRoleAsync(user!, role!);
        
    }
    
    private async Task SeedPackages()
    {
        if (!await context.Packages.AnyAsync()) 
        {
            var packages = new List<DataAccess.Entities.Package>
            {
                new DataAccess.Entities.Package
                {
                    PackageId = Guid.NewGuid(),
                    NumberOfFields = 5,
                    Price = 20.00M
                },
                new DataAccess.Entities.Package
                {
                    PackageId = Guid.NewGuid(),
                    NumberOfFields = 6,
                    Price = 40.00M
                },
                new DataAccess.Entities.Package
                {
                    PackageId = Guid.NewGuid(),
                    NumberOfFields = 7,
                    Price = 80.00M
                },
                new DataAccess.Entities.Package
                {
                    PackageId = Guid.NewGuid(),
                    NumberOfFields = 8,
                    Price = 160.00M
                }
            };

            await context.Packages.AddRangeAsync(packages);
        }
    }

}