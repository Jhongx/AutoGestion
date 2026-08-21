using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore; 
namespace AutoGestion.Repositories.Implementations
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles
                .Where(v => v.IsActive)
                .Include(v => v.Client)
                .AsNoTracking()
                .OrderBy(v => v.Brand)
                .ThenBy(v => v.Model)
                .ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            return await _context.Vehicles
                .Where(v => v.IsActive)
                .Include(v => v.Client)
                .Include(v => v.ReceivingOrders)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                return null;

            string normalizedPlate = licensePlate.Trim().ToUpper();

            return await _context.Vehicles
                .Where(v => v.IsActive)
                .Include(v => v.Client)
                .FirstOrDefaultAsync(v => v.LicensePlate.ToUpper() == normalizedPlate);
        }

        public async Task<IEnumerable<Vehicle>> GetByClientIdAsync(int clientId)
        {
            return await _context.Vehicles
                .Where(v => v.IsActive && v.ClientId == clientId)
                .AsNoTracking()
                .OrderByDescending(v => v.Year)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Vehicle> Vehicles, int TotalCount)> GetPagedAsync(
            string? searchTerm,
            int pageIndex,
            int pageSize)
        {
            var query = _context.Vehicles
                .Where(v => v.IsActive)
                .Include(v => v.Client)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.Trim().ToLower();
                query = query.Where(v =>
                    v.LicensePlate.ToLower().Contains(term) ||
                    v.Brand.ToLower().Contains(term) ||
                    v.Model.ToLower().Contains(term) ||
                    (v.Client != null && (v.Client.FirstName.ToLower().Contains(term) || v.Client.LastName.ToLower().Contains(term)))
                );
            }

            int totalCount = await query.CountAsync();

            var vehicles = await query
                .OrderByDescending(v => v.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (vehicles, totalCount);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Vehicles.AnyAsync(v => v.IsActive && v.Id == id);
        }

        public async Task<bool> LicensePlateExistsAsync(string licensePlate, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                return false;

            string normalizedPlate = licensePlate.Trim().ToUpper();

            var query = _context.Vehicles.Where(v => v.IsActive).AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(v => v.Id != excludeId.Value);
            }

            return await query.AnyAsync(v => v.LicensePlate.ToUpper() == normalizedPlate);
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
        }

        public void Update(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
        }

        public void Delete(Vehicle vehicle)
        {
            vehicle.IsActive = false;
            _context.Vehicles.Update(vehicle);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
