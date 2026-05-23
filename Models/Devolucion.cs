using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Devoluciones")]
    public class Devolucion
    {
        [Key]
        [Column("id_devolucion")]
        public int id_devolucion { get; set; }

        [Column("id_factura")]
        public int? id_factura { get; set; }

        [Column("id_producto")]
        public int? id_producto { get; set; }

        [Column("cantidad")]
        [Required]
        public int cantidad { get; set; }

        [Column("motivo")]
        [StringLength(255)]
        public string? motivo { get; set; }

        [Column("fecha_devolucion")]
        public DateTime fecha_devolucion { get; set; } = DateTime.Now;

        [Column("id_usuario_autoriza")]
        public int? id_usuario_autoriza { get; set; }

        [ForeignKey("id_factura")]
        public virtual Factura? Factura { get; set; }

        [ForeignKey("id_producto")]
        public virtual Producto? Producto { get; set; }

        [ForeignKey("id_usuario_autoriza")]
        public virtual Usuario? UsuarioAutoriza { get; set; }
    }
}