using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Empresa")]
    public class Empresa
    {
        [Key]
        [Column("id_empresa")]
        public int IdEmpresa { get; set; } = 1;

        [Column("nombre_comercial")]
        [Required]
        [StringLength(100)]
        public string NombreComercial { get; set; } = string.Empty;

        [Column("nit_cedula_juridica")]
        [Required]
        [StringLength(50)]
        public string NitCedulaJuridica { get; set; } = string.Empty;

        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("telefono")]
        [StringLength(8)]
        public string? Telefono { get; set; }

        [Column("mensaje_pie_factura")]
        [StringLength(255)]
        public string? MensajePieFactura { get; set; }
    }
}