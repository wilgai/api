using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlClientes.Models
{
    public class Direccion
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string LineaDeDireccion1 { get; set; }
        public string LineaDeDireccion2 { get; set; }
        public string Pais { get; set; }
        public string Provicia { get; set; }
       
        public string Municipio { get; set; }
        public string CodigoPostal { get; set; }
        public virtual Cliente Cliente { get; set; }

        [NotMapped]
        public int codigoCliente { get; set; }


    }
}
