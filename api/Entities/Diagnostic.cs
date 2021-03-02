using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Diagnostic
    {
        [Required]
        public string estado { get; set; }
        [Required]
        public string fallaTecnica { get; set; }
        [Required]
        public decimal total { get; set; }
        [Required]
        public int Id { get; set; }
    }
}
