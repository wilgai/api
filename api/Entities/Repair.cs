using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Repair
    {
        public int Id { get; set; }
        [Required]
        public string numero { get; set; }
        public Client codigoCliente { get; set; }
        public User resgistradoPor { get; set; }
        public string detalle { get; set; }
        public decimal total { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime fecha { get; set; }
        public string estado { get; set; }
        public string categoria { get; set; }
        [Required]
        public string equipo { get; set; }
        public string serie { get; set; }
        public string color { get; set; }
        public bool trajoAccesorio { get; set; }
        public string accesorios { get; set; }
        public string averia { get; set; }
        [Required]
        public string fallaTecnica { get; set; }
        public decimal costoPieza { get; set; }
        public string repuesto { get; set; }
    }
}
