using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("empresa")]
    public class Empresa
    {
        [Key]
        public string cnpj { get; set; }
        public string nome_social { get; set; }
        public DateTime data_cadastro { get; set; }
        public int? id_plano { get; set; }
    }
}
