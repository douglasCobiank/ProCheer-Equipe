using Equipe.Core.Models;
using Equipe.Infrastructure.Data.Models;
using Refit;

namespace Equipe.Core.Interface.API
{
    public interface IAtletaService
    {
        [Post("/api/Atleta/criar-atleta")]
        Task CriarAtletaAsync([Body] AtletaDto atletaDto);

        [Put("/api/Atleta/editar-atleta/{id}")]
        Task EditarAtletaAsync(int id, [Body] AtletaDto atletaDto);

        [Delete("/api/Atleta/deletar-atleta/{id}")]
        Task DeletarAtletaAsync(int id);

        [Get("/api/Atleta/buscar-atleta")]
        Task BuscarAtletasAsync();

        [Get("/api/Atleta/buscar-atleta-por-nome/{nome}")]
        Task<AtletaData> BuscarAtletaPorNomeAsync(string nome);
    }
}