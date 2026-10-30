using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JuegosApi.Dtos;

public class RegisterDto
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no es válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;
}

public class LoginDto
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; } = string.Empty;
}

public record UsuarioDto(int Id, string Email);

// El cliente Vue lee exactamente "access_token" (stores/auth.js), por eso va en snake_case.
public record LoginResponseDto([property: JsonPropertyName("access_token")] string AccessToken);
