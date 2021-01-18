using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Order_Detail
    {
        public int Id { get; set; }
        public Product codigo_articulo { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }

        public decimal itbis { get; set; }
        public int IdInventario { get; set; }
        public string referencia { get; set; }
        public decimal valor => (decimal)cantidad * precio;
    }
}
