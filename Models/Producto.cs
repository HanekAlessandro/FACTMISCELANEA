using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Productos")]
    public class Producto
    {
        private string _codigo_barras = string.Empty;

        [Key]
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
        public string nombre { get; set; } = string.Empty;

        [StringLength(255)]
        public string? descripcion { get; set; }

        public int? id_categoria { get; set; }

        public decimal iva_porcentaje { get; set; } = 15.00m;

        public int stock_actual { get; set; } = 0;

        public int stock_minimo { get; set; } = 5;

        [ForeignKey("id_categoria")]
        public virtual Categoria? Categoria { get; set; }
    }
}