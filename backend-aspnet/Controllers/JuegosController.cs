using JuegosApi.Dtos;
using JuegosApi.Infrastructure;
using JuegosApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuegosApi.Controllers;

[ApiController]
[Route("juegos")]
public class JuegosController(IJuegosService juegos) : ControllerBase
{
    // GET /juegos?search=&page=1&limit=10  -> pública, 200
    [HttpGet]
    public async Task<ActionResult<PaginatedJuegosDto>> GetAll(
        [FromQuery] string? search, [FromQuery] int? page, [FromQuery] int? limit)
    {
        return Ok(await juegos.FindAllAsync(search, page ?? 1, limit ?? 10));
    }

    // GET /juegos/{id} -> pública, 200 / 404
    [HttpGet("{id}")]
    public async Task<ActionResult<JuegoDto>> GetById(int id)
    {
        var juego = await juegos.FindOneAsync(id);
        return juego is null ? NestError.NotFound($"No existe un juego con id {id}") : Ok(juego);
    }

    // POST /juegos -> PROTEGIDA, 201 / 400 / 401
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<JuegoDto>> Create([FromBody] CreateJuegoDto dto)
    {
        var creado = await juegos.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
    }

    // PATCH /juegos/{id} -> PROTEGIDA, 200 / 400 / 401 / 404
    [HttpPatch("{id}")]
    [Authorize]
    public async Task<ActionResult<JuegoDto>> Update(int id, [FromBody] UpdateJuegoDto dto)
    {
        var actualizado = await juegos.UpdateAsync(id, dto);
        return actualizado is null ? NestError.NotFound($"No existe un juego con id {id}") : Ok(actualizado);
    }

    // DELETE /juegos/{id} -> PROTEGIDA, 204 / 401 / 404
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Remove(int id)
    {
        var eliminado = await juegos.RemoveAsync(id);
        return eliminado ? NoContent() : NestError.NotFound($"No existe un juego con id {id}");
    }
}
