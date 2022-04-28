using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControlClientes.Models
{
    public class ConocimientoDedesarrollo
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Conocimiento { get; set; }
       
        public virtual Cliente Cliente { get; set; }
        [NotMapped]
        public int codigoCliente { get; set; }
    }
}
