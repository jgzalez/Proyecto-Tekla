
// Validación simple de los atributos de la clase, sin usar paquetes extra.
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Api.Models;

public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(0.01, 999999.99, ErrorMessage = "El precio debe estar entre 0.01 y 999999.99")]
    public decimal Price { get; set; }

    [Required, MaxLength(100)]
    public string Category { get; set; } = default!;
}
