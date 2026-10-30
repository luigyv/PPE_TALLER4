using JuegosApi.Data;
using JuegosApi.Dtos;
using JuegosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JuegosApi.Services;

public class AuthService(AppDbContext db, ITokenService tokens) : IAuthService
{
    public async Task<UsuarioDto?> RegisterAsync(RegisterDto dto)
    {
        var email = Normalizar(dto.Email);

        if (await db.Usuarios.AnyAsync(u => u.Email == email))
            return null;

        var usuario = new Usuario
        {
            Email = email,
            // BCrypt genera la sal y la guarda dentro del propio hash.
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreadoEn = DateTime.UtcNow
        };

        db.Usuarios.Add(usuario);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Carrera: otro registro con el mismo correo entró entre el AnyAsync y el Save.
            return null;
        }

        return new UsuarioDto(usuario.Id, usuario.Email);
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var email = Normalizar(dto.Email);
        var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            return null;

        return tokens.CreateToken(usuario);
    }

    private static string Normalizar(string email) => email.Trim().ToLowerInvariant();
}
