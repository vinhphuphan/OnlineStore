// ExceptionMiddleware handles all unhandled exceptions during HTTP request processing.
// It returns a standardized JSON error response depending on the environment.

using System.Net;
using System.Text.Json;
using API.Errors;


namespace API.Middlewares;

public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, env);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = env.IsDevelopment()
                    ? new ApiErrorReponse(context.Response.StatusCode, ex.Message, ex.StackTrace)
                    : new ApiErrorReponse(context.Response.StatusCode, "Internal Server Error", null);

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        return context.Response.WriteAsync(json);
    }
}

