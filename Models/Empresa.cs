using System.ComponentModel.DataAnnotations;

namespace FactMiscelanea.Models
{
    public class Empresa
    {
        [Key]
        public int id_empresa { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre_comercial { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string nit_cedula_juridica { get; set; } = string.Empty;

        public string? direccion { get; set; }

        [StringLength(8)]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener 8 dígitos.")]
        public string? telefono { get; set; }

        [StringLength(255)]
        public string? mensaje_pie_factura { get; set; }
    }
}