using api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace api.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public User usuario_registro { get; set; }
        public string tipoDocumento { get; set; }
        public Client codigoCliente { get; set; }
        public string ncf { get; set; }
        public string referencia { get; set; }
        public decimal descuento { get; set; }
        public string detalle { get; set; }
        public OrderStatus estado { get; set; }
        public decimal totaln { get; set; }
        public decimal itbistot { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime fecha { get; set; }
        public string OrderNumber { get; set; }
        public string metPago { get; set; }
        public string tipoFactura { get; set; }
        public int suplidor { get; set; }
        public ICollection<Order_Detail> Order_Details { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Lines => Order_Details == null ? 0 : Order_Details.Count;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public float Quantity => Order_Details == null ? 0 : Order_Details.Sum(od => od.cantidad);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Valor => Order_Details == null ? 0 : Order_Details.Sum(od => od.valor);
    }
}

