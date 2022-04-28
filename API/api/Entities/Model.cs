using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Entities
{
    public class Model
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string nombre { get; set; }
        public int brandId { get; set; }
        [ForeignKey("brandId")]
        public Brand Brand { get; set; }
    }
}
