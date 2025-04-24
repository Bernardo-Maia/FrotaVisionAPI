using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("manutencao")]
    public class Manutencao
    {
        [Key]
        public int id_manutencao { get;  set; }
        public string nome {  get; set; }
        public string? descricao { get; set; }
        public double quilometragem_preventiva { get; set; }

        public bool habilitado { get; set; }
    }
}
