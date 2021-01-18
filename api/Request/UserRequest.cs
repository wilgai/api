using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Request
{
    public class UserRequest
    {
        
        [Required]
        public string identificacion { get; set; }

        [Required]
        public string nombre { get; set; }

        [Required]
        public string apellidos { get; set; }

        [Required]
        public string correo { get; set; }

        [Required]
        public string direccion { get; set; }

        [Required]
        public string telefono { get; set; }
        [Required]
        public string usuario { get; set; }

        [Required]
        public string contresana { get; set; }

        public string foto { get; set; }

        [Required]
        public string sexo { get; set; }

        [Required]
        public string estado { get; set; }


    }
}
