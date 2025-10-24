using Equipe.Core.Models;

namespace Equipe.Core.Interface
{
    public interface IEquipeService
    {
        Task AddEquipeAsync(EquipeDto equipeDto);
        Task DeletaEquipeAsync(int id);
        Task EditarEquipeAsync(EquipeDto equipeDto, int id);
        Task<List<EquipeDto>> GetEquipeAsync();
        Task<EquipeDto> GetEquipePorNomeAsync(string nome);
    }
}