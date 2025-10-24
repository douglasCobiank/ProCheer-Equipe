using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Equipe.Infrastructure.Data.Models
{
    public class EquipeData
    {
        [Key]
        public int Id { get; set; }
        public int NivelEquipe { get; set; }
        public bool NonTumbling { get; set; }
        public int Modalidade { get; set; }
        public int TipoEquipe { get; set; }
        public string NomeEquipe { get; set; } = "";
        public List<EquipeAtleta> EquipeAtletas { get; set; } = new();
        public int IdGinasio { get; set; }
        public GinasioData Ginasio { get; set; } = new GinasioData();
    }

    public class EquipeAtleta
    {
        public int IdAtleta { get; set; }
        public int IdEquipe { get; set; }
        public AtletaData Atleta { get; set; }
        public EquipeData Equipe { get; set; }
    }
}