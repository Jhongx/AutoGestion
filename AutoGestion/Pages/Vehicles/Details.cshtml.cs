using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.VehiclePages;

public class DetailsModel : PageModel
{
    private readonly IVehicleRepository _vehicleRepository;

    public DetailsModel(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public Vehicle Vehicle { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        // GetByIdAsync ya incluye las relaciones (Client y ReceivingOrders)
        var vehicle = await _vehicleRepository.GetByIdAsync(id.Value);

        if (vehicle is null)
        {
            return NotFound();
        }

        Vehicle = vehicle;
        return Page();
    }
}
