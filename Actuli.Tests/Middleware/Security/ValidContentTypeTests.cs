namespace Actuli.Tests.Middleware.Security;

using System;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using Actuli.Api.Middleware;

public class ValidContentTypeTests
{
    [Theory]
    [InlineData("POST", "application/json", true)] // Valid content type
    [InlineData("PUT", "application/json; charset=utf-8", true)] // Valid with charset
    [InlineData("PATCH", "application/json", true)] // Valid content type
    [InlineData("POST", "text/plain", false)] // Invalid content type
    [InlineData("PUT", null, false)] // Missing content type
    [InlineData("GET", null, true)] // GET request, no validation required
    [InlineData("DELETE", null, true)] // DELETE request, no validation required
    public void IsValidContentType_ShouldValidateCorrectly(string method, string contentType, bool expected)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.ContentType = contentType;

        var middleware = new SecurityMiddleware(null);

        // Act
        var result = InvokeIsValidContentType(middleware, context);

        // Assert
        Assert.Equal(expected, result);
    }

    // Private helper method to invoke the protected IsValidContentType method
    private bool InvokeIsValidContentType(SecurityMiddleware middleware, HttpContext context)
    {
        var method = typeof(SecurityMiddleware).GetMethod("IsValidContentType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)method.Invoke(middleware, new object[] { context });
    }
}