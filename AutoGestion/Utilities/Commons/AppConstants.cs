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

        public static class FuelLevelDisplayNames
        {
            public const string Reserve = "Reserva";
            public const string Quarter = "1/4";
            public const string Half = "1/2";
            public const string ThreeQuarters = "3/4";
            public const string Full = "Lleno";

            public static readonly Dictionary<FuelLevel, string> DisplayNames = new()
        {
            { FuelLevel.Reserve, "Reserva" },
            { FuelLevel.Quarter, "1/4" },
            { FuelLevel.Half, "1/2" },
            { FuelLevel.ThreeQuarters, "3/4" },
            { FuelLevel.Full, "Lleno" }
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

        // ============ VALIDATION MESSAGES ============
        public static class ValidationMessages
        {
            public const string RequiredField = "Este campo es obligatorio";
            public const string InvalidEmail = "El correo electrónico no es válido";
            public const string MaxLengthExceeded = "La longitud máxima permitida es de {0} caracteres";
        }

        // ============ DEFAULT VALUES ============
        public static class DefaultValues
        {
            public const int CurrentYear = 2026; // O puedes usar DateTime.Now.Year
        }
    }
}
