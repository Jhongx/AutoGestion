using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoGestion.Pages.InventoryMovements
{
    public class MovementsHistoryModel : PageModel
    {
        private readonly IInventoryMovementRepository _movementRepository;

        public MovementsHistoryModel(IInventoryMovementRepository movementRepository)
        {
            _movementRepository = movementRepository;
        }

        public IList<InventoryMovement> Movements { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Obtenemos todos los movimientos registrados. 
            // Si tu repositorio tiene un método para filtrar u obtener con los datos del inventario incluídos (Include), úsalo aquí.
            var allMovements = await _movementRepository.GetAllMovementsAsync(); // O el método correspondiente en tu interfaz

            // Aplicamos un filtro opcional por referencia o nombre del artículo si se ingresó texto
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Movements = allMovements
                    .Where(m => (m.Reference != null && m.Reference.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                                (m.Inventory != null && m.Inventory.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)))
                    .OrderByDescending(m => m.MovementDate)
                    .ToList();
            }
            else
            {
                Movements = allMovements.OrderByDescending(m => m.MovementDate).ToList();
            }

            return Page();
        }
    }
}
