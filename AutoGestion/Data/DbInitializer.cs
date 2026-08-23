using AutoGestion.Models;

namespace AutoGestion.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            // 1. Catálogo de Tipos de Documento (DocTypes)
            if (!context.DocTypes.Any())
            {
                context.DocTypes.AddRange(
                    new DocType { Code = "01", Name = "Cédula Física" },
                    new DocType { Code = "02", Name = "Cédula Jurídica" },
                    new DocType { Code = "03", Name = "DIMEX" },
                    new DocType { Code = "04", Name = "NITE" },
                    new DocType { Code = "05", Name = "Extranjero No Domiciliado" },
                    new DocType { Code = "06", Name = "No Contribuyente" }
                );
            }

            // 2. Catálogo de Niveles de Combustible (FuelLevels)
            if (!context.FuelLevels.Any())
            {
                context.FuelLevels.AddRange(
                    new FuelLevel { Name = "Reserva (E)" },
                    new FuelLevel { Name = "1/4" },
                    new FuelLevel { Name = "1/2" },
                    new FuelLevel { Name = "3/4" },
                    new FuelLevel { Name = "Lleno (F)" }
                );
            }

            // Guardar los cambios
            context.SaveChanges();
        }
    }
}
