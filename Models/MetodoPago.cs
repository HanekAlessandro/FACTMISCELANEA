using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Metodos_Pago")]
    public class MetodoPago
    {
        [Key]
        [Column("id_metodo")]
        public int IdMetodo { get; set; }

        [Column("nombre_metodo")]
        [Required]
        [StringLength(30)]
        public string NombreMetodo { get; set; } = string.Empty;

        // Propiedades de navegación
        public virtual ICollection<Factura>? Facturas { get; set; }
    }
}