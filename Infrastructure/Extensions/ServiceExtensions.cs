using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Missing connection string.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

        }

        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }

        public static void ConfigureWebLayer(this IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
        }
    }
}
