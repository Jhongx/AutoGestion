using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using AutoGestion.Utilities.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.InventoryPages;

public class CreateModel : PageModel
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryMovementRepository _movementRepository;

    // Inyectamos ambos repositorios necesarios para la operación compuesta
    public CreateModel(
        IInventoryRepository inventoryRepository,
        IInventoryMovementRepository movementRepository)
    {
        _inventoryRepository = inventoryRepository;
        _movementRepository = movementRepository;
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

        // 1. Validar que el código no exista previamente en la BD
        var existingItem = await _inventoryRepository.GetByCodeAsync(Inventory.Code);
        if (existingItem != null)
        {
            ModelState.AddModelError("Inventory.Code", "Ya existe un repuesto/insumo registrado con este código.");
            return Page();
        }

        // Guardamos temporalmente el stock inicial ingresado por el usuario
        int initialStock = Inventory.CurrentStock;

        // Hacemos que nazca en 0 para que el movimiento oficial asigne y sume el stock correctamente
        Inventory.CurrentStock = 0;

        // 2. Creamos el artículo base
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
                // Si algo falla con el movimiento, puedes manejar el error o dejar una advertencia
                ModelState.AddModelError(string.Empty, "El artículo se creó, pero hubo un error al registrar el movimiento inicial de stock.");
                return Page();
            }
        }

        return RedirectToPage("./Index");
    }
}
