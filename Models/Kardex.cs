using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    public class Kardex
    {
        [Key]
        public int id_movimiento { get; set; }

        public int id_producto { get; set; }

        [StringLength(20)]
        public string? tipo_movimiento { get; set; }

        public int cantidad { get; set; }

        public DateTime fecha { get; set; } = DateTime.Now;

        public int? referencia_id { get; set; }

        [StringLength(255)]
        public string? observaciones { get; set; }

        [ForeignKey("id_producto")]
        public Producto Producto { get; set; } = null!;
    }
}