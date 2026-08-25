using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoGestion.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IReceivingOrderRepository _receivingOrderRepository;

        public IndexModel(
            IInventoryRepository inventoryRepository,
            IReceivingOrderRepository receivingOrderRepository)
        {
            _inventoryRepository = inventoryRepository;
            _receivingOrderRepository = receivingOrderRepository;
        }

        // Propiedades públicas para los KPIs
        public int TotalItemsInInventory { get; set; }
        public int LowStockItemsCount { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public int ActiveReceivingOrders { get; set; }

        public async Task OnGetAsync()
        {
            // 1. Obtener todos los inventarios activos para calcular totales y valor
            var allInventory = await _inventoryRepository.GetAllAsync();
            var activeInventory = allInventory.Where(i => i.IsActive).ToList();

            TotalItemsInInventory = activeInventory.Count;

            // Valor total del inventario (Stock * Costo)
            TotalInventoryValue = activeInventory.Sum(i => i.CurrentStock * i.Cost);

            // 2. Obtener items con stock bajo (usando el método que ya tienes en tu repositorio con umbral de 5)
            var lowStockItems = await _inventoryRepository.GetLowStockAsync(5);
            LowStockItemsCount = lowStockItems.Count(i => i.IsActive);

            // 3. Órdenes activas en taller
            // Usamos GetPagedAsync trayendo un pageSize alto (ej. 1000) o un método GetAll si estuviera disponible, 
            // filtrando por aquellas que no tengan fecha de finalización (CompletionDate == null)
            var pagedOrders = await _receivingOrderRepository.GetPagedAsync(null, null, 1, 1000);
            ActiveReceivingOrders = pagedOrders.Orders.Count(o => o.IsActive && o.CompletionDate == null);
        }
    }
}
