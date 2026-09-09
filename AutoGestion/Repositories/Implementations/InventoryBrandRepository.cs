using AutoGestion.Data;
using AutoGestion.Models.Inventory;
using AutoGestion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace AutoGestion.Repositories.Implementations
{
    public class InventoryBrandRepository : IInventoryBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryBrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryBrand>> GetAllAsync()
        {
            return await _context.InventoryBrands
                .AsNoTracking()
                .Where(b => b.IsActive) // Opcional: filtrar solo activos por defecto
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<InventoryBrand?> GetByIdAsync(int id)
        {
            return await _context.InventoryBrands
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
