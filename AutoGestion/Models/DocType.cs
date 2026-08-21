using System.ComponentModel.DataAnnotations;

namespace AutoGestion.Models
{
    public class DocType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del tipo de documento es obligatorio")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Relación con Clientes
        public List<Client> Clients { get; set; } = new();
    }
}
