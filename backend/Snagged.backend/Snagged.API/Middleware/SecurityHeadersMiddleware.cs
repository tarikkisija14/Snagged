namespace Snagged.API.Middleware;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public SecurityHeadersMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            var headers = context.Response.Headers;

            headers["X-Content-Type-Options"] = "nosniff";
            headers["X-Frame-Options"] = "DENY";
            headers["X-XSS-Protection"] = "1; mode=block";
            headers["Referrer-Policy"] = "no-referrer";
            headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), payment=()";

            if (_env.IsDevelopment())
            {
                headers["Content-Security-Policy"] =
                    "default-src 'self'; " +
                    "script-src 'self' 'unsafe-inline' https://js.stripe.com; " +
                    "style-src 'self' 'unsafe-inline'; " +
                    "img-src 'self' data:; " +
                    "frame-src https://js.stripe.com; " +
                    "connect-src 'self' ws: wss: http://localhost:* https://localhost:* https://api.stripe.com;";
            }
            else
            {
               
                headers["Content-Security-Policy"] =
                    "default-src 'self'; " +
                    "script-src 'self' https://js.stripe.com; " +
                    "style-src 'self' 'unsafe-inline'; " +
                    "img-src 'self' data:; " +
                    "frame-src https://js.stripe.com; " +
                    "connect-src 'self' https://api.stripe.com;";
            }

            headers.Remove("Server");
            headers.Remove("X-Powered-By");

            return Task.CompletedTask;
        });

        await _next(context);
    }
}