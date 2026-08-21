using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using static AutoGestion.Utilities.Commons.AppConstants;

namespace AutoGestion.Models
{
    public class ReceivingOrder
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;
        public DateTime EntryDate { get; set; } = DateTime.Now;

        public int Mileage { get; set; }

        // ==========================================
        // CATÁLOGO: FUEL LEVEL (Nivel de Combustible)
        // ==========================================
        [Required(ErrorMessage = "El nivel de combustible es obligatorio")]
        public int FuelLevelId { get; set; }

        [ForeignKey("FuelLevelId")]
        public FuelLevel? FuelLevel { get; set; }

        [StringLength(100)]
        public string ServiceType { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción del problema es obligatoria")]
        public string ProblemDescription { get; set; } = string.Empty;

        // ==========================================
        // PROPIEDADES DE ESTADO Y SEGUIMIENTO:
        // ==========================================
        [StringLength(50)]
        public string? Status { get; set; }

        public string? DiagnosticNotes { get; set; }

        public DateTime? CompletionDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
