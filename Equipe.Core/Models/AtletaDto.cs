namespace Equipe.Core.Models
{
    public class AtletaDto
    {
        public int IdAtleta { get; set; }
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public DocumentoDto Documento { get; set; } = new DocumentoDto();
        public string ComprovanteMatricula { get; set; } = "";
        public int Genero { get; set; }
        public int Idade { get; set; }
    }
}