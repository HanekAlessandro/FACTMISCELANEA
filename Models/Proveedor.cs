using System.ComponentModel.DataAnnotations;

namespace FactMiscelanea.Models
{
    public class Proveedor
    {
        [Key]
        public int id_proveedor { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre_empresa { get; set; } = string.Empty;

        [StringLength(100)]
        public string? contacto_nombre { get; set; }

        [StringLength(8)]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener 8 dígitos.")]
        public string? telefono { get; set; }

        public string? direccion { get; set; }
    }
}