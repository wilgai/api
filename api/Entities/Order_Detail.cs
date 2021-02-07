using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Order_Detail
    {
        public int Id { get; set; }
        public int codigo_articulo { get; set; }
        [ForeignKey("codigo_articulo")]
        public Product Product { get; set; }
        public int cantidad { get; set; }
        public Nullable<int> OrderID { get; set; }
        public virtual Order Order { get; set; }
        public decimal PrecioVenta { get; set; }
        public string   idInventario { get; set; }
        [ForeignKey("idInventario")]
        public Inventory Inventory { get; set; }
        public decimal itbis { get; set; }
        public string referencia { get; set; }
        [NotMapped]
        public decimal PrecioCompra { get; set; }
        public string idFactura { get; set; }
        public decimal totalVenta => cantidad * PrecioVenta;
        public decimal totalCompra => cantidad * PrecioVenta;
        public decimal Total => cantidad * PrecioVenta;
        [NotMapped]
        public decimal Ganancia { get; set; }
        [NotMapped]
        public decimal PorcientoDescuento { get; set; }


        


   
  
   
   
    
    
  
    
    

   
    }
}
