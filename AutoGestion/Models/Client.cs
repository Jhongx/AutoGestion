using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
namespace AutoGestion.Models
{
    public class Client : BaseEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(75)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(75)]
        public string LastName { get; set; } = string.Empty;
        // Relación con Tipo de Documento (Ahora opcional)
        public int? DocTypeId { get; set; }
        [ValidateNever]
        public DocType? DocType { get; set; }

        // Identificación (Ahora opcional)
        [StringLength(20)]
        public string? Identification { get; set; }

        [Required(ErrorMessage = "El teléfono principal es obligatorio")]
        [StringLength(15)]
        public string PrimaryPhone { get; set; } = string.Empty;

        [StringLength(15)]
        public string? SecondaryPhone { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;

        public List<Vehicle> Vehicles { get; set; } = new();
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
