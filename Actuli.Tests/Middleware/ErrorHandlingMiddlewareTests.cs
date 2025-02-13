using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Actuli.Tests.Middleware;

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Actuli.Api.Middleware;

public class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async Task Middleware_ShouldReturn400_ForValidationException()
    {
        // Arrange
        var context = CreateHttpContext();
        var middleware = CreateMiddleware(context => throw new ValidationException("Invalid input"));

        // Act
        await middleware.Invoke(context);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        AssertResponseContains(context, "Validation Error");
    }

    [Fact]
    public async Task Middleware_ShouldReturn401_ForUnauthorizedAccessException()
    {
        // Arrange
        var context = CreateHttpContext();
        var middleware = CreateMiddleware(context => { throw new UnauthorizedAccessException("Unauthorized access"); });

        // Act
        await middleware.Invoke(context);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        AssertResponseContains(context, "Unauthorized Access");
    }

    [Fact]
    public async Task Middleware_ShouldReturn404_ForKeyNotFoundException()
    {
        // Arrange
        var context = CreateHttpContext();
        var middleware = CreateMiddleware(context => { throw new KeyNotFoundException("Resource not found"); });

        // Act
        await middleware.Invoke(context);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
        AssertResponseContains(context, "Resource Not Found");
    }

    [Fact]
    public async Task Middleware_ShouldReturn500_ForUnhandledException()
    {
        // Arrange
        var context = CreateHttpContext();
        var middleware = CreateMiddleware( context => { throw new Exception("Something went wrong"); });

        // Act
        await middleware.Invoke(context);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        AssertResponseContains(context, "An Unhandled Error Occurred");
    }

    [Fact]
    public async Task Middleware_ShouldReturnDetailedError_WhenInDevelopmentMode()
    {
        // Arrange
        var context = CreateHttpContext();
        var middleware = CreateMiddleware(context => { throw new Exception("Something went wrong"); }, isDevelopment: true);

        // Act
        await middleware.Invoke(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var json = await JsonDocument.ParseAsync(context.Response.Body);

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal("An Unhandled Error Occurred", json.RootElement.GetProperty("title").GetString());
        Assert.NotNull(json.RootElement.GetProperty("detail").GetString()); // Detailed exception info
    }

    [Fact]
    public async Task Middleware_ShouldReturnGenericError_WhenInProductionMode()
    {
        // Arrange
        var middleware = CreateMiddleware(next: null, isDevelopment: false); // Production mode
        var context = CreateHttpContext();

        // Act
        await middleware.Invoke(context);

        // Assert
        AssertResponseContains(context, "An error occurred. Please contact support.");
    }

    private static ErrorHandlingMiddleware CreateMiddleware(RequestDelegate next, bool isDevelopment = false)
    {
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var envMock = new Mock<IWebHostEnvironment>();

        // Set the environment name based on whether it's development or not
        envMock.Setup(env => env.EnvironmentName)
            .Returns(isDevelopment ? "Development" : "Production");

        return new ErrorHandlingMiddleware(next, loggerMock.Object, envMock.Object);
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async void AssertResponseContains(HttpContext context, string expectedContent)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        Assert.Contains(expectedContent, responseBody);
    }
}