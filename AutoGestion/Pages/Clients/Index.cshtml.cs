using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.ClientPages;

public class IndexModel : PageModel
{
    private readonly IClientRepository _clientRepository;

    public IndexModel(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public IList<Client> Client { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Client = await _clientRepository.GetAllAsync();
    }
}
