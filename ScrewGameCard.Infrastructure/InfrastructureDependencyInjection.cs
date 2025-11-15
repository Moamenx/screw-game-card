using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.Repository;
using ScrewGameCard.Domain.Entities;
using ScrewGameCard.Infrastructure.Data;
using ScrewGameCard.Infrastructure.Repositories;
using ScrewGameCard.Infrastructure.Repository;
using ScrewGameCard.Infrastructure.Services;

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
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAuthenticationProvider, JwtAuthenticationProvider>();
        }

        private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ScrewGameCardDbContext>(opts =>
            {
                opts.UseNpgsql(configuration.GetConnectionString("ScrewGameCardDb"));
                opts.EnableDetailedErrors();
            });
        }
    }
}
