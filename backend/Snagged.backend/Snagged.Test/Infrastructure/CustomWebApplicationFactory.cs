using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Snagged.Application.Abstractions;
using Snagged.Test.Infrastructure;
using System.Linq;

namespace Snagged.Test.Infrastructure
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Ukloni postojeći IAppDbContext ako postoji
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAppDbContext));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Registriraj InMemoryAppDbContext
                services.AddSingleton<IAppDbContext, InMemoryAppDbContext>();

                // Seed test podatke
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
                TestDataSeeder.SeedAsync(ctx).GetAwaiter().GetResult();
            });
        }
    }
}
