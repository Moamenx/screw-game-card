using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.Service;

namespace ScrewGameCard.Application
{
    public static class ApplicationDependencyInjection 
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterServices(services);
            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IGameEngine, GameEngine>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGameManager, GameManager>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
