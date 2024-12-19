using System.Net.Http.Headers;
using API;
using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PgCtx;
using Service;
using Service.Security;
using Service.Upload;
using Service.Users;
using Xunit.Abstractions;
using Program = API.Program;

namespace ApiIntegrationTests;

public class ApiTestBase : WebApplicationFactory<Program>
{
    private readonly ITestOutputHelper _outputHelper;

    public ApiTestBase(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        PgCtxSetup = new PgCtxSetup<AppDbContext>();
        Environment.SetEnvironmentVariable(nameof(AppOptions) + ":" + nameof(AppOptions.DbConnectionString),
            PgCtxSetup._postgres.GetConnectionString());
        ApplicationServices = base.Services.CreateScope().ServiceProvider;

        Logger = ApplicationServices.GetRequiredService<ILogger<ApiTestBase>>();
        UserManager = ApplicationServices.GetRequiredService<UserManager<User>>();
        RoleManager = ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();
        TokenClaimsService = ApplicationServices.GetRequiredService<ITokenClaimsService>();
        Seed().GetAwaiter().GetResult();
        
        TestHttpClient = CreateClient();
        TestHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserJwt);

        TestHttpAdmin = CreateClient();
        TestHttpAdmin.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AdminJwt);
    }

    /// <summary>
    ///     Data that will be populated before each test
    /// </summary>
    public async Task Seed()
    {
        var ctx = ApplicationServices.GetRequiredService<AppDbContext>();

        await CreateRoles(Role.Admin, Role.Player);
        await CreateUser(firstName: "John", lastName: "TheRipper", email: "mockadmin@example.com", username: "mockadmin", password: "S3cret!", role: Role.Admin, phone: "123123123", isActive: true, isAutoplay: false, registrationDate: DateTime.UtcNow);
        await CreateUser(firstName: "Beef", lastName: "Steven", email: "mockuser1@example.com", username: "mockuser1", password: "S3cret!", role: Role.Player, phone: "321321321", isActive: true, isAutoplay: false, registrationDate: DateTime.UtcNow);
        await CreateUser(firstName: "Pigeon", lastName: "Bradly", email: "mockuser2@example.com", username: "mockuser2", password: "S3cret!", role: Role.Player, phone: "321321321", isActive: true, isAutoplay: false, registrationDate: DateTime.UtcNow);

        await ctx.SaveChangesAsync();
        
        AdminJwt = await TokenClaimsService.GetTokenAsync("mockadmin");
        UserJwt = await TokenClaimsService.GetTokenAsync("mockuser1");
    }

    private async Task CreateRoles(params string[] roles)
    {
        foreach (var role in roles)
        {
            await RoleManager.CreateAsync(new IdentityRole(role));
        }
    }

    private async Task CreateUser(string firstName, string lastName, string email, string username, string? phone, string password, string role, bool isActive, bool isAutoplay, DateTime registrationDate)
    {
        if (await UserManager.FindByNameAsync(username) != null) return;
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

        var result = await UserManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                Logger.LogWarning("{Code}: {Description}", error.Code, error.Description);
            }
        }
        await UserManager.AddToRoleAsync(user!, role!);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null) services.Remove(descriptor);
            
            var uploadService = services.SingleOrDefault(
                d => d.ServiceType == typeof(IUserService));
            if(uploadService != null) services.Remove(uploadService);

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseNpgsql(PgCtxSetup._postgres.GetConnectionString());
                opt.EnableSensitiveDataLogging(true);
                opt.LogTo(_ => { });

            });
            services.AddScoped<IUploadService, MockUploadService>();

        }).ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddXUnit(_outputHelper);
        });
        return base.CreateHost(builder);
    }

    #region properties

    public PgCtxSetup<AppDbContext> PgCtxSetup { get; set; }
    public HttpClient TestHttpClient { get; set; }
    public HttpClient TestHttpAdmin { get; set; }
    public string UserJwt { get; set; } 
    public string AdminJwt { get; set; } 
    public IServiceProvider ApplicationServices { get; set; }
    public ILogger<ApiTestBase> Logger { get; set; }
    public UserManager<User> UserManager { get; set; }
    public RoleManager<IdentityRole> RoleManager { get; set; }
    public ITokenClaimsService TokenClaimsService { get; set; }

    #endregion
}