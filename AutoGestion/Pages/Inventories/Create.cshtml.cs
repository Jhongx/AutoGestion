using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.InventoryPages;

public class CreateModel : PageModel
{
    private readonly IInventoryRepository _inventoryRepository;

    public CreateModel(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    [BindProperty]
    public Inventory Inventory { get; set; } = new Inventory();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Validar que el código no exista previamente en la BD
        var existingItem = await _inventoryRepository.GetByCodeAsync(Inventory.Code);
        if (existingItem != null)
        {
            ModelState.AddModelError("Inventory.Code", "Ya existe un repuesto/insumo registrado con este código.");
            return Page();
        }

        await _inventoryRepository.AddAsync(Inventory);

        return RedirectToPage("./Index");
    }
}
