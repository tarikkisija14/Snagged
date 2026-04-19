using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Snagged.API.Middleware;

public class XssSanitizationMiddleware
{
    private const int MaxBodyBytes = 10 * 1024 * 1024; // 10 MB guard

    private static readonly string[] SanitizableContentTypes =
        ["application/json", "text/json"];

    private readonly RequestDelegate _next;
    private readonly ILogger<XssSanitizationMiddleware> _logger;

    public XssSanitizationMiddleware(RequestDelegate next, ILogger<XssSanitizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (HasJsonBody(context.Request))
        {
            context.Request.EnableBuffering();

            try
            {
                if (context.Request.ContentLength is > MaxBodyBytes)
                {
                    _logger.LogWarning(
                        "XSS sanitization skipped for {Path}: body size {Size} exceeds limit.",
                        context.Request.Path,
                        context.Request.ContentLength);
                }
                else
                {
                    var sanitizedBody = await SanitizeRequestBodyAsync(context.Request);

                    if (sanitizedBody is not null)
                    {
                        var bytes = Encoding.UTF8.GetBytes(sanitizedBody);
                        context.Request.Body = new MemoryStream(bytes);
                        context.Request.ContentLength = bytes.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "XSS sanitization failed for {Path}; passing body through unchanged.", context.Request.Path);
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }
        }

        await _next(context);
    }

    private static bool HasJsonBody(HttpRequest request)
    {
        if (request.ContentLength is 0 || request.Body == Stream.Null)
            return false;

        if (string.IsNullOrEmpty(request.ContentType))
            return false;

        return SanitizableContentTypes.Any(ct =>
            request.ContentType.Contains(ct, StringComparison.OrdinalIgnoreCase));
    }

    private static async Task<string?> SanitizeRequestBodyAsync(HttpRequest request)
    {
        request.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var rawBody = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);

        if (string.IsNullOrWhiteSpace(rawBody))
            return null;

        using var doc = JsonDocument.Parse(rawBody);
        var sanitized = SanitizeElement(doc.RootElement);

        return JsonSerializer.Serialize(sanitized);
    }

    private static object? SanitizeElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => SanitizeObject(element),
            JsonValueKind.Array => SanitizeArray(element),
            JsonValueKind.String => SanitizeString(element.GetString()),
            JsonValueKind.Number => element.TryGetInt64(out var l) ? (object)l : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => null
        };
    }

    private static Dictionary<string, object?> SanitizeObject(JsonElement element)
    {
        var result = new Dictionary<string, object?>();
        foreach (var property in element.EnumerateObject())
            result[property.Name] = SanitizeElement(property.Value);
        return result;
    }

    private static List<object?> SanitizeArray(JsonElement element)
        => element.EnumerateArray().Select(SanitizeElement).ToList();

    private static string? SanitizeString(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        // Remove <script>…</script> blocks and their content
        value = Regex.Replace(value,
            @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>",
            string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // Remove event handler attributes (onclick, onload, onerror, …)
        value = Regex.Replace(value,
            @"\bon\w+\s*=\s*(""[^""]*""|'[^']*'|[^\s>]*)",
            string.Empty, RegexOptions.IgnoreCase);

        // Remove javascript: URI scheme
        value = Regex.Replace(value, @"javascript\s*:", string.Empty, RegexOptions.IgnoreCase);

        // Remove vbscript: URI scheme
        value = Regex.Replace(value, @"vbscript\s*:", string.Empty, RegexOptions.IgnoreCase);

        // Strip remaining HTML tags
        value = Regex.Replace(value, @"<[^>]+>", string.Empty);

        // Encode residual angle brackets as a safety net
        value = value.Replace("<", "&lt;").Replace(">", "&gt;");

        return value;
    }
}