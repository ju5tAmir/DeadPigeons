using API.Misc;
using DataAccess;
using DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Service;
using Service.Auth;
using Service.Authorization;
using Service.Board;
using Service.Game;
using Service.Package;
using Service.Repositories;
using Service.Security;
using Service.Transactions;
using Service.Upload;
using Service.Users;
using Service.Winner;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Configuration
        builder
            .Services.AddOptionsWithValidateOnStart<AppOptions>()
            .Bind(builder.Configuration.GetSection(nameof(AppOptions)))
            .ValidateDataAnnotations();
        builder.Services.AddSingleton(_ => TimeProvider.System);
        var configuration = builder.Configuration;
        Environment.SetEnvironmentVariable("BUCKET_NAME", configuration["BUCKET_NAME"]);
        #endregion

        #region Data Access
        var connectionString = builder.Configuration.GetConnectionString("AppDb");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options
                .UseNpgsql(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );
        builder.Services.AddScoped<DbSeeder>();
        builder.Services.AddScoped<IRepository<User>, UserRepository>();
        builder.Services.AddScoped<IRepository<Game>, GameRepository>();
        builder.Services.AddScoped<IRepository<Board>, BoardRepository>();
        builder.Services.AddScoped<IRepository<Package>, PackageRepository>();
        builder.Services.AddScoped<IRepository<Winner>, WinnerRepository>();
        builder.Services.AddScoped<IRepository<Transaction>, TransactionRepository>();
        builder.Services.AddScoped<IRepository<ManualPayment>, ManualPaymentRepository>();
        #endregion

        #region Security
        builder
            .Services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
        builder.Services.AddSingleton<IPasswordHasher<User>, Argon2IdPasswordHasher<User>>();
        var options = builder.Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>()!;
        builder
            .Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = JwtTokenClaimService.ValidationParameters(options);
            });
        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                // Globally require users to be authenticated
                .RequireAuthenticatedUser()
                .Build();
        });
        builder.Services.AddScoped<ITokenClaimsService, JwtTokenClaimService>();
        builder.Services.AddSingleton<IEmailSender<User>, AppEmailSender>();
        #endregion

        #region Services
        builder.Services.AddValidatorsFromAssemblyContaining<ServiceAssembly>();
        builder.Services.AddScoped<IAuthority, Authority>();
        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddScoped<IBoardService, BoardService>();
        builder.Services.AddScoped<IPackageService, PackageService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IWinnerService, WinnerService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUploadService, UploadService>();
        #endregion

        #region Swagger
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                }
            );
            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new string[] { }
                    }
                }
            );
            c.OperationFilter<FileUploadOperationFilter>();
        });
        #endregion

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        // Seed
        using (var scope = app.Services.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<DbSeeder>().SeedAsync().Wait();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api/swagger";
            });
        }

        app.UseHttpsRedirection();
        
        // app.UseDeveloperExceptionPage();
   
        app.MapControllers();
        app.UseCors( opts => {
            opts.AllowAnyOrigin();
            opts.AllowAnyMethod();
            opts.AllowAnyHeader();
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.Run();
    }
}
