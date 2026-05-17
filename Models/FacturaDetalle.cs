using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Factura_Detalle")]
    public class FacturaDetalle
    {
        [Key]
        [Column("id_detalle")]
        public int id_detalle { get; set; }

        [Column("id_factura")]
        public int id_factura { get; set; }

        [Column("id_producto")]
        public int id_producto { get; set; }

        [Column("cantidad")]
        public int cantidad { get; set; }

        [Column("precio_unitario_aplicado")]
        public decimal precio_unitario_aplicado { get; set; }

        [Column("iva_aplicado")]
        public decimal iva_aplicado { get; set; }

        [Column("subtotal_linea")]
        public decimal subtotal_linea { get; set; }

        [ForeignKey("id_factura")]
        public virtual Factura? Factura { get; set; }

        [ForeignKey("id_producto")]
        public virtual Producto? Producto { get; set; }
    }
}