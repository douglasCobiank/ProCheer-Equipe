namespace Equipe.Api.Models
{
    public class Equipes
    {
        public int NivelEquipe { get; set; }
        public bool NonTumbling { get; set; }
        public int Modalidade { get; set; }
        public int TipoEquipe { get; set; }
        public string NomeEquipe { get; set; } = "";
        public List<Atleta> Atletas { get; set; } = [];
        public Ginasio Ginasio { get; set; } = new Ginasio();

    }
}