using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoGestion.Models
{
    public class InspectionAppointment : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        // 1. Fecha del día de la cita (Ajustada a la hora de Costa Rica por defecto)
        [Required(ErrorMessage = "La fecha de la cita es obligatoria.")]
        [Display(Name = "Fecha de la Cita")]
        public DateOnly AppointmentDate { get; set; } = DateOnly.FromDateTime(
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("America/Costa_Rica"))
        );

        // 2. Hora de inicio del rango
        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        [Display(Name = "Hora de Inicio")]
        public TimeOnly StartTime { get; set; } = new TimeOnly(8, 0); // 08:00 AM por defecto

        // 3. Hora de fin del rango
        [Required(ErrorMessage = "La hora de finalización es obligatoria.")]
        [Display(Name = "Hora de Finalización")]
        public TimeOnly EndTime { get; set; } = new TimeOnly(9, 0); // 09:00 AM por defecto

        [Required(ErrorMessage = "El tipo de inspección es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Tipo de Inspección")]
        public string InspectionType { get; set; } = "Diagnóstico General";

        [StringLength(500)]
        [Display(Name = "Motivo / Sintomatología")]
        public string? Reason { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Estado")]
        public string Status { get; set; } = "Programada"; // Programada, Confirmada, Convertida, Cancelada

        // --- Relaciones ---

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        [Display(Name = "Cliente")]
        public int ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public Client? Client { get; set; }

        [Display(Name = "Vehículo")]
        public int? VehicleId { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public Vehicle? Vehicle { get; set; }

        [Display(Name = "Orden de Recepción")]
        public int? ReceivingOrderId { get; set; }

        [ForeignKey(nameof(ReceivingOrderId))]
        public ReceivingOrder? ReceivingOrder { get; set; }

        // --- Propiedades Calculadas / Auxiliares ([NotMapped]) ---

        /// <summary>
        /// Fecha y hora completa de inicio. Permite lectura y asignación externa (p. ej. FullCalendar / DTOs).
        /// </summary>
        [NotMapped]
        public DateTime ScheduledDateTime
        {
            get => AppointmentDate.ToDateTime(StartTime);
            set
            {
                AppointmentDate = DateOnly.FromDateTime(value);
                StartTime = TimeOnly.FromDateTime(value);
            }
        }

        /// <summary>
        /// Fecha y hora completa de fin. Permite lectura y asignación externa.
        /// </summary>
        [NotMapped]
        public DateTime EndDateTime
        {
            get => AppointmentDate.ToDateTime(EndTime);
            set
            {
                EndTime = TimeOnly.FromDateTime(value);
            }
        }

        /// <summary>
        /// Duración total calculada automáticamente en minutos.
        /// </summary>
        [NotMapped]
        public int EstimatedDurationMinutes
        {
            get
            {
                var start = AppointmentDate.ToDateTime(StartTime);
                var end = AppointmentDate.ToDateTime(EndTime);

                // Si la hora final es menor que la inicial, asume que finaliza al día siguiente
                if (end < start)
                {
                    end = end.AddDays(1);
                }

                return (int)(end - start).TotalMinutes;
            }
        }
    }
}
