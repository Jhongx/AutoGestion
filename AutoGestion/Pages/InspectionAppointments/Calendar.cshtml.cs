using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AutoGestion.Pages.InspectionAppointments
{
    public class CalendarModel : PageModel
    {
        private readonly IInspectionAppointmentRepository _appointmentRepository;

        public CalendarModel(IInspectionAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public string EventsJson { get; set; } = "[]";

        public async Task OnGetAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();

            var events = appointments.Select(a => new
            {
                id = a.Id,
                title = $"Cita #{a.Id:D5} - {a.Vehicle?.LicensePlate ?? a.Client?.FirstName ?? "S/P"}",
                start = a.ScheduledDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                url = $"/InspectionAppointments/Details?id={a.Id}",
                color = GetColorByStatus(a.Status),
                extendedProps = new
                {
                    time = a.ScheduledDateTime.ToString("hh:mm tt"),
                    inspectionType = a.InspectionType,
                    status = a.Status,
                    // Validamos que no esté convertida a orden (ignorando mayúsculas/minúsculas)
                    canReceive = !string.Equals(a.Status, "Convertida a Orden", StringComparison.OrdinalIgnoreCase) &&
                                 !string.Equals(a.Status, "Convertida", StringComparison.OrdinalIgnoreCase),
                    client = a.Client != null ? $"{a.Client.FirstName} {a.Client.LastName}" : "N/A",
                    vehicle = a.Vehicle != null ? $"{a.Vehicle.Brand} {a.Vehicle.Model} ({a.Vehicle.LicensePlate})" : "Por definir",
                    reason = a.Reason ?? "Sin detalles",
                    fullTitle = $"Cita #{a.Id:D5} - {a.InspectionType}"
                }
            });

            EventsJson = JsonSerializer.Serialize(events);
        }

        private static string GetColorByStatus(string? status)
        {
            return status?.ToLower() switch
            {
                "programada" => "#0d6efd",   // Azul
                "confirmada" => "#198754",   // Verde
                "convertida" => "#6c757d",   // Gris (ya pasó a recepción)
                "cancelada" => "#dc3545",    // Rojo
                _ => "#ffc107"               // Amarillo
            };
        }
    }
}
