using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoGestion.Pages.InspectionAppointments
{
    public class EditModel : PageModel
    {
        private readonly IInspectionAppointmentRepository _appointmentRepo;
        private readonly IVehicleRepository _vehicleRepo;
        private readonly IClientRepository _clientRepo;

        public EditModel(
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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            InspectionAppointment = await _appointmentRepo.GetByIdAsync(id);

            if (InspectionAppointment == null)
            {
                return NotFound();
            }

            await LoadSelectListsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            await _appointmentRepo.UpdateAsync(InspectionAppointment);
            return RedirectToPage("./Calendar");
        }

        private async Task LoadSelectListsAsync()
        {
            var clients = await _clientRepo.GetAllAsync();
            var vehicles = await _vehicleRepo.GetAllAsync();

            ClientsList = new SelectList(clients.Select(c => new { c.Id, Name = $"{c.FirstName} {c.LastName}" }), "Id", "Name", InspectionAppointment?.ClientId);
            VehiclesList = new SelectList(vehicles.Select(v => new { v.Id, Desc = $"{v.LicensePlate} - {v.Brand} {v.Model}" }), "Id", "Desc", InspectionAppointment?.VehicleId);
        }
    }
}
