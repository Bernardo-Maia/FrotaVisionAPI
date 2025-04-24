using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("motorista")]
    public class Motorista
    {
        [Key]
        public int id_motorista { get; set; }
        public string nome { get; set; }
        public DateTime data_cadastro { get; set; }
        public string cnpj { get; set; }
        public bool habilitado { get; set; }
    }
}
