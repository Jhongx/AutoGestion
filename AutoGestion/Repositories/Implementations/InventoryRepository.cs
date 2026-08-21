using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace AutoGestion.Repositories.Implementations
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todo el catálogo activo
        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _context.Set<Inventory>()
                .AsNoTracking()
                .Where(i => i.IsActive)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        // Obtener por ID (Solo si está activo)
        public async Task<Inventory?> GetByIdAsync(int id)
        {
            return await _context.Set<Inventory>()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id && i.IsActive);
        }

        // Obtener repuesto por código único (Solo si está activo)
        public async Task<Inventory?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;

            var term = code.Trim();

            return await _context.Set<Inventory>()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.IsActive && EF.Functions.Like(i.Code, term));
        }

        // Obtener repuestos activos con stock bajo o igual al umbral
        public async Task<IEnumerable<Inventory>> GetLowStockAsync(int threshold = 5)
        {
            return await _context.Set<Inventory>()
                .AsNoTracking()
                .Where(i => i.IsActive && i.CurrentStock <= threshold)
                .OrderBy(i => i.CurrentStock)
                .ToListAsync();
        }

        // Buscar coincidencias activas por nombre o código
        public async Task<IEnumerable<Inventory>> SearchByNameOrCodeAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var term = $"%{searchTerm.Trim()}%";

            return await _context.Set<Inventory>()
                .AsNoTracking()
                .Where(i => i.IsActive && (EF.Functions.Like(i.Name, term) || EF.Functions.Like(i.Code, term)))
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        // Persistencia (CRUD)
        public async Task AddAsync(Inventory entity)
        {
            entity.IsActive = true;
            await _context.Set<Inventory>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Inventory entity)
        {
            _context.Set<Inventory>().Update(entity);
            await _context.SaveChangesAsync();
        }

        // Soft Delete (Borrado Lógico)
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Inventory>().FindAsync(id);
            if (entity != null && entity.IsActive)
            {
                entity.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
