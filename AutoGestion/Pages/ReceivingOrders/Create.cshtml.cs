using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.ReceivingOrderPages;

public class CreateModel : PageModel
{
    private readonly IReceivingOrderRepository _receivingOrderRepo;
    private readonly IVehicleRepository _vehicleRepo;
    private readonly IClientRepository _clientRepo;
    private readonly ApplicationDbContext _context; // <-- AGREGADO

    public CreateModel(
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

    // Propiedad para llenar el <select> de vehículos en la vista .cshtml
    public SelectList VehiclesList { get; set; } = default!;

    // Propiedad para llenar el <select> de niveles de combustible
    public SelectList FuelLevelsList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Si la validación falla, se recargan las listas antes de volver a la vista
            await LoadSelectListsAsync();
            return Page();
        }

        await _receivingOrderRepo.AddAsync(ReceivingOrder);

        return RedirectToPage("./Index");
    }

    /// <summary>
    /// Carga los datos necesarios para los controles desplegables del formulario.
    /// </summary>
    private async Task LoadSelectListsAsync()
    {
        var vehicles = await _vehicleRepo.GetAllAsync();

        // Proyectamos los datos agregando la información del cliente
        var vehicleOptions = vehicles.Select(v => new
        {
            Id = v.Id,
            DisplayText = $"{v.LicensePlate} - {v.Brand} {v.Model} (Propietario: {v.Client?.FirstName} {v.Client?.LastName})"
        });

        // "Id" será el valor enviado al servidor (VehicleId) y "DisplayText" el texto visible en el <select>
        VehiclesList = new SelectList(vehicleOptions, "Id", "DisplayText", ReceivingOrder?.VehicleId);

        // Carga de FuelLevels activos desde la base de datos
        var fuelLevels = await _context.FuelLevels
            .Where(f => f.IsActive)
            .OrderBy(f => f.Id)
            .ToListAsync();

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
