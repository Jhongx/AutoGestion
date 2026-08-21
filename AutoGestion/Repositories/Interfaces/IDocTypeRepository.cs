using AutoGestion.Models;

namespace AutoGestion.Repositories.Interfaces
{
    public interface IDocTypeRepository
    {
        Task<List<DocType>> GetAllAsync();
    }
}
