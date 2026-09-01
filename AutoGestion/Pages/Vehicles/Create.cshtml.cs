using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.VehiclePages;

public class CreateModel : PageModel
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IClientRepository _clientRepository;

    public CreateModel(IVehicleRepository vehicleRepository, IClientRepository clientRepository)
    {
        _vehicleRepository = vehicleRepository;
        _clientRepository = clientRepository;
    }

    [BindProperty]
    public Vehicle Vehicle { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public int? ClientId { get; set; } // Parámetro opcional que viene por la URL

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadClientsSelectListAsync(ClientId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Si la validación falla, recargamos el SelectList antes de volver a mostrar la página
            await LoadClientsSelectListAsync();
            return Page();
        }

        await _vehicleRepository.AddAsync(Vehicle);
        await _vehicleRepository.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private async Task LoadClientsSelectListAsync(int? selectedClientId = null)
    {
        var clients = await _clientRepository.GetAllAsync();

        // Si viene un ClientId por URL, lo asignamos como seleccionado en el SelectList
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
