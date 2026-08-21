using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.VehiclePages;

public class IndexModel : PageModel
{
    private readonly IVehicleRepository _vehicleRepository;

    public IndexModel(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    // Se mantiene 'Vehicle' para hacer match directo con tu vista Index.cshtml
    public IEnumerable<Vehicle> Vehicle { get; set; } = new List<Vehicle>();

    public async Task OnGetAsync()
    {
        Vehicle = await _vehicleRepository.GetAllAsync();
    }
}
