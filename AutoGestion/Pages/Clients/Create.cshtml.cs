using AutoGestion.Data;
using AutoGestion.Helpers;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Pages.ClientPages;

public class CreateModel : PageModel
{
    private readonly IClientRepository _clientRepository;
    private readonly IDocTypeRepository _docTypeRepository;

    public CreateModel(IClientRepository clientRepository, IDocTypeRepository docTypeRepository)
    {
        _clientRepository = clientRepository;
        _docTypeRepository = docTypeRepository;
    }

    [BindProperty]
    public Client Client { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDocTypesSelectListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDocTypesSelectListAsync();
            return Page();
        }

        await _clientRepository.AddAsync(Client);

        // Activamos la alerta interactiva preguntando por el vehículo
        SweetAlertHelper.ShowConfirm(
            TempData,
            title: "¡Cliente Registrado!",
            text: "¿Deseas asignarle un vehículo a este cliente ahora mismo?",
            confirmButtonText: "Sí, registrar vehículo",
            cancelButtonText: "No, ir al listado",
            confirmRedirectUrl: $"/Vehicles/Create?clientId={Client.Id}", // Pasamos el ID por parámetro
            cancelRedirectUrl: "./Index"
        );

        return RedirectToPage("./Index");
    }

    private async Task LoadDocTypesSelectListAsync()
    {
        var docTypes = await _docTypeRepository.GetAllAsync();

        // Muestra en el select la combinación del código y nombre (Ej: "01 - Cédula Física")
        ViewData["DocTypeId"] = new SelectList(
            docTypes.Select(d => new {
                d.Id,
                DisplayName = d.Name
            }),
            "Id",
            "DisplayName"
        );
    }
}
