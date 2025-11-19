using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.Repositories;
using ScrewGameCard.Domain.Entities;
using ScrewGameCard.Infrastructure.Data;
using ScrewGameCard.Infrastructure.Repositories;
using ScrewGameCard.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ScrewGameCard.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            ConfigureDatabase(services,configuration);
            RegisterRepositories(services); 
            RegisterServices(services);
            RegisterSingleR(services);
            AddHostedServices(services);
            ConfigureAuthentication(services, configuration);
            return services;
        }

        private static void RegisterSingleR(IServiceCollection services)
        {
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.KeepAliveInterval = TimeSpan.FromSeconds(10);
            });

        }
        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGenericRepository<GamePlayer>, GenericRepository<GamePlayer>>();
            services.AddScoped<IGenericRepository<Round>, GenericRepository<Round>>();
            services.AddScoped<IGenericRepository<RefreshToken>, RefreshTokenRepository>();
            services.AddScoped<ILocalizationRepository, LocalizationRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAuthenticationProvider, JwtAuthenticationProvider>();
            services.AddScoped<IRateLimiter, RateLimiterService>();
            services.AddMemoryCache();
            services.AddScoped<ILocalizationService, LocalizationService>();
        }

        private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ScrewGameCardDbContext>(opts =>
            {
                opts.UseNpgsql(configuration.GetConnectionString("ScrewGameCardDb"));
                opts.EnableDetailedErrors();
            });
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private static void AddHostedServices(IServiceCollection services)
        {
            services.AddHostedService<RefreshTokenCleanupService>();
            services.AddHostedService<LocalizationCacheHostedService>();
        }
    }
}
