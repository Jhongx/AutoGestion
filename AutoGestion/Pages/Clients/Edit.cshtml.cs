using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.ClientPages;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IDocTypeRepository _docTypeRepository;

    public EditModel(ApplicationDbContext context, IDocTypeRepository docTypeRepository)
    {
        _context = context;
        _docTypeRepository = docTypeRepository;
    }

    [BindProperty]
    public Client Client { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var client = await _context.Clients.FirstOrDefaultAsync(m => m.Id == id);
        if (client is null)
        {
            return NotFound();
        }

        Client = client;

        // Cargar la lista desplegable marcando el valor actual del cliente
        await LoadDocTypesSelectListAsync(Client.DocTypeId);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Si la validación falla, recargar el desplegable manteniendo la selección enviada
            await LoadDocTypesSelectListAsync(Client.DocTypeId);
            return Page();
        }

        _context.Attach(Client).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(Client.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool ClientExists(int id)
    {
        return _context.Clients.Any(e => e.Id == id);
    }

    private async Task LoadDocTypesSelectListAsync(int? selectedDocTypeId = null)
    {
        var docTypes = await _docTypeRepository.GetAllAsync();

        ViewData["DocTypeId"] = new SelectList(
            docTypes.Select(d => new {
                d.Id,
                DisplayName = d.Name
            }),
            "Id",
            "DisplayName",
            selectedDocTypeId
        );
    }
}
