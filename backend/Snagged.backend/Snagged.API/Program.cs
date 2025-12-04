using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Snagged.Application.Abstractions; // for IJwtService
using Snagged.Application.Catalog.Items.Queries.GetItems;
using Snagged.Infrastructure.Commom;
using Snagged.Infrastructure.Database;
using Snagged.Infrastructure.Services; //  for JwtService
using System.Net;
using System.Reflection.Emit;
using System.Threading.RateLimiting;
using Stripe;
using Snagged.Application.Commom.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext and connect to SQL Server
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Snagged.Infrastructure") // migrations go here
    )
);

//Register IAppDbContext so DI can resolve it in handlers
builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<DatabaseContext>());

//Register JwtService for IJwtService
builder.Services.AddScoped<IJwtService, JwtService>();



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
            message = "Previ�e zahtjeva, poku�ajte kasnije.",
            timestamp = DateTime.UtcNow
        }, cancellationToken: token);
    };
});


// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

// register services
builder.Services.AddScoped<IStripeService, StripeService>();


var app = builder.Build();


var projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
var imagesFolder = Path.Combine(projectRoot, builder.Configuration["ImageSettings:ItemsPath"]);

if (!Directory.Exists(imagesFolder))
    Directory.CreateDirectory(imagesFolder);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesFolder),
    RequestPath = "/images/items"
});

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
