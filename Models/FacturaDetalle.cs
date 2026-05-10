using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    public class FacturaDetalle
    {
        [Key]
        public int id_detalle { get; set; }

        public int id_factura { get; set; }

        public int id_producto { get; set; }

        [Required]
        public int cantidad { get; set; }

        public decimal? precio_unitario_aplicado { get; set; }

        public decimal? iva_aplicado { get; set; }

        public decimal? subtotal_linea { get; set; }

        [ForeignKey("id_factura")]
        // CORREGIDO: Hacer nullable
        public Factura? Factura { get; set; }

        [ForeignKey("id_producto")]
        // CORREGIDO: Hacer nullable
        public Producto? Producto { get; set; }
    }
}