using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Snagged.Infrastructure.Database.Seeders;
using System.Threading.Tasks;

namespace Snagged.Infrastructure.Database
{
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Apply migrations and seed data for Snagged project.
        /// </summary>
        public static async Task InitializeDatabaseAsync(this IServiceProvider services, IHostEnvironment env)
        {
            await using var scope = services.CreateAsyncScope();
            var ctx = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            if (env.IsEnvironment("Test"))
            {
                
                await ctx.Database.EnsureDeletedAsync();
                await ctx.Database.EnsureCreatedAsync();

               
                await DynamicDataSeeder.SeedAsync(ctx);
                return;
            }

            // Apply any pending migrations (SQL Server / Dev environment)
            await ctx.Database.MigrateAsync();

            if (env.IsDevelopment())
            {
                await StaticDataSeeder.SeedAsync(ctx);  // <-- ovo je ispravno
                await DynamicDataSeeder.SeedAsync(ctx);
            }
        }
    }
}
