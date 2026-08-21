using AutoGestion.Models;

namespace AutoGestion.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        Task AddAsync(Inventory entity);
        Task UpdateAsync(Inventory entity);
        Task DeleteAsync(int id);
        Task<Inventory?> GetByIdAsync(int id);
        Task<Inventory?> GetByCodeAsync(string code);
        Task<IEnumerable<Inventory>> GetAllAsync();
        Task<IEnumerable<Inventory>> GetLowStockAsync(int threshold = 5);
        Task<IEnumerable<Inventory>> SearchByNameOrCodeAsync(string searchTerm);
    }
}
