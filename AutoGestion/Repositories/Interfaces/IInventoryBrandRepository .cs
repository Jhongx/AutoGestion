using AutoGestion.Models.Inventory;

namespace AutoGestion.Repositories.Interfaces
{
    public interface IInventoryBrandRepository
    {
        Task<List<InventoryBrand>> GetAllAsync();
        Task<InventoryBrand?> GetByIdAsync(int id);
    }
}
