using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.ClientPages;

public class DetailsModel : PageModel
{
    private readonly IClientRepository _clientRepository;

    public DetailsModel(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public Client Client { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        // Se obtiene el cliente con la lista de vehículos desde el repositorio
        var client = await _clientRepository.GetByIdAsync(id.Value);

        if (client is null)
        {
            return NotFound();
        }

        Client = client;

        return Page();
    }
}
