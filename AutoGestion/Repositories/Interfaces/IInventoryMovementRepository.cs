using AutoGestion.Models;
using AutoGestion.Models.Common;
using static AutoGestion.Utilities.Commons.AppConstants;

namespace AutoGestion.Repositories.Interfaces
{
    public interface IInventoryMovementRepository
    {
        Task<ServiceResponse<InventoryMovement>> RegisterMovementAsync(int inventoryId, int quantity, MovementType type, decimal unitPrice, string reference);
        Task<ServiceResponse<Inventory>> UpdateStockAsync(int inventoryId, int newStock);
        Task<IEnumerable<InventoryMovement>> GetAllMovementsAsync();
    }
}
