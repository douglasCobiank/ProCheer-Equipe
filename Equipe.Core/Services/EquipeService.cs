using AutoMapper;
using Equipe.Core.Interface;
using Equipe.Core.Interface.API;
using Equipe.Core.Models;
using Equipe.Infrastructure.Cache;
using Equipe.Infrastructure.Data.Models;
using Equipe.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Equipe.Core.Services
{
    public class EquipeService(IEquipeRepository equipeRepository,
        IMapper mapper,
        IGinasioService ginasioService,
        IAtletaService atletaService,
        ILogger<EquipeService> logger,
        ICacheService cacheService,
        IMensageriaService mensageriaService) : IEquipeService
    {
        private readonly IEquipeRepository _equipeRepository = equipeRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IGinasioService _ginasioService = ginasioService;
        private readonly ILogger<EquipeService> _logger = logger;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IMensageriaService _mensageria = mensageriaService;

        public async Task AddEquipeAsync(EquipeDto equipeDto)
        {
            _logger.LogInformation("Iniciando cadastro de equipe: {Nome}", equipeDto.NomeEquipe);

            var equipeData = MapearEquipe(equipeDto);

            // 🔹 Adiciona os relacionamentos corretamente
            foreach (var atletaDto in equipeDto.Atletas)
            {
                var atletaData = await atletaService.BuscarAtletaPorNomeAsync(atletaDto.Usuario.Nome);

                if (atletaData is not null)
                {
                    equipeData.EquipeAtletas.Add(new EquipeAtleta
                    {
                        Atleta = atletaData,
                        IdAtleta = atletaData.Id, // garante FK
                        Equipe = equipeData
                    });
                }
            }

            // 🔹 GINÁSIO
            var ginasio = await ObterOuCriarGinasiosAsync(equipeDto.Ginasio);
            if (ginasio.Id > 0)
            {
                // Ginasio já existe → anexa sem recriar
                var ginasioData = _mapper.Map<GinasioData>(ginasio);

                _equipeRepository.AttachEntity(ginasioData);
                if (ginasioData.Endereco?.Id > 0)
                    _equipeRepository.AttachEntity(ginasioData.Endereco);

                equipeData.Ginasio = ginasioData;
                equipeData.IdGinasio = ginasio.Id;
            }

            await _equipeRepository.AddAsync(equipeData);
            equipeData.EquipeAtletas.ForEach(x => x.Atleta = null);
            equipeData.EquipeAtletas.ForEach(x => x.Equipe = null);
            await AtualizarCache(equipeData.NomeEquipe, equipeData);

            _logger.LogInformation("Equipe {Nome} cadastrado com sucesso", equipeDto.NomeEquipe);
        }

        public async Task DeletaEquipeAsync(int id)
        {
            _logger.LogInformation("Tentando deletar equipe com ID: {Id}", id);
            var atleta = await _equipeRepository.GetByIdAsync(id);
            if (atleta is null)
            {
                _logger.LogWarning("Equipe com ID {Id} não encontrado para exclusão", id);
                return;
            }

            await RemoverCache(atleta.NomeEquipe);

            await _equipeRepository.DeleteAsync(atleta);
            _logger.LogInformation("Equipe com ID {Id} deletado com sucesso", id);

        }

        public async Task EditarEquipeAsync(EquipeDto equipeDto, int id)
        {
            _logger.LogInformation("Editando equipe com ID: {Id}", id);

            var atleta = await _equipeRepository.GetByIdAsync(id);
            if (atleta is null)
            {
                _logger.LogWarning("Equipe com ID {Id} não encontrado para edição", id);
                return;
            }

            await _equipeRepository.EditAsync(atleta);
            await RemoverCache(atleta.NomeEquipe);

            _logger.LogInformation("Atleta {Nome} atualizado com sucesso", atleta.NomeEquipe);
        }

        public async Task<List<EquipeDto>> GetEquipeAsync()
        {
            const string cacheKey = "equipe_todos";

            var cached = await _cacheService.GetAsync<List<EquipeDto>>(cacheKey);
            if (cached is not null) return cached;

            var equipes = await _equipeRepository.GetAllWithIncludeAsync();

            var equipeDto = _mapper.Map<List<EquipeDto>>(equipes);
            await _cacheService.SetAsync(cacheKey, equipeDto, TimeSpan.FromMinutes(10));

            return equipeDto;
        }

        public async Task<EquipeDto> GetEquipePorNomeAsync(string nome)
        {
            var cacheKey = $"atleta_{nome.ToLower()}";

            var cached = await _cacheService.GetAsync<EquipeDto>(cacheKey);
            if (cached is not null) return cached;

            var equipe = await _equipeRepository.GetEquipePorNomeAsync(nome);

            var dto = _mapper.Map<EquipeDto>(equipe);
            await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        private EquipeData MapearEquipe(EquipeDto dto)
        {
            var equipe = _mapper.Map<EquipeData>(dto);
            return equipe;
        }

        private async Task AtualizarCache(string nome, EquipeData equipeData)
        {
            await RemoverCache(nome);
            _mensageria.PublicarMensagem("atualizar-cache", equipeData);
        }

        private async Task RemoverCache(string nome)
        {
            await _cacheService.RemoveAsync("atletas_todos");
            if (!string.IsNullOrWhiteSpace(nome))
                await _cacheService.RemoveAsync($"atleta_{nome.ToLower()}");
        }

        private async Task<GinasioDto> ObterOuCriarGinasiosAsync(GinasioDto ginasiosDto)
        {
            if (ginasiosDto is null)
                return new GinasioDto();

            var ginasiosExistentes = (await _ginasioService.GetGinasioPorNomeAsync(ginasiosDto.Nome)).FirstOrDefault();

            if (ginasiosExistentes == null)
                await CadastrarGinasioAsync(ginasiosExistentes);

            var ginasiosAtualizados = (await _ginasioService.GetGinasioPorNomeAsync(ginasiosDto.Nome)).FirstOrDefault();

            return ginasiosAtualizados;
        }

        private async Task CadastrarGinasioAsync(GinasioDto dto)
        {
            await _ginasioService.CadastrarGinasioAsync(dto);
            _logger.LogInformation("Ginásio {Nome} cadastrado com sucesso", dto.Nome);
        }
    }
}