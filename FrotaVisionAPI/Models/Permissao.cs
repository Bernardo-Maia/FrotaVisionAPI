using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("permissoes_usuario")]
    public class Permissao
    {
        [Key]
        public int id_permissao { get; set; }
        public string? nome { get; set; }
        public string? descricao { get; set; }
        public bool habilitado { get; set; }
        public DateTime data_cadastro { get; set; }
        
    }
}
