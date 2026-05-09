using System.ComponentModel.DataAnnotations;

namespace FactMiscelanea.Models
{
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
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener 8 dígitos.")]
        public string? telefono { get; set; }

        public int puntos_lealtad { get; set; } = 0;

        public ICollection<Factura>? Facturas { get; set; }
    }
}