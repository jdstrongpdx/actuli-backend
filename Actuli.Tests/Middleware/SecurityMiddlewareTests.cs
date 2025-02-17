using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;
using Actuli.Api.Middleware;

namespace Actuli.Api.Tests.Middleware;

public class SecurityMiddlewareTests
{
    private DefaultHttpContext CreateHttpContext(string method = "GET", string contentType = null,
        string body = null)
    {
        var context = new DefaultHttpContext();

        // Set HTTP method
        context.Request.Method = method;

        // Set Content-Type if applicable
        if (!string.IsNullOrEmpty(contentType))
        {
            context.Request.ContentType = contentType;
        }

        // Set body if applicable
        if (!string.IsNullOrEmpty(body))
        {
            var bodyBytes = Encoding.UTF8.GetBytes(body);
            context.Request.Body = new MemoryStream(bodyBytes);
            context.Request.ContentLength = bodyBytes.Length;
        }

        return context;
    }

    private RequestDelegate CreateNextMiddleware() =>
        (HttpContext context) =>
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            return Task.CompletedTask;
        };
    

    [Fact]
    public async Task InvokeAsync_ShouldAllowRequests_WithValidContentType()
    {
        // Arrange
        var middleware = new SecurityMiddleware(CreateNextMiddleware());
        var context = CreateHttpContext(method: "POST", contentType: "application/json");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldAddSecurityHeaders()
    {
        // Arrange
        var middleware = new SecurityMiddleware(CreateNextMiddleware());
        var context = CreateHttpContext();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(context.Response.Headers.ContainsKey("X-Content-Type-Options"));
        Assert.True(context.Response.Headers.ContainsKey("X-XSS-Protection"));
        Assert.True(context.Response.Headers.ContainsKey("Content-Security-Policy"));
        Assert.True(context.Response.Headers.ContainsKey("X-Frame-Options"));

        Assert.Equal("nosniff", context.Response.Headers["X-Content-Type-Options"]);
        Assert.Equal("1; mode=block", context.Response.Headers["X-XSS-Protection"]);
        Assert.Equal("default-src 'self';", context.Response.Headers["Content-Security-Policy"]);
        Assert.Equal("DENY", context.Response.Headers["X-Frame-Options"]);
    }

    [Theory]
    [InlineData("application/json", true)]
    [InlineData("application/json; charset=utf-8", true)]
    [InlineData("text/plain", false)]
    [InlineData(null, false)]
    public void IsValidContentType_ShouldValidateProperly(string contentType, bool expected)
    {
        // Arrange
        var context = CreateHttpContext(method: "POST", contentType: contentType);
        var middleware = new SecurityMiddleware(CreateNextMiddleware());

        // Act
        var result = middleware.GetType()
            .GetMethod("IsValidContentType",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(middleware, new[] { context });

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("SELECT * FROM Users", true)]
    [InlineData("<script>alert('XSS')</script>", false)]
    [InlineData("", false)]
    public void HasSqlInjectionPatterns_ShouldValidateProperly(string queryValue, bool expected)
    {
        // Arrange
        var context = CreateHttpContext();
        context.Request.Query = new QueryCollection(
            new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "test", queryValue }
            });
        var middleware = new SecurityMiddleware(CreateNextMiddleware());

        // Act
        var result = middleware.GetType()
            .GetMethod("HasSqlInjectionPatterns",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(middleware, new[] { context });

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("<script>alert('XSS')</script>", true)]
    [InlineData("SELECT * FROM Users", false)]
    [InlineData("", false)]
    public void HasXssPatterns_ShouldValidateProperly(string queryValue, bool expected)
    {
        // Arrange
        var context = CreateHttpContext();
        context.Request.Query = new QueryCollection(
            new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "test", queryValue }
            });
        var middleware = new SecurityMiddleware(CreateNextMiddleware());

        // Act
        var result = middleware.GetType()
            .GetMethod("HasXssPatterns",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(middleware, new[] { context });

        // Assert
        Assert.Equal(expected, result);
    }
}
