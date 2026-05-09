using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    public class PrecioPromocion
    {
        [Key]
        public int id_precio { get; set; }

        public int id_producto { get; set; }

        public decimal costo_compra { get; set; }

        public decimal? precio_sugerido { get; set; }

        public decimal precio_final { get; set; }

        public bool es_promocional { get; set; }

        public DateTime? fecha_inicio_promo { get; set; }

        public DateTime? fecha_fin_promo { get; set; }

        [ForeignKey("id_producto")]
        public Producto Producto { get; set; } = null!;
    }
}