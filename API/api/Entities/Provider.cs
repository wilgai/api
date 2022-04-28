using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Provider
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string nombre { get; set; }
        public string rnc { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string web { get; set; }
        public string tipo { get; set; }
        public string logo { get; set; }
    }
}
