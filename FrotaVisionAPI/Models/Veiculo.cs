﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotaVisionAPI.Models
{
    [Table("veiculo")]
    public class Veiculo
    {
        [Key]
        public int id_veiculo { get; set; }
        public string apelido { get; set; }
        public float quilometragem { get; set; }
        public float horas_motor { get; set; }
        public string descricao { get; set; }
        public string placa { get; set; }
        public string chassi { get; set; }
        public string? cnpj { get; set; }



    }
}
