using AutoMapper;
using Equipe.Api.Models;
using Equipe.Core.Interface;
using Equipe.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Equipe.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipeController(IEquipeHandler equipeHandler, IMapper mapper) : ControllerBase
    {
        private readonly IEquipeHandler _equipeHandler = equipeHandler;
        private readonly IMapper _mapper = mapper;

        [HttpPost("criar-equipe")]
        public async Task<IActionResult> CriarAtletaAsync([FromBody] Equipes equipes)
        {
            if (equipes is null)
                return BadRequest("Dados do equipe inválidos.");

            var equipeDto = _mapper.Map<EquipeDto>(equipes);
            await _equipeHandler.CadastrarEquipeHandler(equipeDto);

            return Created(string.Empty, new { Mensagem = "Equipe criada com sucesso." });
        }

        [HttpPut("editar-equipe/{id:int}")]
        public async Task<IActionResult> EditarEquipeAsync(int id, [FromBody] Equipes equipes)
        {
            if (equipes is null)
                return BadRequest("Dados do equipe inválidos.");

            var equipeDto = _mapper.Map<EquipeDto>(equipes);
            await _equipeHandler.EditarEquipeAsync(equipeDto, id);

            return Ok(new { Mensagem = "Equipe atualizado com sucesso." });
        }

        [HttpDelete("deletar-equipe/{id:int}")]
        public async Task<IActionResult> DeletarAtletaAsync(int id)
        {
            await _equipeHandler.DeletaEquipeAsync(id);
            return Ok(new { Mensagem = "Equipe deletado com sucesso." });
        }

        [HttpGet("buscar-equipe")]
        public async Task<IActionResult> BuscarAtletasAsync()
        {
            var equipes = await _equipeHandler.BuscaEquipeAsync();
            if (equipes == null || equipes.Count == 0)
                return NotFound("Nenhuma Equipe encontrado.");

            return Ok(equipes);
        }

        [HttpGet("buscar-equipe-por-nome/{nome}")]
        public async Task<IActionResult> BuscarEquipePorNomeAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("Nome inválido.");

            var equipes = await _equipeHandler.BuscaEquipePorNomeAsync(nome);
            if (equipes == null)
                return NotFound("Atleta não encontrado.");

            return Ok(equipes);
        }
    }
}