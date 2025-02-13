namespace Actuli.Tests.Middleware.Security;

using Microsoft.AspNetCore.Http;
using Xunit;
using Actuli.Api.Middleware;

public class AddSecurityHeadersTests
{
    [Fact]
    public void AddSecurityHeaders_ShouldAddAllExpectedHeaders()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var middleware = new SecurityMiddleware(null);

        // Act
        InvokeAddSecurityHeaders(middleware, context);

        // Assert
        Assert.Equal("nosniff", context.Response.Headers["X-Content-Type-Options"]);
        Assert.Equal("1; mode=block", context.Response.Headers["X-XSS-Protection"]);
        Assert.Equal("default-src 'self';", context.Response.Headers["Content-Security-Policy"]);
        Assert.Equal("DENY", context.Response.Headers["X-Frame-Options"]);
    }

    private void InvokeAddSecurityHeaders(SecurityMiddleware middleware, HttpContext context)
    {
        var method = typeof(SecurityMiddleware).GetMethod("AddSecurityHeaders",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(middleware, new object[] { context });
    }
}