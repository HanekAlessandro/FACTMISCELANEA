using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    public class Producto
    {
        [Key]
        public int id_producto { get; set; }

        [Required]
        [StringLength(50)]
        public string codigo_barras { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string nombre { get; set; } = string.Empty;

        [StringLength(255)]
        public string? descripcion { get; set; }

        public int? id_categoria { get; set; }

        public decimal iva_porcentaje { get; set; } = 15;

        public int stock_actual { get; set; } = 0;

        public int stock_minimo { get; set; } = 5;

        [ForeignKey("id_categoria")]
        public Categoria? Categoria { get; set; }

        // CORREGIDO: Cambiar de PreciosPromociones (plural) a PrecioPromocion (singular - uno a uno)
        public PrecioPromocion? PrecioPromocion { get; set; }

        public ICollection<FacturaDetalle>? FacturaDetalles { get; set; }

        public ICollection<Devolucion>? Devoluciones { get; set; }

        // CORREGIDO: Cambiar de KardexMovimientos a Kardex
        public ICollection<Kardex>? Kardex { get; set; }
    }
}