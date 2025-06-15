using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("newsletter")]
    public class Newsletter
    {
        [Key]
        public int id { get; set; }
        public string email { get; set; }
        public DateTime data_cadastro { get; set; }

    }
}
