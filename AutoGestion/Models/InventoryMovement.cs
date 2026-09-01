using System.ComponentModel.DataAnnotations;

namespace AutoGestion.Models
{
    public class InventoryMovement : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public int InventoryId { get; set; }
        public Inventory? Inventory { get; set; }

        [Required]
        public Utilities.Commons.AppConstants.MovementType Type { get; set; } // Entrada o Salida

        [Required]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; } // Precio al que entró o se vendió

        [StringLength(200)]
        public string Reference { get; set; } = string.Empty; // Ejemplo: "Factura #102" o "Venta #55"

        public DateTime MovementDate { get; set; } = DateTime.UtcNow;
    }
}
