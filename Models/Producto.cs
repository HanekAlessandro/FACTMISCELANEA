using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;

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

        // Propiedades de navegación
        [ForeignKey("id_categoria")]
        public virtual Categoria? Categoria { get; set; }

        [ForeignKey("id_proveedor_preferente")]
        public virtual Proveedor? ProveedorPreferente { get; set; }

        // Relación con precios y promociones
        public virtual ICollection<PrecioPromocion> PreciosPromociones { get; set; } = new List<PrecioPromocion>();

        // Relación con detalles de factura
        public virtual ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();

        /// <summary>
        /// Obtiene el precio final actual del producto (considerando promociones activas)
        /// </summary>
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

        /// <summary>
        /// Obtiene el costo de compra actual
        /// </summary>
        [NotMapped]
        public decimal CostoActual => PreciosPromociones?.FirstOrDefault()?.costo_compra ?? 0;

        /// <summary>
        /// Calcula el precio con IVA incluido
        /// </summary>
        [NotMapped]
        public decimal PrecioConIva => PrecioActual * (1 + iva_porcentaje / 100);

        /// <summary>
        /// Indica si el producto tiene stock bajo
        /// </summary>
        [NotMapped]
        public bool TieneStockBajo => stock_actual <= stock_minimo;

        /// <summary>
        /// Indica si el producto tiene promoción activa
        /// </summary>
        [NotMapped]
        public bool TienePromocionActiva => PreciosPromociones?.Any(p => 
            p.es_promocional && 
            (p.fecha_fin_promo == null || p.fecha_fin_promo >= DateOnly.FromDateTime(DateTime.Today))) ?? false;
    }
}