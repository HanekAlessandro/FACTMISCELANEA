using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        public int id_categoria { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre_categoria { get; set; } = string.Empty;

        public virtual ICollection<Producto>? Productos { get; set; }
    }
}