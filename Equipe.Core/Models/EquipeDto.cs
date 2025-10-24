using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Equipe.Core.Models
{
    public class EquipeDto
    {
        public int NivelEquipe { get; set; }
        public bool NonTumbling { get; set; }
        public int Modalidade { get; set; }
        public int TipoEquipe { get; set; }
        public string NomeEquipe { get; set; } = "";
        public List<AtletaDto> Atletas { get; set; } = [];
        public GinasioDto Ginasio { get; set; } = new GinasioDto();
    }
}