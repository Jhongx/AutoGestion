using AutoGestion.Data;
using AutoGestion.Helpers;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.ReceivingOrderPages;

public class EditModel : PageModel
{
    private readonly IReceivingOrderRepository _receivingOrderRepo;
    private readonly IVehicleRepository _vehicleRepo;
    private readonly IClientRepository _clientRepo;
    private readonly ApplicationDbContext _context; // <-- AGREGADO

    public EditModel(
        IReceivingOrderRepository receivingOrderRepo,
        IVehicleRepository vehicleRepo,
        IClientRepository clientRepo,
        ApplicationDbContext context) // <-- AGREGADO
    {
        _receivingOrderRepo = receivingOrderRepo;
        _vehicleRepo = vehicleRepo;
        _clientRepo = clientRepo;
        _context = context; // <-- AGREGADO
    }

    [BindProperty]
    public ReceivingOrder ReceivingOrder { get; set; } = default!;

    // Propiedad para llenar el <select> de vehículos en la vista
    public SelectList VehiclesList { get; set; } = default!;

    // Propiedad para llenar el <select> de niveles de combustible en la vista
    public SelectList FuelLevelsList { get; set; } = default!; // <-- AGREGADO

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var receivingorder = await _receivingOrderRepo.GetByIdAsync(id.Value);
        if (receivingorder is null)
        {
            return NotFound();
        }

        ReceivingOrder = receivingorder;
        await LoadSelectListsAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Si la validación falla, recargamos el SelectList antes de retornar la página
            await LoadSelectListsAsync();
            return Page();
        }

        try
        {
            ReceivingOrder.UpdatedAt = DateTime.UtcNow.ToCostaRicaTime();
            _receivingOrderRepo.Update(ReceivingOrder);
        }
        catch (Exception)
        {
            if (!await _receivingOrderRepo.ExistsAsync(ReceivingOrder.Id))
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

    /// <summary>
    /// Carga los datos necesarios para los controles desplegables del formulario.
    /// </summary>
    private async Task LoadSelectListsAsync()
    {
        // Carga la lista completa de vehículos desde el repositorio
        var vehicles = await _vehicleRepo.GetAllAsync();

        // Proyectamos los datos para formatear la opción: "Placa - Marca Modelo (Propietario: Nombre Apellido)"
        var vehicleOptions = vehicles.Select(v => new
        {
            Id = v.Id,
            DisplayText = $"{v.LicensePlate} - {v.Brand} {v.Model} (Propietario: {v.Client?.FirstName} {v.Client?.LastName})"
        });

        // Se asigna ReceivingOrder?.VehicleId como cuarto argumento para marcar el valor activo en el Edit
        VehiclesList = new SelectList(
            vehicleOptions,
            "Id",
            "DisplayText",
            ReceivingOrder?.VehicleId
        );

        // Carga de catálogo FuelLevel activos
        var fuelLevels = await _context.FuelLevels
            .Where(f => f.IsActive)
            .OrderBy(f => f.Id)
            .ToListAsync();

        // Se asigna ReceivingOrder?.FuelLevelId como cuarto argumento para marcar la opción seleccionada
        FuelLevelsList = new SelectList(
            fuelLevels,
            "Id",
            "Name",
            ReceivingOrder?.FuelLevelId
        );
    }

    public async Task<JsonResult> OnGetVehicleClientInfoAsync(int vehicleId)
    {
        var vehicle = await _vehicleRepo.GetByIdAsync(vehicleId); // Asegúrate que devuelva v.Client

        if (vehicle?.Client != null)
        {
            return new JsonResult(new
            {
                success = true,
                clientName = $"{vehicle.Client.FirstName} {vehicle.Client.LastName}",
                phone = vehicle.Client.PrimaryPhone ?? "Sin teléfono"
            });
        }

        return new JsonResult(new { success = false });
    }
}
