using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("plano")]
    public class Plano
    {
        [Key]
        public int id_plano { get; set; }
        public string? nome { get; set; }
        public string? descricao { get; set; }
        public double valor { get; set; }
        public bool habilitado { get; set; }
        public DateTime data_cadastro { get; set; }
        public int qntd_usuarios { get; set; }
        public int qntd_veiculos { get; set; }
        public int qntd_simultaneos { get; set; }

    }
}
