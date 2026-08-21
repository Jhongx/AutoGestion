using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AutoGestion.Models;
using AutoGestion.Data;

namespace AutoGestion.Pages.VehiclePages;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Vehicle Vehicle { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var vehicle = await _context.Vehicles.FirstOrDefaultAsync(m => m.Id == id);
        if (vehicle is null)
        {
            return NotFound();
        }
        else
        {
            Vehicle = vehicle;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle != null)
        {
            Vehicle = vehicle;
            _context.Vehicles.Remove(Vehicle);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
