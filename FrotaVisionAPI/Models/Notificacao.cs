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
        public bool urgencia { get; set; }
        public string tipo_caminhao { get; set; }
        public int idManutencaoRealizada { get; set; }
        public DateTime data_Manutencao { get; set; }
        public string descricao_manutencao { get; set; }

    }
}
