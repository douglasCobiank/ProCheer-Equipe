using Equipe.Core.Interface;
using Equipe.Core.Models;
using Microsoft.Extensions.Logging;

namespace Equipe.Core
{
    public class EquipeHandler(IEquipeService equipeService, ILogger<EquipeHandler> logger) : IEquipeHandler
    {
        private readonly IEquipeService _equipeService = equipeService;
        private readonly ILogger<EquipeHandler> _logger = logger;

        public async Task<List<EquipeDto>> BuscaEquipeAsync()
        {
            try
            {
                _logger.LogInformation("Buscando todos os equipe...");
                var equipe = await _equipeService.GetEquipeAsync();
                _logger.LogInformation("{Count} equipe encontrados.", equipe.Count);
                return equipe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar lista de equipe.");
                throw;
            }
        }

        public async Task<EquipeDto> BuscaEquipePorNomeAsync(string nome)
        {
            try
            {
                _logger.LogInformation("Buscando equipe pelo nome: {Nome}", nome);               
                var equipe = await _equipeService.GetEquipePorNomeAsync(nome);

                if (equipe is null)
                    _logger.LogWarning("equipe com o nome {Nome} não encontrado.", nome);
                else
                    _logger.LogInformation("equipe {Nome} encontrado com sucesso.", nome);

                return equipe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar equipe pelo nome: {Nome}", nome);
                throw;
            }
        }

        public async Task CadastrarEquipeHandler(EquipeDto equipeDto)
        {
            try
            {
                _logger.LogInformation("Iniciando cadastro do equipe: {Nome}", equipeDto.NomeEquipe);
                await _equipeService.AddEquipeAsync(equipeDto);
                _logger.LogInformation("equipe {Nome} cadastrado com sucesso.", equipeDto.NomeEquipe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar equipe: {Nome}", equipeDto.NomeEquipe);
                throw;
            }
        }

        public async Task DeletaEquipeAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deletando equipe com ID: {Id}", id);
                await _equipeService.DeletaEquipeAsync(id);
                _logger.LogInformation("Equipe com ID {Id} deletado com sucesso.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar equipe com ID {Id}", id);
                throw;
            }
        }

        public async Task EditarEquipeAsync(EquipeDto equipeDto, int id)
        {
            try
            {
                _logger.LogInformation("Editando equipe com ID: {Id}", id);
                await _equipeService.EditarEquipeAsync(equipeDto, id);
                _logger.LogInformation("equipe {Nome} atualizado com sucesso.", equipeDto.NomeEquipe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar atleta com ID {Id}", id);
                throw;
            }
        }
    }
}