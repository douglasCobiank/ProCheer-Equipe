using Equipe.Core.Models;

namespace Equipe.Core.Interface
{
    public interface IEquipeHandler
    {
        Task<List<EquipeDto>> BuscaEquipeAsync();

        Task<EquipeDto> BuscaEquipePorNomeAsync(string nome);

        Task CadastrarEquipeHandler(EquipeDto equipeDto);

        Task DeletaEquipeAsync(int id);

        Task EditarEquipeAsync(EquipeDto equipeDto, int id);
    }
}