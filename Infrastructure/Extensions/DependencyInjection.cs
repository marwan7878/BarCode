using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureDatabase(configuration);
            services.ConfigureApplicationServices();
            services.ConfigureIdentity();

            return services;
        }
    }
}
