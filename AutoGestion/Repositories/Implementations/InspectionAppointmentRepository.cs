using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace AutoGestion.Repositories.Implementations
{
    public class InspectionAppointmentRepository : IInspectionAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public InspectionAppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InspectionAppointment>> GetAllAsync()
        {
            return await _context.InspectionAppointments
                .Include(a => a.Client)
                .Include(a => a.Vehicle)
                .Include(a => a.ReceivingOrder)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<InspectionAppointment?> GetByIdAsync(int id)
        {
            return await _context.InspectionAppointments
                .Include(a => a.Client)
                .Include(a => a.Vehicle)
                .Include(a => a.ReceivingOrder)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<InspectionAppointment>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _context.InspectionAppointments
                .Include(a => a.Client)
                .Include(a => a.Vehicle)
                .Where(a => a.ScheduledDateTime >= start && a.ScheduledDateTime <= end)
                .OrderBy(a => a.ScheduledDateTime)
                .ToListAsync();
        }

        public async Task AddAsync(InspectionAppointment appointment)
        {
            await _context.InspectionAppointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(InspectionAppointment appointment)
        {
            _context.InspectionAppointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _context.InspectionAppointments.FindAsync(id);
            if (appointment != null)
            {
                _context.InspectionAppointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.InspectionAppointments.AnyAsync(a => a.Id == id);
        }
    }
}
