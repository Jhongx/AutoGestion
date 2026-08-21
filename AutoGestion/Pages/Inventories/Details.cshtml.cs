using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.InventoryPages;

public class DetailsModel : PageModel
{
    private readonly IInventoryRepository _inventoryRepository;

    public DetailsModel(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public Inventory Inventory { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var inventory = await _inventoryRepository.GetByIdAsync(id.Value);
        if (inventory is null)
        {
            return NotFound();
        }

        Inventory = inventory;
        return Page();
    }
}
