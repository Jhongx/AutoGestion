using AutoGestion.Data;
using AutoGestion.Models.Inventory;
using AutoGestion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace AutoGestion.Repositories.Implementations
{
    public class InventoryTypeRepository : IInventoryTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryType>> GetAllAsync()
        {
            return await _context.InventoryTypes
                .AsNoTracking()
                .Where(t => t.IsActive) // Opcional: filtrar solo activos por defecto
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<InventoryType?> GetByIdAsync(int id)
        {
            return await _context.InventoryTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
