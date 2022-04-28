using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Configuration
    {
        public int Id { get; set; }
        public string empresa { get; set; }
        public string web { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string whatsap { get; set; }
        public string facebook { get; set; }
        public string instagram { get; set; }
        public string tiktok { get; set; }
        public string logo { get; set; }
        public string notaFactura { get; set; }
        public string notaOrdenDeServicio { get; set; }
        public string notaServicio { get; set; }
        public string instrucionOdenDeServicio { get; set; }
        public string moneda { get; set; }
        public string direccion { get; set; }
        public string notaCotizacionFactura { get; set; }
        public string notaCotizacionReparacion { get; set; }
        public string tipoFactura { get; set; }
    }
}
