using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AutoGestion.Models;
using AutoGestion.Data;

namespace AutoGestion.Pages.ReceivingOrderPages;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public ReceivingOrder ReceivingOrder { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var receivingorder = await _context.ReceivingOrders.FirstOrDefaultAsync(m => m.Id == id);
        if (receivingorder is null)
        {
            return NotFound();
        }
        else
        {
            ReceivingOrder = receivingorder;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var receivingorder = await _context.ReceivingOrders.FindAsync(id);
        if (receivingorder != null)
        {
            ReceivingOrder = receivingorder;
            _context.ReceivingOrders.Remove(ReceivingOrder);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
