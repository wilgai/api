using System;
using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string nombre { get; set; }
        public Provider codigo_suplidor { get; set; }
        public int usuario_registro { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime fecha_registro { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime fecha_actualizacion { get; set; }
        public string tipo_impuesto { get; set; }
        public bool estado { get; set; }
        public Category codigo_categoria { get; set; }
        public string referencia_interna { get; set; }
        public string referencia_suplidor { get; set; }
        public string foto { get; set; }
        public bool oferta { get; set; }
        public bool modificar_precio { get; set; }
        public bool acepta_descuento { get; set; }
        public string detalle { get; set; }
        public Brand codigo_marca { get; set; }
        public int porciento_beneficio { get; set; }
        public int porciento_minimo { get; set; }
        public Model modelo { get; set; }
        public string codigo { get; set; }
        public int garantia { get; set; }
    }
}
