using System.ComponentModel.DataAnnotations;

namespace AutoGestion.Models
{
    public class FuelLevel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del nivel de combustible es obligatorio")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // Ej: "1/4", "Medio", "Lleno"

        [StringLength(150)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
