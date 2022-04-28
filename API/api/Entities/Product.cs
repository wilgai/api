using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string nombre { get; set; }
        public int ProviderId { get; set; }
        [ForeignKey("ProviderId")]
        public Provider Provider
        { get; set; }
        public string usuario_registro { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime fecha_registro { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime fecha_actualizacion { get; set; }
        public string tipo_impuesto { get; set; }
        public bool Activo { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public string referencia_interna { get; set; }
        public string referencia_suplidor { get; set; }
        public string foto { get; set; }
        public bool oferta { get; set; }
        public bool modificar_precio { get; set; }
        public bool acepta_descuento { get; set; }
        public string detalle { get; set; }
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
        public int porciento_beneficio { get; set; }
        public int porciento_minimo { get; set; }
        public int ModelId { get; set; }
        [ForeignKey("ModelId")]
        public Model Model { get; set; }
        public string codigo { get; set; }
        public int garantia { get; set; }
    }
}
