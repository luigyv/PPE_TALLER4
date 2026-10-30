using JuegosApi.Dtos;

namespace JuegosApi.Services;

public interface IJuegosService
{
    Task<PaginatedJuegosDto> FindAllAsync(string? search, int page, int limit);
    Task<JuegoDto?> FindOneAsync(int id);
    Task<JuegoDto> CreateAsync(CreateJuegoDto dto);
    Task<JuegoDto?> UpdateAsync(int id, UpdateJuegoDto dto);
    Task<bool> RemoveAsync(int id);
}
