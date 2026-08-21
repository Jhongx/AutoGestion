using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoGestion.Pages.InspectionAppointments
{
    public class CreateModel : PageModel
    {
        private readonly IInspectionAppointmentRepository _appointmentRepo;
        private readonly IVehicleRepository _vehicleRepo;
        private readonly IClientRepository _clientRepo;

        public CreateModel(
            IInspectionAppointmentRepository appointmentRepo,
            IVehicleRepository vehicleRepo,
            IClientRepository clientRepo)
        {
            _appointmentRepo = appointmentRepo;
            _vehicleRepo = vehicleRepo;
            _clientRepo = clientRepo;
        }

        [BindProperty]
        public InspectionAppointment InspectionAppointment { get; set; } = default!;

        public SelectList ClientsList { get; set; } = default!;
        public SelectList VehiclesList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? selectedDate = null)
        {
            // 1. Determinar fecha y hora base de inicio (redondeada al siguiente intervalo de 15 min)
            DateTime baseDateTime;
            if (!string.IsNullOrEmpty(selectedDate) && DateTime.TryParse(selectedDate, out DateTime parsedDate))
            {
                baseDateTime = parsedDate;
            }
            else
            {
                var now = DateTime.Now;
                // Redondear los minutos al siguiente múltiplo de 15 (ej: 10:12 -> 10:15)
                int roundedMinutes = (int)(Math.Ceiling(now.Minute / 15.0) * 15);
                baseDateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddMinutes(roundedMinutes);
            }

            // 2. Instanciar el objeto asignando las propiedades individuales
            InspectionAppointment = new InspectionAppointment
            {
                Status = "Programada",
                InspectionType = "Diagnóstico General",
                AppointmentDate = DateOnly.FromDateTime(baseDateTime),
                StartTime = TimeOnly.FromDateTime(baseDateTime),
                EndTime = TimeOnly.FromDateTime(baseDateTime.AddHours(1))
            };

            await LoadSelectListsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Si seleccionaron un vehículo pero no un cliente explícito,
            // asignamos automáticamente el ClientId desde el Vehículo
            if (InspectionAppointment.VehicleId.HasValue && InspectionAppointment.ClientId == 0)
            {
                var vehicle = await _vehicleRepo.GetByIdAsync(InspectionAppointment.VehicleId.Value);
                if (vehicle != null)
                {
                    InspectionAppointment.ClientId = vehicle.ClientId;
                    // Limpiamos el error de validación de ClientId si existía
                    ModelState.Remove("InspectionAppointment.ClientId");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            await _appointmentRepo.AddAsync(InspectionAppointment);
            return RedirectToPage("./Calendar");
        }

        private async Task LoadSelectListsAsync()
        {
            var clients = await _clientRepo.GetAllAsync();
            var vehicles = await _vehicleRepo.GetAllAsync();

            // Lista de Clientes
            var clientOptions = clients.Select(c => new
            {
                Id = c.Id,
                DisplayText = $"{c.FirstName} {c.LastName} {(string.IsNullOrEmpty(c.PrimaryPhone) ? "" : $"({c.PrimaryPhone})")}"
            });
            ClientsList = new SelectList(clientOptions, "Id", "DisplayText", InspectionAppointment?.ClientId);

            // Lista de Vehículos
            var vehicleOptions = vehicles.Select(v => new
            {
                Id = v.Id,
                ClientId = v.ClientId,
                DisplayText = $"{v.LicensePlate} - {v.Brand} {v.Model} (Propietario: {v.Client?.FirstName} {v.Client?.LastName})"
            });
            VehiclesList = new SelectList(vehicleOptions, "Id", "DisplayText", InspectionAppointment?.VehicleId);
        }

        /// <summary>
        /// Handler AJAX para sincronizar el Cliente al seleccionar un Vehículo
        /// </summary>
        public async Task<JsonResult> OnGetVehicleClientInfoAsync(int vehicleId)
        {
            var vehicle = await _vehicleRepo.GetByIdAsync(vehicleId);

            if (vehicle != null)
            {
                return new JsonResult(new
                {
                    success = true,
                    clientId = vehicle.ClientId,
                    clientName = vehicle.Client != null ? $"{vehicle.Client.FirstName} {vehicle.Client.LastName}" : "Sin asignar"
                });
            }

            return new JsonResult(new { success = false });
        }
    }
}
