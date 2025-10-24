using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Equipe.Infrastructure.Data.Models
{
    public class AtletaData
    {
        [Key]
        public int Id { get; set; }
        public int IdAtleta { get; set; }
        public UsuarioData Usuario { get; set; } = new UsuarioData();
        public DocumentoData Documento { get; set; } = new DocumentoData();
        public string ComprovanteMatricula { get; set; } = "";
        public int Genero { get; set; }
        public int Idade { get; set; }

        [JsonIgnore]
        public List<EquipeAtleta> EquipeAtletas { get; set; } = new();
    }
}