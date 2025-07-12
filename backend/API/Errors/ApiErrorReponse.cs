// ApiErrorResponse represents the standard structure for API error messages.
// Includes status code, user-friendly message, and optional error details for debugging.

namespace API.Errors;

public class ApiErrorReponse(int statusCode, string message, string? details)
{
    public int StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = message;
    public string? Details { get; set; } = details;
}
