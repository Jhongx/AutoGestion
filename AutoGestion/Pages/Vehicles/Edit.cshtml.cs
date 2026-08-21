using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.VehiclePages;

public class EditModel : PageModel
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IClientRepository _clientRepository;

    public EditModel(IVehicleRepository vehicleRepository, IClientRepository clientRepository)
    {
        _vehicleRepository = vehicleRepository;
        _clientRepository = clientRepository;
    }

    [BindProperty]
    public Vehicle Vehicle { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var vehicle = await _vehicleRepository.GetByIdAsync(id.Value);
        if (vehicle is null)
        {
            return NotFound();
        }

        Vehicle = vehicle;

        // Cargamos el SelectList seleccionando por defecto el cliente actual
        await LoadClientsSelectListAsync(Vehicle.ClientId);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadClientsSelectListAsync(Vehicle.ClientId);
            return Page();
        }

        // Validar placa duplicada excluyendo el vehículo actual
        if (await _vehicleRepository.LicensePlateExistsAsync(Vehicle.LicensePlate, Vehicle.Id))
        {
            ModelState.AddModelError("Vehicle.LicensePlate", "La placa ingresada ya pertenece a otro vehículo.");
            await LoadClientsSelectListAsync(Vehicle.ClientId);
            return Page();
        }

        try
        {
            _vehicleRepository.Update(Vehicle); // Se marca la entidad como modificada
            await _vehicleRepository.SaveChangesAsync(); // Se persisten los cambios en la BD
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _vehicleRepository.ExistsAsync(Vehicle.Id))
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

    private async Task LoadClientsSelectListAsync(int? selectedClientId = null)
    {
        var clients = await _clientRepository.GetAllAsync();

        ViewData["ClientId"] = new SelectList(
            clients.Select(c => new {
                c.Id,
                FullName = c.FullName
            }),
            "Id",
            "FullName",
            selectedClientId
        );
    }
}
