using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoGestion.Repositories.Implementations
{
    public class DocTypeRepository : IDocTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public DocTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DocType>> GetAllAsync()
        {
            return await _context.DocTypes
                .AsNoTracking()
                .OrderBy(d => d.Code)
                .ToListAsync();
        }
    }
}
