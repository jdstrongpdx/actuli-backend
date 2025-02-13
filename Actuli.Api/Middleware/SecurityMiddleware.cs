using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Actuli.Api.Middleware;

public class SecurityMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Ensure Content-Type is application/json for all requests with a body
        if (!IsValidContentType(context))
        {
            context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            await context.Response.WriteAsync("Unsupported Media Type. Only 'application/json' is allowed.");
            return;
        }

        // Protect against SQL Injection
        if (HasSqlInjectionPatterns(context))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Bad Request: Possible SQL Injection detected.");
            return;
        }

        // Protect against XSS attacks
        if (HasXssPatterns(context))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Bad Request: Possible XSS attack detected.");
            return;
        }

        // Add security headers
        AddSecurityHeaders(context);

        // Continue to the next middleware
        await _next(context);
    }

    private bool IsValidContentType(HttpContext context)
    {
        // Check only if the request has a body (e.g., POST, PUT, etc.)
        if (context.Request.Method == HttpMethods.Post ||
            context.Request.Method == HttpMethods.Put ||
            context.Request.Method == HttpMethods.Patch)
        {
            // Get the Content-Type header
            var contentType = context.Request.ContentType;

            // Ensure it is 'application/json'
            return contentType != null &&
                   contentType.StartsWith("application/json", System.StringComparison.OrdinalIgnoreCase);
        }

        return true; // Other HTTP methods (e.g., GET, DELETE) don't require a Content-Type header
    }

    private bool HasSqlInjectionPatterns(HttpContext context)
    {
        string[] sqlInjectionKeywords = new[]
        {
            "SELECT", "DROP", "INSERT", "UPDATE", "DELETE",
            "--", ";--", ";", "/*", "*/", "@@", "@", "CHAR(",
            "NCHAR(", "VARCHAR(", "NVARCHAR(", "ALTER", "BEGIN",
            "CAST(", "CREATE", "CURSOR", "DECLARE", "EXEC",
            "FETCH", "SET", "SHUTDOWN", "TRUNCATE", "UNION", "WAITFOR"
        };

        // Escape the keywords to handle regex special characters
        var escapedKeywords = sqlInjectionKeywords
            .Select(kw => System.Text.RegularExpressions.Regex.Escape(kw))
            .ToArray();

        foreach (var query in context.Request.Query)
        {
            foreach (var keyword in escapedKeywords)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(query.Value, keyword,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
        }

        foreach (var header in context.Request.Headers)
        {
            foreach (var keyword in escapedKeywords)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(header.Value, keyword,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool HasXssPatterns(HttpContext context)
    {
        string[] xssPatterns = new[]
        {
            "<script>", "<iframe>", "javascript:", "onerror=", "onload=",
        };

        foreach (var query in context.Request.Query)
        {
            foreach (var pattern in xssPatterns)
            {
                if (query.Value.ToString().IndexOf(pattern, System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
        }

        foreach (var header in context.Request.Headers)
        {
            foreach (var pattern in xssPatterns)
            {
                if (header.Value.ToString().IndexOf(pattern, System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void AddSecurityHeaders(HttpContext context)
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff"; // Prevent MIME type sniffing
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block"; // Cross-Site Scripting protection
        context.Response.Headers["Content-Security-Policy"] = "default-src 'self';"; // Limit external resource loading
        context.Response.Headers["X-Frame-Options"] = "DENY"; // Prevent clickjacking by disallowing iframes
    }
}