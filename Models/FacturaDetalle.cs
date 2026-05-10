using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Factura_Detalle")]
    public class FacturaDetalle
    {
        [Key]
        public int id_detalle { get; set; }

        [Required]
        public int id_factura { get; set; }

        [Required]
        public int id_producto { get; set; }

        [Required]
        public int cantidad { get; set; }

        public decimal precio_unitario_aplicado { get; set; } = 0;

        public decimal iva_aplicado { get; set; } = 0;

        public decimal subtotal_linea { get; set; } = 0;

        [ForeignKey("id_factura")]
        public virtual Factura? Factura { get; set; }

        [ForeignKey("id_producto")]
        public virtual Producto? Producto { get; set; }
    }
}