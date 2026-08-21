using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.ReceivingOrderPages;

public class IndexModel : PageModel
{
    private readonly IReceivingOrderRepository _receivingOrderRepository;

    public IndexModel(IReceivingOrderRepository receivingOrderRepository)
    {
        _receivingOrderRepository = receivingOrderRepository;
    }

    public IList<ReceivingOrder> ReceivingOrder { get; set; } = new List<ReceivingOrder>();

    public async Task OnGetAsync()
    {
        // Usamos el repositorio para traer las órdenes incluyendo la información del vehículo
        var orders = await _receivingOrderRepository.GetAllAsync();

        ReceivingOrder = orders.ToList();
    }
}
