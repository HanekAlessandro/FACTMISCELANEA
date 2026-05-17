using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        [Column("id_cliente")]
        public int id_cliente { get; set; }

        [Column("identificacion")]
        public string identificacion { get; set; } = string.Empty;

        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;

        [Column("direccion")]
        public string direccion { get; set; } = string.Empty;

        [Column("telefono")]
        public string telefono { get; set; } = string.Empty;

        // Agregar esta propiedad
        [Column("email")]
        public string email { get; set; } = string.Empty;

        [Column("puntos_lealtad")]
        public int puntos_lealtad { get; set; }

        [Column("activo")]
        public bool activo { get; set; } = true;

        [Column("fecha_registro")]
        public DateTime fecha_registro { get; set; } = DateTime.Now;
    }
}