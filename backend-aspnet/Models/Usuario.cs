namespace JuegosApi.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;   // único, en minúsculas
    public string PasswordHash { get; set; } = string.Empty; // BCrypt, nunca la contraseña en claro
    public DateTime CreadoEn { get; set; }
}
