using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Model
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string nombre { get; set; }
        public int marca { get; set; }
    }
}
