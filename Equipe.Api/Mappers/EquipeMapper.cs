using AutoMapper;
using Equipe.Api.Models;
using Equipe.Core.Models;

namespace Equipe.Api.Mappers
{
    public class EquipeMapper: Profile
    {
        public EquipeMapper()
        {
            CreateMap<EquipeDto, Equipes>().ReverseMap();
            CreateMap<AtletaDto, Atleta>().ReverseMap();
            CreateMap<DocumentoDto, Documento>().ReverseMap();
            CreateMap<EnderecoDto, Endereco>().ReverseMap();
            CreateMap<GinasioDto, Ginasio>().ReverseMap();
            CreateMap<UsuarioDto, Usuario>().ReverseMap();
        }
    }
}