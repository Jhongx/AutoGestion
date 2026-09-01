using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using AutoGestion.Utilities.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static AutoGestion.Utilities.Commons.AppConstants;

namespace AutoGestion.Pages.ReceivingOrderPages;

public class CreateModel : PageModel
{
    private readonly IReceivingOrderRepository _receivingOrderRepo;
    private readonly IVehicleRepository _vehicleRepo;
    private readonly IClientRepository _clientRepo;
    private readonly IInspectionAppointmentRepository _appointmentRepo; // <-- Repositorio de Citas incorporado
    private readonly ApplicationDbContext _context;

    public CreateModel(
        IReceivingOrderRepository receivingOrderRepo,
        IVehicleRepository vehicleRepo,
        IClientRepository clientRepo,
        IInspectionAppointmentRepository appointmentRepo, // <-- Inyección en el constructor
        ApplicationDbContext context)
    {
        _receivingOrderRepo = receivingOrderRepo;
        _vehicleRepo = vehicleRepo;
        _clientRepo = clientRepo;
        _appointmentRepo = appointmentRepo;
        _context = context;
    }

    [BindProperty]
    public ReceivingOrder ReceivingOrder { get; set; } = default!;

    // Parámetro opcional para rastrear si viene de una cita y asociarla
    [BindProperty(SupportsGet = true)]
    public int? AppointmentId { get; set; }

    // Propiedad para llenar el <select> de vehículos en la vista .cshtml
    public SelectList VehiclesList { get; set; } = default!;

    // Propiedad para llenar el <select> de niveles de combustible
    public SelectList FuelLevelsList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? appointmentId, int? vehicleId, int? clientId, string? reason)
    {
        // 1. Capturamos el ID de la cita
        if (appointmentId.HasValue)
        {
            AppointmentId = appointmentId.Value;

            // Consultamos la cita directamente en la base de datos para extraer sus datos de forma segura
            var appointment = await _appointmentRepo.GetByIdAsync(AppointmentId.Value);
            if (appointment != null)
            {
                // Mapeamos los valores de la cita a la orden de recepción
                // (Asegúrate de que appointment.VehicleId y appointment.Reason coincidan con los nombres de propiedades de tu modelo de Citas)
                vehicleId = appointment.VehicleId;
                reason = appointment.Reason; // O la propiedad que guarde el motivo en tu cita (ej: Description, Notes, etc.)
            }
        }

        // 2. Inicializamos la orden de recepción con los valores ya resueltos
        ReceivingOrder = new ReceivingOrder
        {
            DateTime = DateTime.Now,
            VehicleId = vehicleId ?? 0,
            ProblemDescription = reason ?? string.Empty,
            Status = AppConstants.ReceivingOrderStatusDisplayNames.DisplayNames[ReceivingOrderStatus.Pending].Name // Guarda "Pendiente"
        };

        // 3. Cargamos las listas desplegables (ahora ReceivingOrder.VehicleId ya tiene valor, por lo que el SelectList lo marcará como seleccionado)
        await LoadSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Si la validación falla, se recargan las listas antes de volver a la vista
            await LoadSelectListsAsync();
            return Page();
        }

        // 1. Guardamos la nueva orden de recepción
        await _receivingOrderRepo.AddAsync(ReceivingOrder);

        // 2. Si la orden se generó a partir de una cita, actualizamos el estado de la cita
        if (AppointmentId.HasValue)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(AppointmentId.Value);
            if (appointment != null)
            {
                appointment.Status = "Convertida a Orden"; // Ajusta el texto según los estados que manejes
                await _appointmentRepo.UpdateAsync(appointment);
            }
        }

        return RedirectToPage("./Index");
    }

    /// <summary>
    /// Carga los datos necesarios para los controles desplegables del formulario.
    /// </summary>
    private async Task LoadSelectListsAsync()
    {
        var vehicles = await _vehicleRepo.GetAllAsync();

        // Proyectamos los datos agregando la información del cliente
        var vehicleOptions = vehicles.Select(v => new
        {
            Id = v.Id,
            DisplayText = $"{v.LicensePlate} - {v.Brand} {v.Model} (Propietario: {v.Client?.FirstName} {v.Client?.LastName})"
        });

        // "Id" será el valor enviado al servidor (VehicleId) y "DisplayText" el texto visible en el <select>
        VehiclesList = new SelectList(vehicleOptions, "Id", "DisplayText", ReceivingOrder?.VehicleId);

        // Carga de FuelLevels activos desde la base de datos
        var fuelLevels = await _context.FuelLevels
            .Where(f => f.IsActive)
            .OrderBy(f => f.Id)
            .ToListAsync();

        FuelLevelsList = new SelectList(
            fuelLevels,
            "Id",
            "Name",
            ReceivingOrder?.FuelLevelId
        );
    }

    public async Task<JsonResult> OnGetVehicleClientInfoAsync(int vehicleId)
    {
        var vehicle = await _vehicleRepo.GetByIdAsync(vehicleId);

        if (vehicle?.Client != null)
        {
            return new JsonResult(new
            {
                success = true,
                clientName = $"{vehicle.Client.FirstName} {vehicle.Client.LastName}",
                phone = vehicle.Client.PrimaryPhone ?? "Sin teléfono"
            });
        }

        return new JsonResult(new { success = false });
    }
}
