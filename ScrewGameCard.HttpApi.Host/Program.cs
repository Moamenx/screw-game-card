using Serilog;
using ScrewGameCard.Application;
using ScrewGameCard.Infrastructure;
using ScrewGameCard.Infrastructure.SignalR;
using ScrewGameCard.HttpApi.Host.Middleware;
using ScrewGameCard.DomainShared;

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
        Log.Logger.Information("Registering services...");

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.Configure<CachingDurationOption>(builder.Configuration.GetSection(CachingDurationOption.SectionName));
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Cors",p =>
            {
                p.WithOrigins(GetAllowedOrigins()).AllowAnyMethod().AllowCredentials().AllowAnyHeader();
            });
        });
        Log.Logger.Information("Registered all services!");
        Log.Logger.Information("Application is ready!");

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
