using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public Product IdProducto { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal Ganancia { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal Descuento { get; set; }
        public decimal PorcientoDescuento { get; set; }
        public int Cantidad { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime Fecha { get; set; }
    }
}
