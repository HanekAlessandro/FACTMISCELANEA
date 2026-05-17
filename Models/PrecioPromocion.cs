using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Precios_Promociones")]
    public class PrecioPromocion
    {
        [Key]
        [Column("id_precio")]
        public int id_precio { get; set; }

        [Column("id_producto")]
        public int id_producto { get; set; }

        [Column("costo_compra")]
        public decimal costo_compra { get; set; }

        [Column("precio_sugerido")]
        public decimal? precio_sugerido { get; set; }

        [Column("precio_final")]
        public decimal precio_final { get; set; }

        [Column("es_promocional")]
        public bool es_promocional { get; set; }

        [Column("fecha_inicio_promo")]
        public DateOnly? fecha_inicio_promo { get; set; }

        [Column("fecha_fin_promo")]
        public DateOnly? fecha_fin_promo { get; set; }

        [Column("fecha_actualizacion")]
        public DateTime fecha_actualizacion { get; set; } = DateTime.Now;

        [ForeignKey("id_producto")]
        public virtual Producto? Producto { get; set; }
    }
}