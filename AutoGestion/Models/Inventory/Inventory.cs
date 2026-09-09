using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoGestion.Models.Inventory
{
    public class Inventory : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string? Code { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // --- RELACIONES NULLABLE CON CATÁLOGOS ---
        public int? InventoryTypeId { get; set; }
        [ForeignKey("InventoryTypeId")]
        public virtual InventoryType? InventoryType { get; set; }

        public int? InventoryBrandId { get; set; }
        [ForeignKey("InventoryBrandId")]
        public virtual InventoryBrand? InventoryBrand { get; set; }
        // ----------------------------------------

        public int CurrentStock { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Cost { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

