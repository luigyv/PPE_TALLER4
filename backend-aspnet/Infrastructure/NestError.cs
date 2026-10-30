using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace JuegosApi.Infrastructure;

// Cuerpo de error idéntico al de NestJS: { message, error, statusCode }
public record ErrorResponse(
    object Message,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Error,
    int StatusCode);

public static class NestError
{
    public static ObjectResult Make(int status, object message, string? error) =>
        new(new ErrorResponse(message, error, status)) { StatusCode = status };

    public static ObjectResult NotFound(string message) => Make(404, message, "Not Found");
    public static ObjectResult Conflict(string message) => Make(409, message, "Conflict");
    public static ObjectResult Unauthorized(string message) => Make(401, message, "Unauthorized");
}
