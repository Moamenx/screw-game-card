using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrewGameCard.Contract.Interface;
using ScrewGameCard.Contract.Repository;
using ScrewGameCard.Infrastructure.Data;
using ScrewGameCard.Infrastructure.Repository;
using ScrewGameCard.Infrastructure.Service;

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
            services.AddScoped<IGameRoomRepository, GameRoomRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IGameNotificationService, GameNotificationService>();
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
