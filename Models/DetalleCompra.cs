using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Detalle_Compras")]
    public class DetalleCompra
    {
        [Key]
        [Column("id_detalle_compra")]
        public int id_detalle_compra { get; set; }

        [Column("id_compra")]
        public int id_compra { get; set; }

        [Column("id_producto")]
        public int id_producto { get; set; }

        [Column("cantidad")]
        public int cantidad { get; set; }

        [Column("costo_unitario")]
        public decimal costo_unitario { get; set; }

        [Column("iva_porcentaje")]
        public decimal iva_porcentaje { get; set; } = 15;

        [Column("descuento_unitario")]
        public decimal descuento_unitario { get; set; }

        [Column("subtotal_linea")]
        public decimal subtotal_linea { get; set; }

        // Propiedades de navegación
        [ForeignKey("id_compra")]
        public virtual Compra? Compra { get; set; }

        [ForeignKey("id_producto")]
        public virtual Producto? Producto { get; set; }
    }
}