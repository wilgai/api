using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string nombre { get; set; }
    }
}
