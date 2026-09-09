using AutoGestion.Data;
using AutoGestion.Models.Inventory;
using AutoGestion.Repositories.Interfaces;
using AutoGestion.Utilities.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.InventoryPages;

public class CreateModel : PageModel
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryMovementRepository _movementRepository;
    private readonly IInventoryTypeRepository _typeRepository;
    private readonly IInventoryBrandRepository _brandRepository;

    // Inyectamos todos los repositorios necesarios (inventario, movimientos y catálogos)
    public CreateModel(
        IInventoryRepository inventoryRepository,
        IInventoryMovementRepository movementRepository,
        IInventoryTypeRepository typeRepository,
        IInventoryBrandRepository brandRepository)
    {
        _inventoryRepository = inventoryRepository;
        _movementRepository = movementRepository;
        _typeRepository = typeRepository;
        _brandRepository = brandRepository;
    }

    [BindProperty]
    public Inventory Inventory { get; set; } = new Inventory();

    // Propiedades para los dropdowns en la vista
    public SelectList TypeOptions { get; set; } = default!;
    public SelectList BrandOptions { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadCatalogsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Si falla la validación, debemos recargar los catálogos para que el HTML los vuelva a mostrar
            await LoadCatalogsAsync();
            return Page();
        }

        // 1. Validar que el código no exista previamente en la BD
        var existingItem = await _inventoryRepository.GetByCodeAsync(Inventory.Code);
        if (existingItem != null)
        {
            ModelState.AddModelError("Inventory.Code", "Ya existe un repuesto/insumo registrado con este código.");
            await LoadCatalogsAsync();
            return Page();
        }

        // Guardamos temporalmente el stock inicial ingresado por el usuario
        int initialStock = Inventory.CurrentStock;

        // Hacemos que nazca en 0 para que el movimiento oficial asigne y sume el stock correctamente
        Inventory.CurrentStock = 0;

        // 2. Creamos el artículo base (incluyendo los IDs de tipo y marca seleccionados)
        await _inventoryRepository.AddAsync(Inventory);

        // 3. Si se especificó un stock inicial mayor a 0, registramos la entrada oficial en el historial
        if (initialStock > 0)
        {
            var movementResult = await _movementRepository.RegisterMovementAsync(
                inventoryId: Inventory.Id, // El ID se genera al hacer el AddAsync anterior
                quantity: initialStock,
                type: AppConstants.MovementType.Inbound,
                unitPrice: Inventory.Cost,
                reference: "Stock inicial de creación"
            );

            if (!movementResult.Success)
            {
                ModelState.AddModelError(string.Empty, "El artículo se creó, pero hubo un error al registrar el movimiento inicial de stock.");
                await LoadCatalogsAsync();
                return Page();
            }
        }

        return RedirectToPage("./Index");
    }

    // Método auxiliar privado para no repetir la carga de las listas desplegables
    private async Task LoadCatalogsAsync()
    {
        var types = await _typeRepository.GetAllAsync();
        var brands = await _brandRepository.GetAllAsync();

        TypeOptions = new SelectList(types, "Id", "Name");
        BrandOptions = new SelectList(brands, "Id", "Name");
    }
}
