using AutoGestion.Data;
using AutoGestion.Helpers;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.InventoryPages;

public class EditModel : PageModel
{
    private readonly IInventoryRepository _inventoryRepository;

    public EditModel(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Validar que el nuevo código no pertenezca a OTRO producto existente
        var existingByCode = await _inventoryRepository.GetByCodeAsync(Inventory.Code);
        if (existingByCode != null && existingByCode.Id != Inventory.Id)
        {
            ModelState.AddModelError("Inventory.Code", "Ya existe otro repuesto registrado con este código.");
            return Page();
        }

        try
        {
            Inventory.UpdatedAt = DateTime.UtcNow.ToCostaRicaTime();
            await _inventoryRepository.UpdateAsync(Inventory);
        }
        catch (DbUpdateConcurrencyException)
        {
            var exists = await _inventoryRepository.GetByIdAsync(Inventory.Id);
            if (exists is null)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }
}
