using System.Text.Json;

namespace UseManagementAPI.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    private const string VALID_TOKEN =
        "my-secret-token";

    public AuthenticationMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        var authHeader =
            context.Request.Headers["Authorization"]
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authHeader))
        {
            context.Response.StatusCode = 401;

            await context.Response.WriteAsJsonAsync(
                new
                {
                    Success = false,
                    Message = "Unauthorized."
                });

            return;
        }

        if (!authHeader.StartsWith("Bearer "))
        {
            context.Response.StatusCode = 401;

            await context.Response.WriteAsJsonAsync(
                new
                {
                    Success = false,
                    Message = "Unauthorized."
                });

            return;
        }

        var token = authHeader.Replace(
            "Bearer ",
            "");

        if (token != VALID_TOKEN)
        {
            context.Response.StatusCode = 401;

            await context.Response.WriteAsJsonAsync(
                new
                {
                    Success = false,
                    Message = "Unauthorized."
                });

            return;
        }

        await _next(context);
    }
}