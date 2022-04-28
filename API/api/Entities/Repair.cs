using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Repair
    {
        public int Id { get; set; }
        [Required]
        public string numero { get; set; }
        public int codigoCliente { get; set; }
        [ForeignKey("codigoCliente")]
        public Client Cliente { get; set; }
        public string resgistradoPor { get; set; }
        [ForeignKey("resgistradoPor")]
        public User User { get; set; }
        public string detalle { get; set; }
        public decimal total { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime fecha { get; set; }
        public string FechaLocal => fecha.ToLocalTime().ToString("dd-MM-yyy HH:mm tt");
        public string estado { get; set; }
        public string categoria { get; set; }
        [Required]
        public string equipo { get; set; }
        public string serie { get; set; }
        public string color { get; set; }
        public bool trajoAccesorio { get; set; }
        public string accesorios { get; set; }
        [Required]
        public string averia { get; set; }
        public string fallaTecnica { get; set; }
        public decimal costoPieza { get; set; }
        public string repuesto { get; set; }

        public decimal TotalReparacion => total + costoPieza;
        [NotMapped]
        public List<Order_Detail> DeletedOrderItemIDs { get; set; }
        public List<Order_Detail> Order_Details { get; set; }
    }
}
