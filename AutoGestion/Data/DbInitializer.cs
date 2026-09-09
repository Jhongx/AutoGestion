using AutoGestion.Models;
using AutoGestion.Models.Inventory;

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

            // 3. Catálogo de Tipos de Inventario (InventoryTypes)
            if (!context.InventoryTypes.Any())
            {
                context.InventoryTypes.AddRange(
                    new InventoryType { Name = "Filtros" },
                    new InventoryType { Name = "Lubricantes y Aceites" },
                    new InventoryType { Name = "Frenos" },
                    new InventoryType { Name = "Suspensión y Dirección" },
                    new InventoryType { Name = "Sistema Eléctrico" },
                    new InventoryType { Name = "Accesorios y Varios" },
                    new InventoryType { Name = "Otros" }
                );
            }

            // 4. Catálogo de Marcas de Inventario (InventoryBrands)
            if (!context.InventoryBrands.Any())
            {
                context.InventoryBrands.AddRange(
                    // --- Aceites y Lubricantes ---
                    new InventoryBrand { Name = "Mobil" },
                    new InventoryBrand { Name = "Castrol" },
                    new InventoryBrand { Name = "Valvoline" },
                    new InventoryBrand { Name = "Shell" },
                    new InventoryBrand { Name = "Motul" },
                    new InventoryBrand { Name = "Total" },
                    new InventoryBrand { Name = "Petro-Canada" },
                    new InventoryBrand { Name = "Gulf" },
                    new InventoryBrand { Name = "Kendall" },
                    new InventoryBrand { Name = "Liqui Moly" },
                    new InventoryBrand { Name = "Red Line" },
                    new InventoryBrand { Name = "Amsoil" },

                    // --- Filtros y Repuestos de Motor ---
                    new InventoryBrand { Name = "Mann Filter" },
                    new InventoryBrand { Name = "Bosch" },
                    new InventoryBrand { Name = "Fram" },
                    new InventoryBrand { Name = "Denso" },
                    new InventoryBrand { Name = "NGK" },
                    new InventoryBrand { Name = "Mahle" },
                    new InventoryBrand { Name = "Wix" },
                    new InventoryBrand { Name = "Purflux" },
                    new InventoryBrand { Name = "K&N" },
                    new InventoryBrand { Name = "Hengst" },
                    new InventoryBrand { Name = "Champion" },
                    new InventoryBrand { Name = "ACDelco" },

                    // --- Frenos y Suspensión ---
                    new InventoryBrand { Name = "Brembo" },
                    new InventoryBrand { Name = "Monroe" },
                    new InventoryBrand { Name = "Kayaba (KYB)" },
                    new InventoryBrand { Name = "Wagner" },
                    new InventoryBrand { Name = "Akebono" },
                    new InventoryBrand { Name = "EBC Brakes" },
                    new InventoryBrand { Name = "Power Stop" },
                    new InventoryBrand { Name = "Centric" },
                    new InventoryBrand { Name = "Bilstein" },
                    new InventoryBrand { Name = "Sachs" },
                    new InventoryBrand { Name = "TRW" },
                    new InventoryBrand { Name = "ATE" },

                    // --- Repuestos Originales (OEM) ---
                    new InventoryBrand { Name = "Mobis" },          // Hyundai/Kia
                    new InventoryBrand { Name = "Motorcraft" },     // Ford
                    new InventoryBrand { Name = "Mopar" },          // Chrysler/Dodge/Jeep
                    new InventoryBrand { Name = "Honda" },          // Repuestos originales Honda
                    new InventoryBrand { Name = "Nissan" },         // Repuestos originales Nissan
                    new InventoryBrand { Name = "Mazda" },          // Repuestos originales Mazda
                    new InventoryBrand { Name = "Subaru" },         // Repuestos originales Subaru
                    new InventoryBrand { Name = "Mitsubishi" },     // Repuestos originales Mitsubishi
                    new InventoryBrand { Name = "Toyota" },         // Repuestos originales Toyota
                    new InventoryBrand { Name = "Chevrolet" },      // Repuestos originales Chevrolet

                    // --- Marcas de Fabricantes de Vehículos ---
                    new InventoryBrand { Name = "Ford" },
                    new InventoryBrand { Name = "Volkswagen" },
                    new InventoryBrand { Name = "BMW" },
                    new InventoryBrand { Name = "Mercedes-Benz" },
                    new InventoryBrand { Name = "Audi" },
                    new InventoryBrand { Name = "Peugeot" },
                    new InventoryBrand { Name = "Renault" },
                    new InventoryBrand { Name = "Fiat" },
                    new InventoryBrand { Name = "Volvo" },
                    new InventoryBrand { Name = "Jeep" },

                    // --- Genéricos / Otros ---
                    new InventoryBrand { Name = "Genérica" },
                    new InventoryBrand { Name = "Otros" }
                );
            }

            // Guardar los cambios
            context.SaveChanges();
        }
    }
}
