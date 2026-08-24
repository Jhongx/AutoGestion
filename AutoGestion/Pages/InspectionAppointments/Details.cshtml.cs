using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoGestion.Pages.InspectionAppointments
{
    public class DetailsModel : PageModel
    {
        private readonly IInspectionAppointmentRepository _appointmentRepo;

        public DetailsModel(IInspectionAppointmentRepository appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
        }

        public InspectionAppointment InspectionAppointment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            InspectionAppointment = await _appointmentRepo.GetByIdAsync(id);

            if (InspectionAppointment == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
