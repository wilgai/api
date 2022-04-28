using api.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class AddModelviewModel
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string nombre { get; set; }
        [Required]
        public int marca { get; set; }
    }
}
