using System.ComponentModel.DataAnnotations;

namespace FactMiscelanea.Models
{
    public class Categoria
    {
        [Key]
        public int id_categoria { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre_categoria { get; set; } = string.Empty;

        public ICollection<Producto>? Productos { get; set; }
    }
}