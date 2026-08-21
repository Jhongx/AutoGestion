using AutoGestion.Models;

namespace AutoGestion.Repositories.Interfaces
{
    public interface IInspectionAppointmentRepository
    {
        Task<IEnumerable<InspectionAppointment>> GetAllAsync();
        Task<InspectionAppointment?> GetByIdAsync(int id);
        Task<IEnumerable<InspectionAppointment>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task AddAsync(InspectionAppointment appointment);
        Task UpdateAsync(InspectionAppointment appointment);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
