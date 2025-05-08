using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("viagem")]
    public class Viagem
    {
        [Key]
        public int id_viagem { get; set; }

        public int id_veiculo { get; set; }
        public DateTime data_viagem { get; set; }
        public double quilometragem_viagem { get; set; }
        public double horasMotor { get; set; }
        public int id_motorista { get; set; }

        public bool habilitado { get; set; }
        public string? descricao { get; set; }

        public string origem { get; set; }

        public string destino { get; set; }

        public DateTime data_inicio { get; set; }

        public DateTime data_fim { get; set; }



    }
}
