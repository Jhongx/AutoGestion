using AutoGestion.Data;
using AutoGestion.Models;
using AutoGestion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace AutoGestion.Repositories.Implementations
{
    public class ReceivingOrderRepository : IReceivingOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public ReceivingOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReceivingOrder>> GetAllAsync()
        {
            return await _context.ReceivingOrders
                .Where(ro => ro.IsActive)
                .Include(ro => ro.Vehicle)
                    .ThenInclude(v => v!.Client)
                .Include(ro => ro.FuelLevel)
                .AsNoTracking()
                .OrderByDescending(ro => ro.EntryDate)
                .ToListAsync();
        }

        public async Task<ReceivingOrder?> GetByIdAsync(int id)
        {
            return await _context.ReceivingOrders
                .Where(ro => ro.IsActive)
                .Include(ro => ro.Vehicle)
                    .ThenInclude(v => v!.Client)
                .Include(ro => ro.FuelLevel)
                .FirstOrDefaultAsync(ro => ro.Id == id);
        }

        public async Task<IEnumerable<ReceivingOrder>> GetByVehicleIdAsync(int vehicleId)
        {
            return await _context.ReceivingOrders
                .Where(ro => ro.IsActive && ro.VehicleId == vehicleId)
                .Include(ro => ro.FuelLevel)
                .AsNoTracking()
                .OrderByDescending(ro => ro.EntryDate)
                .ToListAsync();
        }

        public async Task<(IEnumerable<ReceivingOrder> Orders, int TotalCount)> GetPagedAsync(
            string? searchTerm,
            string? statusFilter,
            int pageIndex,
            int pageSize)
        {
            var query = _context.ReceivingOrders
                .Where(ro => ro.IsActive)
                .Include(ro => ro.Vehicle)
                    .ThenInclude(v => v!.Client)
                .Include(ro => ro.FuelLevel)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                query = query.Where(ro => ro.Status == statusFilter);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.Trim().ToLower();
                query = query.Where(ro =>
                    ro.ProblemDescription.ToLower().Contains(term) ||
                    ro.ServiceType.ToLower().Contains(term) ||
                    (ro.Vehicle != null && (
                        ro.Vehicle.LicensePlate.ToLower().Contains(term) ||
                        ro.Vehicle.Brand.ToLower().Contains(term) ||
                        ro.Vehicle.Model.ToLower().Contains(term)
                    )) ||
                    (ro.Vehicle != null && ro.Vehicle.Client != null && (
                        ro.Vehicle.Client.FirstName.ToLower().Contains(term) ||
                        ro.Vehicle.Client.LastName.ToLower().Contains(term)
                    ))
                );
            }

            int totalCount = await query.CountAsync();

            var orders = await query
                .OrderByDescending(ro => ro.EntryDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalCount);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ReceivingOrders.AnyAsync(ro => ro.IsActive && ro.Id == id);
        }

        public async Task AddAsync(ReceivingOrder receivingOrder)
        {
            await _context.ReceivingOrders.AddAsync(receivingOrder);
            await _context.SaveChangesAsync();
        }

        public void Update(ReceivingOrder receivingOrder)
        {
            _context.ReceivingOrders.Update(receivingOrder);
            _context.SaveChangesAsync();
        }

        public void Delete(ReceivingOrder receivingOrder)
        {
            receivingOrder.IsActive = false;
            _context.ReceivingOrders.Update(receivingOrder);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
