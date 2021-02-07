using api.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(20)]
        [Required]
        public string identificacion { get; set; }   

        
        [MaxLength(50)]
        [Required]
        public string nombre { get; set; }  

       
        [MaxLength(50)]
        [Required]
        public string apellidos { get; set; }

        [MaxLength(100)]
        public string direccion { get; set; }

      
        public string foto { get; set; }

        public string sexo { get; set; }

        public bool estado { get; set; }
        

        public string tipo_usuario { get; set; }

      
        public string FullName => $"{nombre} {apellidos}";

        








    }
}

