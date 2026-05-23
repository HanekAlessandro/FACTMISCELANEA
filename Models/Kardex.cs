using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Kardex")]
    public class Kardex
    {
        [Key]
        [Column("id_movimiento")]
        public int id_movimiento { get; set; }

        [Column("id_producto")]
        public int id_producto { get; set; }

        [Column("tipo_movimiento")]
        public string tipo_movimiento { get; set; } = string.Empty;

        [Column("cantidad")]
        public int cantidad { get; set; }

        [Column("fecha")]
        public DateTime fecha { get; set; }

        [Column("referencia_id")]
        public int? referencia_id { get; set; }

        [Column("observaciones")]
        [StringLength(255)]
        public string? observaciones { get; set; }

        [ForeignKey("id_producto")]
        public virtual Producto? Producto { get; set; }
    }
}