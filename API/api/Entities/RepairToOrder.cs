using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities
{
    public class RepairToOrder
    {

        public string detalle { get; set; }
        public string estado { get; set; }
        public string repuesto { get; set; }
        public int Id { get; set; }
        public string usuario_registro { get; set; }
        [ForeignKey("usuario_registro")]
        public User User { get; set; }
        public string tipoDocumento { get; set; }
        public int? codigoCliente { get; set; }
        [ForeignKey("codigoCliente")]
        public Client Client { get; set; }
        public string ncf { get; set; }
        public string referencia { get; set; }
        public decimal descuento { get; set; }
        public string detalleFactura { get; set; }
        public string estadoFactura { get; set; }
        public decimal totaln { get; set; }
        public decimal itbistot { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime fecha { get; set; }
        public string OrderNumber { get; set; }
        public string metPago { get; set; }
        public string tipoFactura { get; set; }
    }
}
