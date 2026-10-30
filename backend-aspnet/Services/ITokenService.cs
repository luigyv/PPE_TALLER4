using JuegosApi.Models;

namespace JuegosApi.Services;

public interface ITokenService
{
    string CreateToken(Usuario usuario);
}
