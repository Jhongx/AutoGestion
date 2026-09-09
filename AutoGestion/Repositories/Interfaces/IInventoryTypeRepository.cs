using AutoGestion.Models.Inventory;

namespace AutoGestion.Repositories.Interfaces
{
    public interface IInventoryTypeRepository
    {
        Task<List<InventoryType>> GetAllAsync();
        Task<InventoryType?> GetByIdAsync(int id);
    }
}
