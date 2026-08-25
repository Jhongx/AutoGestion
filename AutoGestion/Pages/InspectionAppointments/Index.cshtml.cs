using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoGestion.Pages.InspectionAppointments
{
    public class IndexModel : PageModel
    {
        private readonly IInspectionAppointmentRepository _appointmentRepo;

        public IndexModel(IInspectionAppointmentRepository appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
        }

        public IList<InspectionAppointment> InspectionAppointments { get; set; } = new List<InspectionAppointment>();

        public async Task OnGetAsync()
        {
            var appointments = await _appointmentRepo.GetAllAsync();
            InspectionAppointments = appointments.ToList();
        }
    }
}
