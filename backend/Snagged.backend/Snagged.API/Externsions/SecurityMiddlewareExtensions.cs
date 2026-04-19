using Snagged.API.Middleware;

namespace Snagged.API.Extensions;

public static class SecurityMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
        => app.UseMiddleware<SecurityHeadersMiddleware>();

    public static IApplicationBuilder UseXssSanitization(this IApplicationBuilder app)
        => app.UseMiddleware<XssSanitizationMiddleware>();

    public static IApplicationBuilder UseCsrfHeaderValidation(this IApplicationBuilder app)
        => app.UseMiddleware<CsrfHeaderValidationMiddleware>();
}