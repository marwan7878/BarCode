using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class DatabaseSeeder
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }

        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            //using var scope = app.Services.CreateScope();
            //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //await DefaultRoles.SeedAsync(roleManager);
            //await DefaultUsers.SeedAdminUserAsync(userManager, roleManager);
        }
    }
}
