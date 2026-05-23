using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Empresa")]
    public class Empresa
    {
        [Key]
        [Column("id_empresa")]
        public int id_empresa { get; set; } = 1;

        [Column("nombre_comercial")]
        [Required]
        [StringLength(100)]
        public string nombre_comercial { get; set; } = string.Empty;

        [Column("nit_cedula_juridica")]
        [Required]
        [StringLength(50)]
        public string nit_cedula_juridica { get; set; } = string.Empty;

        [Column("direccion")]
        public string? direccion { get; set; }

        [Column("telefono")]
        [StringLength(8)]
        public string? telefono { get; set; }

        [Column("mensaje_pie_factura")]
        [StringLength(255)]
        public string? mensaje_pie_factura { get; set; }
    }
}