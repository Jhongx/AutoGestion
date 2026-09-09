using System.ComponentModel.DataAnnotations;

namespace AutoGestion.Models.Inventory
{
    public class InventoryBrand
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // Ej: Bosch, Mobil 1, Denso

        public bool IsActive { get; set; } = true;
    }
}