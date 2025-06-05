using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("veiculo")]
    public class Veiculo
    {
        [Key]
        public int id_veiculo { get; set; }
        public string apelido { get; set; }
        public double quilometragem { get; set; }
        public string descricao { get; set; }
        public string placa { get; set; }
        public string chassi { get; set; }
        public string cnpj { get; set; }
        public int tipo { get; set; }
        public bool habilitado { get; set; }
        public DateTime data_cadastro { get; set; }
        public int ano { get; set; }


    }
}
