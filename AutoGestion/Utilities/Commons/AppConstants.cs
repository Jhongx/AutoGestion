using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoGestion.Utilities.Commons
{
    public static class AppConstants
    {
        // ============ ENUMS ============
        public enum Transmission
        {
            Manual,
            Automatic
        }

        public enum FuelLevel
        {
            Reserve,
            Quarter,
            Half,
            ThreeQuarters,
            Full
        }
        public enum DocumentType
        {
            PhysicalCard = 1,          // 01 - Cédula Física
            LegalEntityCard = 2,       // 02 - Cédula Jurídica
            DIMEX = 3,                 // 03 - DIMEX
            NITE = 4,                  // 04 - NITE
            NonResidentForeigner = 5,  // 05 - Extranjero No Domiciliado
            NonTaxpayer = 6            // 06 - No Contribuyente
        }

        // ============ DISPLAY NAMES PARA ENUMS ============
        public static class TransmissionDisplayNames
        {
            public const string Manual = "Manual";
            public const string Automatic = "Automática";

            public static readonly Dictionary<Transmission, string> DisplayNames = new()
        {
            { Transmission.Manual, "Manual" },
            { Transmission.Automatic, "Automática" }
        };
        }

        // ============ STRING LENGTHS ============
        public static class StringLengths
        {
            // Cliente
            public const int FullName = 150;
            public const int Identification = 20;
            public const int Phone = 15;
            public const int Email = 100;

            // Vehiculo
            public const int LicensePlate = 15;
            public const int BrandModel = 100;

            // OrdenRecepcion
            public const int ServiceType = 100;

            // Inventario
            public const int Code = 30;
            public const int Name = 100;
        }



        // ============ DEFAULT VALUES ============
        public static class DefaultValues
        {
            public const int CurrentYear = 2026; // O puedes usar DateTime.Now.Year
        }

        public enum MovementType
        {
            Inbound,  // Ingreso de stock / Compra
            Outbound  // Salida de stock / Venta
        }

        public enum ReceivingOrderStatus
        {
            Pending,     // Pendiente de diagnóstico / revisión
            InProgress,  // En proceso de reparación
            Completed,   // Finalizado / Listo para entrega
            Delivered    // Entregado al cliente
        }

        public static class ReceivingOrderStatusDisplayNames
        {
            public static readonly Dictionary<ReceivingOrderStatus, (string Name, string BadgeClass)> DisplayNames = new()
            {
                { ReceivingOrderStatus.Pending, ("Pendiente", "bg-warning text-dark border border-warning-subtle") },
                { ReceivingOrderStatus.InProgress, ("En Proceso", "bg-info text-dark border border-info-subtle") },
                { ReceivingOrderStatus.Completed, ("Completado", "bg-success text-white") },
                { ReceivingOrderStatus.Delivered, ("Entregado", "bg-secondary text-white") }
            };

            // Método auxiliar por si guardas el estado como texto plano en la BD
            public static (string Name, string BadgeClass) GetInfo(string? status)
            {
                if (Enum.TryParse<ReceivingOrderStatus>(status, out var parsed))
                {
                    return DisplayNames.TryGetValue(parsed, out var info) ? info : (status ?? "Desconocido", "bg-light text-dark border");
                }
                // Fallback por si el texto no coincide exactamente con el enum
                return (string.IsNullOrWhiteSpace(status) ? "Pendiente" : status, "bg-light text-dark border");
            }
        }

        // NUEVO: Catálogo centralizado de Tipos de Servicio
        public enum ServiceTypeCategory
        {
            RutinaryMaintenance,
            EngineDiagnostic,
            InjectionLaboratory,
            AutomotiveElectricity,
            AirConditioning,
            BrakesSuspension
        }

        public static class ServiceTypeDisplayNames
        {
            public static readonly Dictionary<ServiceTypeCategory, string> Options = new()
            {
                { ServiceTypeCategory.RutinaryMaintenance, "Mantenimiento Rutinario" },
                { ServiceTypeCategory.EngineDiagnostic, "Diagnóstico / Falla Motor" },
                { ServiceTypeCategory.InjectionLaboratory, "Inyección / Laboratorio" },
                { ServiceTypeCategory.AutomotiveElectricity, "Electricidad Automotriz" },
                { ServiceTypeCategory.AirConditioning, "Aire Acondicionado" },
                { ServiceTypeCategory.BrakesSuspension, "Frenos / Suspensión" }
            };

            // Método auxiliar para poblar SelectLists fácilmente en tus PageModels
            public static SelectList GetSelectList(string? selectedValue = null)
            {
                var list = Options.Values.Select(v => new { Value = v, Text = v });
                return new SelectList(list, "Value", "Text", selectedValue);
            }
        }

        // ============ VALIDATION MESSAGES ============
        public static class ValidationMessages
        {
            public const string RequiredField = "Este campo es obligatorio";
            public const string InvalidEmail = "El correo electrónico no es válido";
            public const string MaxLengthExceeded = "La longitud máxima permitida es de {0} caracteres";
        }



    }
}
