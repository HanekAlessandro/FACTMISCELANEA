using System.ComponentModel.DataAnnotations;

namespace FactMiscelanea.Models
{
    public class MetodoPago
    {
        [Key]
        public int id_metodo { get; set; }

        [Required]
        [StringLength(30)]
        public string nombre_metodo { get; set; } = string.Empty;

        public ICollection<Factura>? Facturas { get; set; }
    }
}