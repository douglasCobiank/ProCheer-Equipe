namespace Equipe.Infrastructure.Data.Models
{
    public class GinasioData
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public EnderecoData Endereco { get; set; } = new EnderecoData();
        public string Responsavel { get; set; } = "";
        public string Telefone { get; set; } = "";
        public string Email { get; set; } = "";
        public int Tipo { get; set; }
    }
}