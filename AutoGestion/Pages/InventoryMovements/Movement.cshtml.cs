using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static AutoGestion.Utilities.Commons.AppConstants;

namespace AutoGestion.Pages.InventoryMovements
{
    public class MovementModel : PageModel
    {
        private readonly IInventoryMovementRepository _movementRepository;
        private readonly IInventoryRepository _inventoryRepository; // Asumimos que existe para listar repuestos

        public MovementModel(IInventoryMovementRepository movementRepository, IInventoryRepository inventoryRepository)
        {
            _movementRepository = movementRepository;
            _inventoryRepository = inventoryRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public SelectList InventoryOptions { get; set; } = default!;

        public class InputModel
        {
            [BindProperty(SupportsGet = true)]
            public int? Id { get; set; } // Opcional: si viene precargado desde el botón de la tabla

            [Required(ErrorMessage = "Debe seleccionar un repuesto.")]
            public int InventoryId { get; set; }

            [Required(ErrorMessage = "Debe indicar el tipo de movimiento.")]
            public MovementType Type { get; set; }

            [Required(ErrorMessage = "La cantidad es obligatoria.")]
            [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")] // Ajusta validación numérica según prefieras
            public int Quantity { get; set; } = 1;

            [Required(ErrorMessage = "El precio unitario es obligatorio.")]
            [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
            public decimal UnitPrice { get; set; }

            [StringLength(200, ErrorMessage = "La referencia no puede superar los 200 caracteres.")]
            public string Reference { get; set; } = string.Empty;
        }

        [BindProperty(SupportsGet = true)]
        public MovementType? Type { get; set; } // Para capturar si viene por URL ?type=Inbound o Outbound

        public async Task<IActionResult> OnGetAsync(int? id, MovementType? type)
        {
            await LoadSelectListAsync();

            // Pre-llenar valores si el usuario hizo clic en un botón específico
            Input = new InputModel
            {
                InventoryId = id ?? 0,
                Type = type ?? MovementType.Inbound
            };

            // Si viene un ID de repuesto preseleccionado, podemos intentar precargar su precio de costo o venta sugerido
            if (id.HasValue)
            {
                var item = await _inventoryRepository.GetByIdAsync(id.Value);
                if (item != null)
                {
                    Input.UnitPrice = type == MovementType.Inbound ? item.Cost : item.UnitPrice;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListAsync();
                return Page();
            }

            // Ejecutamos el registro usando tu repositorio dedicado
            var response = await _movementRepository.RegisterMovementAsync(
                Input.InventoryId,
                Input.Quantity,
                Input.Type,
                Input.UnitPrice,
                Input.Reference
            );

            if (!response.Success)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Ocurrió un error al registrar el movimiento.");
                await LoadSelectListAsync();
                return Page();
            }

            // Redirigir de vuelta al inventario o al historial con éxito
            TempData["SuccessMessage"] = "Movimiento de inventario registrado correctamente.";
            return RedirectToPage("./MovementHistory"); // O redirigir a Inventories/Index según prefieras
        }

        private async Task LoadSelectListAsync()
        {
            var items = await _inventoryRepository.GetAllAsync(); // O tu método equivalente para listar repuestos
            InventoryOptions = new SelectList(items, "Id", "Name");
        }
    }
}
