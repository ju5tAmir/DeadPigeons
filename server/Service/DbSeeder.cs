using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Service;

public class DbSeeder
{
    private readonly ILogger<DbSeeder> logger;
    private readonly AppDbContext context;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public DbSeeder(
        ILogger<DbSeeder> logger,
        AppDbContext context,
        UserManager<User> userManager,
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
        await CreateUser(email: "admin@example.com", username: "admin", password: "S3cret!", role: Role.Admin, phone: "");
        await CreateUser(email: "player1@example.com", username: "player1", password: "S3cret!", role: Role.Player, phone: "1234");
        await CreateUser(email: "player2@example.com", username: "player2", password: "S3cret!", role: Role.Player, phone: "5678");
        
        await context.SaveChangesAsync();
    }

    private async Task CreateRoles(params string[] roles)
    {
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    async Task CreateUser(string email, string username, string? phone, string password, string role)
    {
        if (await userManager.FindByNameAsync(username) != null) return;
        var user = new User
        {
            UserName = username,
            Email = email,
            EmailConfirmed = true,
            PhoneNumber = phone,
            PhoneNumberConfirmed = false,
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

}