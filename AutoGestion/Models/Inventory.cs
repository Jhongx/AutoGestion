using System.ComponentModel.DataAnnotations;

namespace AutoGestion.Models
{
    public class Inventory : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public int CurrentStock { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal Cost { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
