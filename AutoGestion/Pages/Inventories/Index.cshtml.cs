using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AutoGestion.Models;
using AutoGestion.Data;
using AutoGestion.Repositories.Interfaces;

namespace AutoGestion.Pages.InventoryPages;

public class IndexModel : PageModel
{
    private readonly IInventoryRepository _inventoryRepository;

    public IndexModel(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public IList<Inventory> Inventory { get; set; } = new List<Inventory>();

    // Opcional: Para mostrar totales en tarjetas superiores si lo deseas
    public int TotalProducts { get; set; }
    public int LowStockCount { get; set; }

    public async Task OnGetAsync()
    {
        var items = await _inventoryRepository.GetAllAsync();

        // Ordenamos alfabéticamente por nombre
        Inventory = items.OrderBy(i => i.Name).ToList();

        // Métricas útiles para el Dashboard de Inventario
        TotalProducts = Inventory.Count;
        LowStockCount = Inventory.Count(i => i.CurrentStock <= 5); // Ejemplo de umbral de stock bajo
    }
}
