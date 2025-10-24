namespace Equipe.Api.Models
{
    public class Endereco
    {
        public string CEP { get; set; } = "";
        public string Rua { get; set; } = "";
        public int Numero { get; set; }
        public string Bairro { get; set; } = "";
        public string Cidade { get; set; } = "";
        public string Estado { get; set; } = "";
        public string Pais { get; set; } = "";
    }
}