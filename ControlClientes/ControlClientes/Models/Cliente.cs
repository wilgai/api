using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ControlClientes.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Nombre { get; set; }
        public string Identificacion { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public string Telefono { get; set; }
        public string Sexo { get; set; }
        public string Imagen { get; set; }
        public string Estado { get; set; }
      
        public ICollection<Direccion> Direcciones { get; set; }
        public ICollection<ConocimientoDedesarrollo> Conocimientos { get; set; }

    }
}
