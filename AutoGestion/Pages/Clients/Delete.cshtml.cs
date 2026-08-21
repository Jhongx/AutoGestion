using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.ClientPages;

public class DeleteModel : PageModel
{
    private readonly IClientRepository _clientRepository;

    public DeleteModel(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    [BindProperty]
    public Client Client { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        // Se utiliza el repositorio para traer al cliente junto con sus vehículos
        var client = await _clientRepository.GetByIdAsync(id.Value);

        if (client is null)
        {
            return NotFound();
        }

        Client = client;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var clientToDelete = await _clientRepository.GetByIdAsync(id.Value);

        if (clientToDelete != null)
        {
            await _clientRepository.DeleteAsync(clientToDelete.Id);
        }

        return RedirectToPage("./Index");
    }
}
