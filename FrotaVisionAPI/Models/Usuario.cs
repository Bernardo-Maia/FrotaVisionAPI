using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        public int idUsuario { get; set; }

        [Required]
        [StringLength(255)]
        public string email { get; set; }

        [Required]
        public string senha { get; set; }

        [Required]
        [StringLength(255)]
        public string nomeUsuario { get; set; }

        public DateTime dataCadastro { get; set; }

        public string? cnpj { get; set; }

        public int? permissoesUsuario { get; set; }
    }
}
