using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Helper;
using Snagged.Infrastructure.Database;
using Snagged.Infrastructure.Services;

namespace Snagged.Infrastructure.Commom
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly("Snagged.Infrastructure")
                )
            );

            services.AddScoped<IAppDbContext>(provider =>
                provider.GetRequiredService<DatabaseContext>());

            
            services.Configure<VapidSettings>(config.GetSection("Vapid"));
            services.AddScoped<IWebPushService, WebPushService>();

            return services;
        }
    }
}