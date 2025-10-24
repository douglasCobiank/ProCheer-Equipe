using Equipe.Infrastructure.Data.Models;

namespace Equipe.Infrastructure.Repositories
{
    public interface IEquipeRepository
    {
        Task AddAsync(EquipeData equipeData);
        Task EditAsync(EquipeData equipeData);
        Task DeleteAsync(EquipeData equipeData);
        Task<List<EquipeData>> GetAllWithIncludeAsync();
        Task<EquipeData?> GetByIdAsync(int id);
        Task<EquipeData?> GetEquipePorNomeAsync(string nome);
        void AttachEntity<T>(T entity) where T : class;
    }
}