using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string nombre { get; set; }
    }
}
