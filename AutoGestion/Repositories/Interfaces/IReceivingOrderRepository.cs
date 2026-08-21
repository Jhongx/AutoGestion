using AutoGestion.Models;

namespace AutoGestion.Repositories.Interfaces
{
    public interface IReceivingOrderRepository
    {
        Task<IEnumerable<ReceivingOrder>> GetAllAsync();

        Task<ReceivingOrder?> GetByIdAsync(int id);

        Task<IEnumerable<ReceivingOrder>> GetByVehicleIdAsync(int vehicleId);

        Task<(IEnumerable<ReceivingOrder> Orders, int TotalCount)> GetPagedAsync(string? searchTerm,string? statusFilter,int pageIndex,int pageSize);

        Task<bool> ExistsAsync(int id);
        Task AddAsync(ReceivingOrder receivingOrder);
        void Update(ReceivingOrder receivingOrder);
        void Delete(ReceivingOrder receivingOrder);
        Task<bool> SaveChangesAsync();
    }
}
