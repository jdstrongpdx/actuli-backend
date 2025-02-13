namespace Actuli.Tests.Middleware.Security;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Xunit;
using Actuli.Api.Middleware;

public class SqlInjectionPatternTests
{
    [Theory]
    [MemberData(nameof(GetSqlInjectionKeywords))]
    public void HasSqlInjectionPatterns_ShouldDetectAllSqlInjectionKeywordsInQuery(string keyword)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Query = new QueryCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "test", keyword }
        });

        var middleware = new SecurityMiddleware(null);

        // Act
        var result = InvokeHasSqlInjectionPatterns(middleware, context);

        // Assert
        Assert.True(result, $"Keyword `{keyword}` was not detected as an SQL injection pattern in the query.");
    }

    [Theory]
    [MemberData(nameof(GetSqlInjectionKeywords))]
    public void HasSqlInjectionPatterns_ShouldDetectAllSqlInjectionKeywordsInHeaders(string keyword)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers.Add("Custom-Header", keyword);

        var middleware = new SecurityMiddleware(null);

        // Act
        var result = InvokeHasSqlInjectionPatterns(middleware, context);

        // Assert
        Assert.True(result, $"Keyword `{keyword}` was not detected as an SQL injection pattern in the header.");
    }

    private bool InvokeHasSqlInjectionPatterns(SecurityMiddleware middleware, HttpContext context)
    {
        var method = typeof(SecurityMiddleware).GetMethod("HasSqlInjectionPatterns",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)method.Invoke(middleware, new object[] { context });
    }

    // Test data: SQL injection keywords
    public static IEnumerable<object[]> GetSqlInjectionKeywords()
    {
        // These should match the keywords defined in SecurityMiddleware
        var sqlInjectionKeywords = new[]
        {
            "SELECT", "DROP", "INSERT", "UPDATE", "DELETE",
            "--", ";--", ";", "/*", "*/", "@@", "@", "CHAR(",
            "NCHAR(", "VARCHAR(", "NVARCHAR(", "ALTER", "BEGIN",
            "CAST(", "CREATE", "CURSOR", "DECLARE", "EXEC",
            "FETCH", "SET", "SHUTDOWN", "TRUNCATE", "UNION", "WAITFOR"
        };

        foreach (var keyword in sqlInjectionKeywords)
        {
            yield return new object[] { keyword };
        }
    }
}