using Serilog;
using ScrewGameCard.Application;
using ScrewGameCard.Infrastructure;
using ScrewGameCard.Infrastructure.Services;
using ScrewGameCard.Infrastructure.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ScrewGameCard.HttpApi.Host.Middleware;

namespace ScrewGameCard.HttpApi.Host;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog();

        Log.Logger.Information("Starting ScrewGameCard HttpApi.Host...");
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Cors",p =>
            {
                p.WithOrigins(GetAllowedOrigins()).AllowAnyMethod().AllowCredentials().AllowAnyHeader();
            });
        });

        // Add Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

        builder.Services.AddHostedService<RefreshTokenCleanupService>();

        var app = builder.Build();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseRouting();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("Cors");
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.MapHub<GameHub>("/gamehub");
        app.Run();
    }

    private static string[] GetAllowedOrigins()
    {
        var allowedOrigins = new[]
        {
            "http://localhost:54435",
            "https://localhost:54435",
            "http://localhost:4200", // Common Angular dev server port
        };
        return allowedOrigins;
    }
}
