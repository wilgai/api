using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Request
{
    public class UserRequest
    {
        public string id { get; set; }
        [Required]
        public string identificacion { get; set; }

        [Required]
        public string nombre { get; set; }

        [Required]
        public string apellidos { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string direccion { get; set; }

        [Required]
        public string phoneNumber { get; set; }
        [Required]
        public string userName { get; set; }

        [Required]
        public string passwordHash { get; set; }

        public string foto { get; set; }

        [Required]
        public string sexo { get; set; }

        [Required]
        public bool estado { get; set; }
        public string tipo_usuario { get; set; }


    }
}
