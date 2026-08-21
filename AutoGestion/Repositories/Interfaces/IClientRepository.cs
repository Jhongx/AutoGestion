using AutoGestion.Models;

namespace AutoGestion.Repositories.Interfaces
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task AddAsync(Client client);
        Task DeleteAsync(int id);
    }
}
