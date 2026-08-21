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

    public async Task OnGetAsync()
    {
        var items = await _inventoryRepository.GetAllAsync();
        Inventory = items.OrderBy(i => i.Name).ToList();
    }
}
