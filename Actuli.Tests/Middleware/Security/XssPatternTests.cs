namespace Actuli.Tests.Middleware.Security;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Xunit;
using Actuli.Api.Middleware;

public class XssPatternTests
{
    [Theory]
    [MemberData(nameof(GetXssPatterns))]
    public void HasXssPatterns_ShouldDetectAllXssPatternsInQuery(string xssPattern)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Query = new QueryCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "test", xssPattern }
        });

        var middleware = new SecurityMiddleware(null);

        // Act
        var result = InvokeHasXssPatterns(middleware, context);

        // Assert
        Assert.True(result, $"Pattern `{xssPattern}` was not detected as an XSS attack in the query.");
    }

    [Theory]
    [MemberData(nameof(GetXssPatterns))]
    public void HasXssPatterns_ShouldDetectAllXssPatternsInHeaders(string xssPattern)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers.Add("Custom-Header", xssPattern);

        var middleware = new SecurityMiddleware(null);

        // Act
        var result = InvokeHasXssPatterns(middleware, context);

        // Assert
        Assert.True(result, $"Pattern `{xssPattern}` was not detected as an XSS attack in the header.");
    }

    private bool InvokeHasXssPatterns(SecurityMiddleware middleware, HttpContext context)
    {
        var method = typeof(SecurityMiddleware).GetMethod("HasXssPatterns",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)method.Invoke(middleware, new object[] { context });
    }

    public static IEnumerable<object[]> GetXssPatterns()
    {
        // Provide the XSS patterns from the method
        var xssPatterns = new[]
        {
            "<script>", "<iframe>", "javascript:", "onerror=", "onload="
        };

        foreach (var pattern in xssPatterns)
        {
            yield return new object[] { pattern };
        }
    }
}