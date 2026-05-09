using System.ComponentModel.DataAnnotations;

namespace FactMiscelanea.Models
{
    public class Usuario
    {
        [Key]
        public int id_usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre_completo { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string nombre_usuario { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string password_hash { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string rol { get; set; } = string.Empty;

        public ICollection<Factura>? Facturas { get; set; }

        public ICollection<Devolucion>? Devoluciones { get; set; }
    }
}