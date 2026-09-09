using System.ComponentModel.DataAnnotations;

namespace AutoGestion.Models.Inventory
{
    public class InventoryType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // Ej: Filtros, Lubricantes, Frenos

        public bool IsActive { get; set; } = true;
    }
}
