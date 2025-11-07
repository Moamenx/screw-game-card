using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrewGameCard.Contract.Interface;
using ScrewGameCard.Contract.Repository;
using ScrewGameCard.Infrastructure.Repository;
using ScrewGameCard.Infrastructure.Service;

namespace ScrewGameCard.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
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
    }
}
