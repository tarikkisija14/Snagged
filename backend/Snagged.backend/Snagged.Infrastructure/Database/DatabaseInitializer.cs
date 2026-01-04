using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Snagged.Infrastructure.Database.Seeders;

namespace Snagged.Infrastructure.Database
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync(this IServiceProvider services, IHostEnvironment env)
        {
            await using var scope = services.CreateAsyncScope();
            var ctx = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseContext>>();

            try
            {
                if (env.IsEnvironment("Test"))
                {
                    logger.LogInformation("Refreshing database for Test environment...");
                    await ctx.Database.EnsureDeletedAsync();
                    await ctx.Database.EnsureCreatedAsync();

                    await StaticDataSeeder.SeedAsync(ctx);
                    await DynamicDataSeeder.SeedAsync(ctx);
                    return;
                }

                logger.LogInformation("Applying migrations...");
                await ctx.Database.MigrateAsync();

                if (env.IsDevelopment())
                {
                    logger.LogInformation("Seeding data for Development...");
                    await StaticDataSeeder.SeedAsync(ctx);
                    await DynamicDataSeeder.SeedAsync(ctx);
                }

                logger.LogInformation("Database initialization completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database initialization.");
                throw; 
            }
        }
    }
}