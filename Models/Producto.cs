using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Productos")]
    public class Producto
    {
        private string _codigo_barras = string.Empty;

        [Key]
        [Column("id_producto")]
        public int id_producto { get; set; }

        [Required]
        [StringLength(50)]
        [Column("codigo_barras")]
        public string codigo_barras
        {
            get => _codigo_barras?.Replace("\n", "").Replace("\r", "").Trim() ?? string.Empty;
            set => _codigo_barras = value?.Replace("\n", "").Replace("\r", "").Trim() ?? string.Empty;
        }

        [Required]
        [StringLength(100)]
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;

        [StringLength(255)]
        [Column("descripcion")]
        public string? descripcion { get; set; }

        [Column("id_categoria")]
        public int? id_categoria { get; set; }

        [Column("id_proveedor_preferente")]
        public int? id_proveedor_preferente { get; set; }

        [Column("iva_porcentaje")]
        public decimal iva_porcentaje { get; set; } = 15.00m;

        [Column("stock_actual")]
        public int stock_actual { get; set; } = 0;

        [Column("stock_minimo")]
        public int stock_minimo { get; set; } = 5;

        [Column("ultimo_costo_compra")]
        public decimal? ultimo_costo_compra { get; set; }

        [Column("costo_promedio")]
        public decimal? costo_promedio { get; set; }

        [Column("activo")]
        public bool activo { get; set; } = true;

        [Column("fecha_registro")]
        public DateTime fecha_registro { get; set; } = DateTime.Now;

        [ForeignKey("id_categoria")]
        public virtual Categoria? Categoria { get; set; }

        [ForeignKey("id_proveedor_preferente")]
        public virtual Proveedor? ProveedorPreferente { get; set; }

        public virtual ICollection<PrecioPromocion> PreciosPromociones { get; set; } = new List<PrecioPromocion>();

        public virtual ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();

        [NotMapped]
        public decimal PrecioActual
        {
            get
            {
                var promoActiva = PreciosPromociones?.FirstOrDefault(p =>
                    p.es_promocional &&
                    (p.fecha_fin_promo == null || p.fecha_fin_promo >= DateOnly.FromDateTime(DateTime.Today)));

                if (promoActiva != null)
                    return promoActiva.precio_final;

                return PreciosPromociones?.FirstOrDefault()?.precio_final ?? 0;
            }
        }

        [NotMapped]
        public decimal CostoActual => PreciosPromociones?.FirstOrDefault()?.costo_compra ?? 0;

        [NotMapped]
        public decimal PrecioConIva => PrecioActual * (1 + iva_porcentaje / 100);

        [NotMapped]
        public bool TieneStockBajo => stock_actual <= stock_minimo;

        [NotMapped]
        public bool TienePromocionActiva => PreciosPromociones?.Any(p =>
            p.es_promocional &&
            (p.fecha_fin_promo == null || p.fecha_fin_promo >= DateOnly.FromDateTime(DateTime.Today))) ?? false;
    }
}