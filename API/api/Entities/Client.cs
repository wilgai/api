using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Client
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string identificacion { get; set; }
        public string correo { get; set; }
        public string celular { get; set; }
        public string telefono { get; set; }
        public string sexo { get; set; }
    }
}
