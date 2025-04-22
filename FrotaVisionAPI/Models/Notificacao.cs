namespace FrotaVisionAPI.Models
{
    public class Notificacao
    {
        public int id_veiculo { get; set; }
        public string nomeVeiculo { get; set; }
        public int id_manutencao { get; set; }
        public string nomeManutencao { get; set; }
        public double quilometragemAtual { get; set; }
        public double quilometragemManutencao { get; set; }

    }
}
