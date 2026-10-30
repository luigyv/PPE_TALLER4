using JuegosApi.Data;
using JuegosApi.Dtos;
using JuegosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JuegosApi.Services;

public class JuegosService(AppDbContext db) : IJuegosService
{
    // GET /juegos?search=&page=1&limit=10  ->  { data, total, page, limit }
    public async Task<PaginatedJuegosDto> FindAllAsync(string? search, int page, int limit)
    {
        page = Math.Max(1, page);
        limit = Math.Clamp(limit, 1, 100);

        var query = db.Juegos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            // LIKE en SQLite no distingue mayúsculas/minúsculas (igual que "contains" en Prisma).
            var pattern = $"%{EscapeLike(search.Trim())}%";
            query = query.Where(j => EF.Functions.Like(j.Nombre, pattern, "\\"));
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(j => j.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return new PaginatedJuegosDto(items.Select(ToDto).ToList(), total, page, limit);
    }

    public async Task<JuegoDto?> FindOneAsync(int id)
    {
        var juego = await db.Juegos.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id);
        return juego is null ? null : ToDto(juego);
    }

    public async Task<JuegoDto> CreateAsync(CreateJuegoDto dto)
    {
        var juego = new Juego
        {
            Nombre = dto.Nombre,
            Imagen = dto.Imagen,
            Categoria = dto.Categoria,
            CreadoEn = DateTime.UtcNow
        };

        db.Juegos.Add(juego);
        await db.SaveChangesAsync();
        return ToDto(juego);
    }

    public async Task<JuegoDto?> UpdateAsync(int id, UpdateJuegoDto dto)
    {
        var juego = await db.Juegos.FirstOrDefaultAsync(j => j.Id == id);
        if (juego is null) return null;

        if (dto.Nombre is not null) juego.Nombre = dto.Nombre;
        if (dto.Imagen is not null) juego.Imagen = dto.Imagen;
        if (dto.Categoria is not null) juego.Categoria = dto.Categoria;

        await db.SaveChangesAsync();
        return ToDto(juego);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var juego = await db.Juegos.FirstOrDefaultAsync(j => j.Id == id);
        if (juego is null) return false;

        db.Juegos.Remove(juego);
        await db.SaveChangesAsync();
        return true;
    }

    // SQLite devuelve DateTime sin "Kind"; lo marcamos UTC para que el JSON termine en "Z".
    private static JuegoDto ToDto(Juego j) =>
        new(j.Id, j.Nombre, j.Imagen, j.Categoria, DateTime.SpecifyKind(j.CreadoEn, DateTimeKind.Utc));

    private static string EscapeLike(string s) =>
        s.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");
}
