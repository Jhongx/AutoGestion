using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.ReceivingOrderPages;

public class DetailsModel : PageModel
{
    private readonly IReceivingOrderRepository _receivingOrderRepository;

    public DetailsModel(IReceivingOrderRepository receivingOrderRepository)
    {
        _receivingOrderRepository = receivingOrderRepository;
    }

    public ReceivingOrder ReceivingOrder { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        // GetByIdAsync incluye el objeto Vehicle y su Client asociado
        var receivingOrder = await _receivingOrderRepository.GetByIdAsync(id.Value);

        if (receivingOrder is null)
        {
            return NotFound();
        }

        ReceivingOrder = receivingOrder;
        return Page();
    }
}
