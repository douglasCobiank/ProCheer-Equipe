using AutoMapper;
using Equipe.Core.Models;
using Equipe.Infrastructure.Data.Models;

namespace Equipe.Core.Mappers
{
    public class EquipeMapper: Profile
    {
        public EquipeMapper()
        {
            CreateMap<EquipeData, EquipeDto>().ReverseMap();
            CreateMap<AtletaData, AtletaDto>().ReverseMap();
            CreateMap<DocumentoData, DocumentoDto>().ReverseMap();
            CreateMap<EnderecoData, EnderecoDto>().ReverseMap();
            CreateMap<GinasioData, GinasioDto>().ReverseMap();
            CreateMap<UsuarioData, UsuarioDto>().ReverseMap();
        }
    }
}