using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        public int id_usuario { get; set; }

        [StringLength(255)]
        public string email { get; set; }

        public string senha { get; set; }

        
        [StringLength(255)]
        public string nome_usuario { get; set; }

        public DateTime data_cadastro { get; set; }

        public string cnpj { get; set; }

        public int permissoes_usuario { get; set; }

        public bool habilitado { get; set; }

    }
}
