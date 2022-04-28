using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Inventory
    {
        public string Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Producto { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal Ganancia { get; set; }
        public decimal Itbis { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal PorcientoDescuento { get; set; }
        public int Cantidad { get; set; }
        public string OrderNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime Fecha { get; set; }
    }
}
