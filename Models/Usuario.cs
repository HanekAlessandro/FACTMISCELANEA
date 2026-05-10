using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("nombre_completo")]
        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Column("nombre_usuario")]
        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Column("password_hash")]
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("rol")]
        [Required]
        [StringLength(20)]
        public string Rol { get; set; } = string.Empty;

        // Propiedades de navegación
        public virtual ICollection<Factura>? Facturas { get; set; }
        public virtual ICollection<Devolucion>? Devoluciones { get; set; }
    }
}