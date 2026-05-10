using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Precios_Promociones")]
    public class PrecioPromocion
    {
        [Key]
        [Column("id_precio")]
        public int IdPrecio { get; set; }

        [Column("id_producto")]
        [Required]
        public int IdProducto { get; set; }

        [Column("costo_compra")]
        [Required]
        public decimal CostoCompra { get; set; }

        [Column("precio_sugerido")]
        public decimal PrecioSugerido { get; set; } = 0;

        [Column("precio_final")]
        [Required]
        public decimal PrecioFinal { get; set; }

        [Column("es_promocional")]
        public bool EsPromocional { get; set; } = false;

        [Column("fecha_inicio_promo")]
        public DateTime? FechaInicioPromo { get; set; }

        [Column("fecha_fin_promo")]
        public DateTime? FechaFinPromo { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdProducto")]
        public virtual Producto? Producto { get; set; }
    }
}