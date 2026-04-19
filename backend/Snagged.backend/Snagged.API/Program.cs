using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Snagged.API.Extensions;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Auth.Commands.Register;
using Snagged.Application.Catalog.Items.Queries.GetItems;
using Snagged.Application.Common.Behaviours;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Helper;
using Snagged.Application.Common.Interfaces;
using Snagged.Infrastructure.Commom;
using Snagged.Infrastructure.Database;
using Snagged.Infrastructure.Services;
using Stripe;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;



var builder = WebApplication.CreateBuilder(args);

// DATABASE
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IJwtService, JwtService>();

// CURRENT USER SERVICE
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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// RATE LIMITING
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        if (HttpMethods.IsOptions(context.Request.Method))
            return RateLimitPartition.GetNoLimiter("cors-preflight");

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
            message = "Too many requests. Please try again later.",
            timestamp = DateTime.UtcNow
        }, cancellationToken: token);
    };
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy
            .WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// CONTROLLERS + VALIDATION
builder.Services.AddControllers();

// FluentValidation
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssembly(Assembly.Load("Snagged.Application"));

// MediatR pipeline validation behaviour
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Snagged API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "CSRF zaštita. Dodaj header: X-CSRF-TOKEN: {tvoj-token}",
        Name = "X-CSRF-TOKEN",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                           Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "CSRF" }
            },
            Array.Empty<string>()
        }
    });
});

// STRIPE
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<IStripeService, StripeService>();

var app = builder.Build();


app.UseExceptionHandler(errApp => errApp.Run(async ctx =>
{
    var ex = ctx.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

    ctx.Response.ContentType = "application/json";

    if (ex is ValidationException ve)
    {
        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        var errors = ve.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
        await ctx.Response.WriteAsJsonAsync(new { errors });
        return;
    }

    if (ex is SnaggedConflictException sce)
    {
        ctx.Response.StatusCode = StatusCodes.Status409Conflict;
        await ctx.Response.WriteAsJsonAsync(new { message = sce.Message });
        return;
    }

    if (ex is SnaggedBusinessRuleException sbe)
    {
        ctx.Response.StatusCode = StatusCodes.Status409Conflict;
        await ctx.Response.WriteAsJsonAsync(new { code = sbe.Code, message = sbe.Message });
        return;
    }

    if (ex is SnaggedNotFoundException snfe)
    {
        ctx.Response.StatusCode = StatusCodes.Status404NotFound;
        await ctx.Response.WriteAsJsonAsync(new { message = snfe.Message });
        return;
    }

    if (ex is UnauthorizedAccessException)
    {
        ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
        await ctx.Response.WriteAsJsonAsync(new { message = "Forbidden." });
        return;
    }

    ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await ctx.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred." });
}));

// ── Security headers (applies to all responses, including errors)
app.UseSecurityHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Serve item images from the configured folder
var imagesFolder = Path.Combine(
    app.Environment.ContentRootPath,
    builder.Configuration["ImageSettings:ItemsPath"] ?? "images/items"
);

if (!Directory.Exists(imagesFolder))
    Directory.CreateDirectory(imagesFolder);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesFolder),
    RequestPath = "/images/items"
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAngular");
app.UseRateLimiter();

// ── XSS body sanitization (before controllers read the body)
app.UseXssSanitization();

app.UseAuthentication();
app.UseAuthorization();

// ── CSRF header validation (after auth so we can inspect the JWT presence)
// app.UseCsrfHeaderValidation(); // ukljucit na kraju

app.MapControllers();

await app.Services.InitializeDatabaseAsync(app.Environment);

app.Run();