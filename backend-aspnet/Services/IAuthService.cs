using JuegosApi.Dtos;

namespace JuegosApi.Services;

public interface IAuthService
{
    /// <summary>Crea el usuario. Devuelve null si el correo ya existe.</summary>
    Task<UsuarioDto?> RegisterAsync(RegisterDto dto);

    /// <summary>Devuelve el JWT, o null si las credenciales son incorrectas.</summary>
    Task<string?> LoginAsync(LoginDto dto);
}
