using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    public class Devolucion
    {
        [Key]
        public int id_devolucion { get; set; }

        public int? id_factura { get; set; }

        public int? id_producto { get; set; }

        [Required]
        public int cantidad { get; set; }

        [StringLength(255)]
        public string? motivo { get; set; }

        public DateTime fecha_devolucion { get; set; } = DateTime.Now;

        public int? id_usuario_autoriza { get; set; }

        [ForeignKey("id_factura")]
        public Factura? Factura { get; set; }

        [ForeignKey("id_producto")]
        public Producto? Producto { get; set; }

        [ForeignKey("id_usuario_autoriza")]
        public Usuario? UsuarioAutoriza { get; set; }
    }
}