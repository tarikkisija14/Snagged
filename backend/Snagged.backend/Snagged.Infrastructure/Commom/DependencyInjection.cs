using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snagged.Application.Abstractions;
using Snagged.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Snagged.Infrastructure.Commom
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<DatabaseContext>());

            return services;
        }
    }
}
