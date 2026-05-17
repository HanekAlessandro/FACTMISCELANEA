using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Metodos_Pago")]
    public class MetodoPago
    {
        [Key]
        [Column("id_metodo")]
        public int id_metodo { get; set; }

        [Column("nombre_metodo")]
        public string nombre_metodo { get; set; } = string.Empty;

        [Column("activo")]
        public bool activo { get; set; } = true;
    }
}