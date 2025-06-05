using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("tipo_caminhao")]
    public class TipoCaminhao
    {
        [Key]

        public int id { get; set; }

        public string nome { get; set; }
        public bool habilitado { get; set; }
        public DateTime data_cadastro { get; set; }


    }
}
