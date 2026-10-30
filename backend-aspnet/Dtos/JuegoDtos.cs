using System.ComponentModel.DataAnnotations;

namespace JuegosApi.Dtos;

// Equivale a CreateJuegoDto de NestJS (los tres campos son obligatorios).
public class CreateJuegoDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "La imagen (URL) es obligatoria")]
    public string Imagen { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "La categoría es obligatoria")]
    public string Categoria { get; set; } = string.Empty;
}

// Equivale a UpdateJuegoDto (PartialType): PATCH acepta cualquier subconjunto de campos.
public class UpdateJuegoDto
{
    [RegularExpression(@"(?s).*\S.*", ErrorMessage = "El nombre no puede estar vacío")]
    public string? Nombre { get; set; }

    [RegularExpression(@"(?s).*\S.*", ErrorMessage = "La imagen (URL) no puede estar vacía")]
    public string? Imagen { get; set; }

    [RegularExpression(@"(?s).*\S.*", ErrorMessage = "La categoría no puede estar vacía")]
    public string? Categoria { get; set; }
}

// Respuesta de un juego: { id, nombre, imagen, categoria, creadoEn }
public record JuegoDto(int Id, string Nombre, string Imagen, string Categoria, DateTime CreadoEn);

// Respuesta paginada de NestJS: { data, total, page, limit }.
// Además se exponen "items" y "totalPages" porque la vista Vue del Taller #1
// (JuegosListView.vue) lee esos dos nombres. Así el cliente funciona sin tocarlo.
public record PaginatedJuegosDto(IReadOnlyList<JuegoDto> Data, int Total, int Page, int Limit)
{
    public IReadOnlyList<JuegoDto> Items => Data;
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(Total / (double)Limit));
}
