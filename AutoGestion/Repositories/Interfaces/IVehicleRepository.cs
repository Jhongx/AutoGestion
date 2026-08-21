using AutoGestion.Models;

namespace AutoGestion.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(int id);
        Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
        Task<IEnumerable<Vehicle>> GetByClientIdAsync(int clientId);
        Task<(IEnumerable<Vehicle> Vehicles, int TotalCount)> GetPagedAsync(
            string? searchTerm,
            int pageIndex,
            int pageSize);
        Task<bool> ExistsAsync(int id);
        Task<bool> LicensePlateExistsAsync(string licensePlate, int? excludeId = null);
        Task AddAsync(Vehicle vehicle);
        void Update(Vehicle vehicle);
        void Delete(Vehicle vehicle);
        Task<bool> SaveChangesAsync();
    }
}
