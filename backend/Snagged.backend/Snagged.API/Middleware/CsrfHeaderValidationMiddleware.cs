namespace Snagged.API.Middleware;

public class CsrfHeaderValidationMiddleware
{
    private const string CsrfHeaderName = "X-CSRF-TOKEN";

    private static readonly HashSet<string> SafeMethods =
        new(StringComparer.OrdinalIgnoreCase) { "GET", "HEAD", "OPTIONS", "TRACE" };

    private static readonly string[] ExcludedPathPrefixes =
    [
        "/api/auth/login",
        "/api/auth/register",
        "/api/stripe/webhook",
        "/swagger",
        "/images"
    ];

    private readonly RequestDelegate _next;
    private readonly ILogger<CsrfHeaderValidationMiddleware> _logger;

    public CsrfHeaderValidationMiddleware(
        RequestDelegate next,
        ILogger<CsrfHeaderValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        if (SafeMethods.Contains(request.Method))
        {
            await _next(context);
            return;
        }

        var path = request.Path.Value ?? string.Empty;
        if (ExcludedPathPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        var authHeader = request.Headers.Authorization.ToString();
        if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var csrfToken = request.Headers[CsrfHeaderName].ToString();
        if (string.IsNullOrWhiteSpace(csrfToken) || !IsValidTokenFormat(csrfToken))
        {
            _logger.LogWarning(
                "CSRF validation failed for {Method} {Path} from {IP}. Missing or invalid {Header} header.",
                request.Method,
                path,
                context.Connection.RemoteIpAddress,
                CsrfHeaderName);

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                error = "ForbiddenRequest",
                message = $"CSRF validation failed. The request is missing or has an invalid '{CsrfHeaderName}' header."
            });
            return;
        }

        await _next(context);
    }

    
    private static bool IsValidTokenFormat(string token)
        => token.Length >= 8 &&
          token.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '.');
}