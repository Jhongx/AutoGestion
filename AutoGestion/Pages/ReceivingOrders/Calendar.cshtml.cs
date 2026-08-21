using AutoGestion.Models.Session.DTO;
using AutoGestion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AutoGestion.Pages.ReceivingOrders
{
    public class CalendarModel : PageModel
    {
        private readonly IReceivingOrderRepository _receivingOrderRepository;

        public CalendarModel(IReceivingOrderRepository receivingOrderRepository)
        {
            _receivingOrderRepository = receivingOrderRepository;
        }

        public string EventsJson { get; set; } = "[]";

        public async Task OnGetAsync()
        {
            var orders = await _receivingOrderRepository.GetAllAsync();

            var events = orders.Select(o => new
            {
                id = o.Id,
                // Título corto para que quepa limpio dentro de la celda mensual
                title = $"#{o.Id:D5} - {o.Vehicle?.LicensePlate ?? "S/P"}",
                start = o.DateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                url = $"/ReceivingOrders/Details?id={o.Id}",
                color = GetColorByServiceType(o.ServiceType),
                extendedProps = new
                {
                    time = o.DateTime.ToString("hh:mm tt"),
                    serviceType = o.ServiceType ?? "General",
                    client = o.Vehicle?.Client != null ? $"{o.Vehicle.Client.FirstName} {o.Vehicle.Client.LastName}" : "N/A",
                    vehicle = o.Vehicle != null ? $"{o.Vehicle.Brand} {o.Vehicle.Model}" : "N/A",
                    fullTitle = $"Órden #{o.Id:D5} - {o.Vehicle?.LicensePlate ?? "Sin Placa"}"
                }
            });

            EventsJson = JsonSerializer.Serialize(events);
        }

        private static string GetColorByServiceType(string? serviceType)
        {
            return serviceType?.ToLower() switch
            {
                "mantenimiento" => "#0d6efd", // Azul
                "reparacion" or "reparación" => "#dc3545", // Rojo
                "diagnostico" or "diagnóstico" => "#ffc107", // Amarillo
                _ => "#198754" // Verde / Por defecto
            };
        }
    }
}
