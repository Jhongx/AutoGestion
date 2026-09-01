using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Models.Common;
using AutoGestion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static AutoGestion.Utilities.Commons.AppConstants;

namespace AutoGestion.Repositories.Implementations
{
    public class InventoryMovementRepository : IInventoryMovementRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryMovementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<InventoryMovement>> RegisterMovementAsync(
            int inventoryId, int quantity, MovementType type, decimal unitPrice, string reference)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var item = await _context.Set<Inventory>().FindAsync(inventoryId);
                if (item == null || !item.IsActive)
                {
                    return ServiceResponse<InventoryMovement>.Fail("El producto no existe o se encuentra inactivo.");
                }

                if (quantity <= 0)
                {
                    return ServiceResponse<InventoryMovement>.Fail("La cantidad debe ser mayor a cero.");
                }

                if (type == MovementType.Outbound)
                {
                    if (item.CurrentStock < quantity)
                    {
                        return ServiceResponse<InventoryMovement>.Fail(
                            $"Stock insuficiente. Stock actual disponible: {item.CurrentStock}");
                    }

                    item.CurrentStock -= quantity;
                }
                else
                {
                    item.CurrentStock += quantity;
                    item.Cost = unitPrice; // Actualiza el costo de adquisición si es entrada
                }

                var movement = new InventoryMovement
                {
                    InventoryId = inventoryId,
                    Quantity = quantity,
                    Type = type,
                    UnitPrice = unitPrice,
                    Reference = reference,
                    MovementDate = DateTime.UtcNow
                };

                _context.InventoryMovements.Add(movement);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ServiceResponse<InventoryMovement>.Ok(movement, "Movimiento de inventario registrado correctamente.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ServiceResponse<InventoryMovement>.Fail($"Ocurrió un error inesperado al procesar el movimiento: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<Inventory>> UpdateStockAsync(int inventoryId, int newStock)
        {
            try
            {
                var item = await _context.Set<Inventory>().FindAsync(inventoryId);
                if (item == null)
                {
                    return ServiceResponse<Inventory>.Fail("El producto no fue encontrado.");
                }

                if (newStock < 0)
                {
                    return ServiceResponse<Inventory>.Fail("El stock no puede ser un valor negativo.");
                }

                item.CurrentStock = newStock;
                _context.Set<Inventory>().Update(item);
                await _context.SaveChangesAsync();

                return ServiceResponse<Inventory>.Ok(item, "Stock actualizado de manera exitosa.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Inventory>.Fail($"Error al actualizar el stock: {ex.Message}");
            }
        }

        // --- NUEVO MÉTODO IMPLEMENTADO ---
        public async Task<IEnumerable<InventoryMovement>> GetAllMovementsAsync()
        {
            return await _context.InventoryMovements
                .Include(m => m.Inventory) // Carga los datos del repuesto asociado
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }
    }
}
