using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snagged.Application.Catalog.Items.Queries.GetItems;
using Snagged.Domain.Entities;
using Snagged.Infrastructure.Commom;
using Snagged.Infrastructure.Database;
using System.Reflection.Emit;
using System.Threading.RateLimiting;
using System.Net;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext and connect to SQL Server
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Snagged.Infrastructure") // migrations go here
    )
);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetItemsQueryHandler>();
});

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddRateLimiter(options =>
{

options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
{
    
    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

    return RateLimitPartition.GetSlidingWindowLimiter(
        partitionKey: ip,
        factory: _ => new SlidingWindowRateLimiterOptions
        {
            PermitLimit = 100, 
            Window = TimeSpan.FromMinutes(1), 
            SegmentsPerWindow = 5, 
            QueueLimit = 0,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });
});

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "TooManyRequests",
            message = "Previše zahtjeva, pokušajte kasnije.",
            timestamp = DateTime.UtcNow
        }, cancellationToken: token);
    };
});


// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();

app.Run();
