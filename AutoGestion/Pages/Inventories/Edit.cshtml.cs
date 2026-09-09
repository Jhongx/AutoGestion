using AutoGestion.Data;
using AutoGestion.Helpers;
using AutoGestion.Models.Inventory;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.InventoryPages;

public class EditModel : PageModel
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryTypeRepository _typeRepository;
    private readonly IInventoryBrandRepository _brandRepository;

    // Inyectamos el repositorio de inventario y los repositorios de los catálogos
    public EditModel(
        IInventoryRepository inventoryRepository,
        IInventoryTypeRepository typeRepository,
        IInventoryBrandRepository brandRepository)
    {
        _inventoryRepository = inventoryRepository;
        _typeRepository = typeRepository;
        _brandRepository = brandRepository;
    }

    [BindProperty]
    public Inventory Inventory { get; set; } = default!;

    // Propiedades para los selectores en la vista de edición
    public SelectList TypeOptions { get; set; } = default!;
    public SelectList BrandOptions { get; set; } = default!;

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

        // Cargamos los catálogos para los dropdowns
        await LoadCatalogsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Si falla la validación, recargamos los catálogos para no perder las listas en la vista
            await LoadCatalogsAsync();
            return Page();
        }

        // Validar que el nuevo código no pertenezca al de OTRO producto existente
        var existingByCode = await _inventoryRepository.GetByCodeAsync(Inventory.Code);
        if (existingByCode != null && existingByCode.Id != Inventory.Id)
        {
            ModelState.AddModelError("Inventory.Code", "Ya existe otro repuesto registrado con este código.");
            await LoadCatalogsAsync();
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

    // Método auxiliar privado para cargar los catálogos de tipo y marca
    private async Task LoadCatalogsAsync()
    {
        var types = await _typeRepository.GetAllAsync();
        var brands = await _brandRepository.GetAllAsync();

        TypeOptions = new SelectList(types, "Id", "Name");
        BrandOptions = new SelectList(brands, "Id", "Name");
    }
}
