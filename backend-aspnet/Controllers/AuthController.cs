using JuegosApi.Dtos;
using JuegosApi.Infrastructure;
using JuegosApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace JuegosApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService auth) : ControllerBase
{
    // POST /auth/register { email, password } -> 201 { id, email } / 400 / 409
    [HttpPost("register")]
    public async Task<ActionResult<UsuarioDto>> Register([FromBody] RegisterDto dto)
    {
        var usuario = await auth.RegisterAsync(dto);
        if (usuario is null) return NestError.Conflict("El correo ya está registrado");

        return StatusCode(StatusCodes.Status201Created, usuario);
    }

    // POST /auth/login { email, password } -> 200 { access_token } / 401
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
    {
        var token = await auth.LoginAsync(dto);
        if (token is null) return NestError.Unauthorized("Credenciales inválidas");

        return Ok(new LoginResponseDto(token));
    }
}
