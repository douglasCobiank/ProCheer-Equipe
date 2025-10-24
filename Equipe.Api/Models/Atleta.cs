namespace Equipe.Api.Models
{
    public class Atleta
    {
        public Usuario Usuario { get; set; } = new Usuario();
        public Documento Documento { get; set; } = new Documento();
        public string ComprovanteMatricula { get; set; } = "";
        public int Genero { get; set; }
        public int Idade { get; set; }
    }
}