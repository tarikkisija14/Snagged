using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Auth.Commands.Register;
using Snagged.Application.Catalog.Items.Queries.GetItems;
using Snagged.Application.Common.Behaviours;
using Snagged.Application.Common.Interfaces;
using Snagged.Application.Common.Helper;
using Snagged.Infrastructure;
using Snagged.Infrastructure.Commom;
using Snagged.Infrastructure.Database;
using Snagged.Infrastructure.Services;
using Stripe;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// DATABASE
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("Snagged.Infrastructure")
    )
);

builder.Services.AddScoped<IAppDbContext>(provider =>
    provider.GetRequiredService<DatabaseContext>());

builder.Services.AddScoped<IJwtService, JwtService>();

//CURRENTUSERSERVICE
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// MEDIATR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetItemsQueryHandler>();
    cfg.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,                      
        typeof(RegisterUserCommand).Assembly,          
        typeof(RegisterUserHandler).Assembly);
});

builder.Services.AddInfrastructure(builder.Configuration);

// AUTHENTICATION (JWT)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();


// RATE LIMITING
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        if (HttpMethods.IsOptions(context.Request.Method))
        {
            return RateLimitPartition.GetNoLimiter("cors-preflight");
        }

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

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        
    });
});

// CONTROLLERS and VALIDATION
builder.Services.AddControllers();

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssembly(Assembly.Load("Snagged.Application"));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Snagged API", Version = "v1" });

    // Add this block for JWT support
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// STRIPE
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<IStripeService, StripeService>();

var app = builder.Build();

// STATIC FILES
var imagesFolder = Path.Combine(
    app.Environment.ContentRootPath,
    builder.Configuration["ImageSettings:ItemsPath"] ?? ""
);

if (!Directory.Exists(imagesFolder))
    Directory.CreateDirectory(imagesFolder);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesFolder),
    RequestPath = "/images/items"
});

// PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAngular");      

app.UseRouting();

app.UseRateLimiter();

app.UseAuthentication();        
app.UseAuthorization();

app.MapControllers();

await app.Services.InitializeDatabaseAsync(app.Environment);


app.Run();
