using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("manutencao_realizada")]
    public class ManutencaoRealizada
    {
        [Key]
        public int id_manutencao_realizada { get; set; }
        public int id_veiculo { get; set; }
        public double quilometragem_ultima_manutencao { get; set; }
        public DateTime data_manutencao { get; set; }
        public string? descricao { get; set; }
        public int id_manutencao { get; set; }
        public DateTime data_cadastro { get; set; }
        public double valor_manutencao { get; set; }
        public double horasMotorManutencao { get; set; }
        public bool eManuntencaoPreventiva { get; set; }
        public bool habilitado { get; set; }
    }
}
