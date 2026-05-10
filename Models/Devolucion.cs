using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Devoluciones")]
    public class Devolucion
    {
        [Key]
        [Column("id_devolucion")]
        public int IdDevolucion { get; set; }

        [Column("id_factura")]
        public int? IdFactura { get; set; }

        [Column("id_producto")]
        public int? IdProducto { get; set; }

        [Column("cantidad")]
        [Required]
        public int Cantidad { get; set; }

        [Column("motivo")]
        [StringLength(255)]
        public string? Motivo { get; set; }

        [Column("fecha_devolucion")]
        public DateTime FechaDevolucion { get; set; } = DateTime.Now;

        [Column("id_usuario_autoriza")]
        public int? IdUsuarioAutoriza { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdFactura")]
        public virtual Factura? Factura { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Producto? Producto { get; set; }

        [ForeignKey("IdUsuarioAutoriza")]
        public virtual Usuario? UsuarioAutoriza { get; set; }
    }
}