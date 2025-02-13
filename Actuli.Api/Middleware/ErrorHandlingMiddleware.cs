namespace Actuli.Api.Middleware;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        // Handle specific exceptions with appropriate status codes
        catch (ValidationException ex)
        {
            await HandleException(context, StatusCodes.Status400BadRequest, "Validation Error", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleException(context, StatusCodes.Status401Unauthorized, "Unauthorized Access", ex);
        }
        catch (KeyNotFoundException ex)
        {
            await HandleException(context, StatusCodes.Status404NotFound, "Resource Not Found", ex);
        }
        catch (InvalidOperationException ex)
        {
            await HandleException(context, StatusCodes.Status409Conflict, "Invalid Operation", ex);
        }
        catch (NotImplementedException ex)
        {
            await HandleException(context, StatusCodes.Status501NotImplemented, "Feature Not Implemented", ex);
        }
        // Catch-all for unhandled exceptions
        catch (Exception ex)
        {
            await HandleException(context, StatusCodes.Status500InternalServerError, "An Unhandled Error Occurred", ex);
        }
    }

    private Task HandleException(HttpContext context, int statusCode, string title, Exception ex)
    {
        // Log the exception
        _logger.LogError(ex, "An exception was caught by the error handling middleware.");

        // Construct a detailed response for development
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = _env.IsDevelopment() ? ex.ToString() : null,
            Instance = context.TraceIdentifier // Include the unique request ID for tracking
        };

        // In production, remove sensitive "Detail" information
        if (!_env.IsDevelopment())
        {
            // Do not set "Detail" in production.
            problemDetails.Detail = null;
        }


        // Set the response details
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        // Return the error in JSON format
        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}