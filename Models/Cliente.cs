using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int id_cliente { get; set; }

        [StringLength(20)]
        public string? identificacion { get; set; }

        [StringLength(100)]
        public string nombre { get; set; } = "Público General";

        [StringLength(255)]
        public string? direccion { get; set; }

        [StringLength(8)]
        public string? telefono { get; set; }

        public int puntos_lealtad { get; set; } = 0;

        public virtual ICollection<Factura>? Facturas { get; set; }
    }
}