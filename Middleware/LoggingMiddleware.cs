using System.Diagnostics;

namespace UseManagementAPI.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        Console.WriteLine(
            $"[{DateTime.UtcNow}] " +
            $"Method: {context.Request.Method} | " +
            $"Path: {context.Request.Path} | " +
            $"StatusCode: {context.Response.StatusCode} | " +
            $"Duration: {stopwatch.ElapsedMilliseconds} ms");
    }
}