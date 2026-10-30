namespace JuegosApi.Models;

// Mismo modelo que Prisma: Juego { id, nombre, imagen, categoria, creadoEn }
public class Juego
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Imagen { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime CreadoEn { get; set; }
}
