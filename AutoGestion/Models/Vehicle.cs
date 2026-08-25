using System.ComponentModel.DataAnnotations;
using static AutoGestion.Utilities.Commons.AppConstants;

namespace AutoGestion.Models
{
    public class Vehicle : BaseEntity
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        [Required(ErrorMessage = "La placa es obligatoria")]
        [StringLength(15)]
        public string LicensePlate { get; set; } = string.Empty;

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }

        public Transmission Transmission { get; set; }
        public bool IsActive { get; set; } = true;

        public List<ReceivingOrder> ReceivingOrders { get; set; } = new();
    }
}
