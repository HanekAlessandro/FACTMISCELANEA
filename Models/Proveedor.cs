using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Proveedores")]
    public class Proveedor
    {
        [Key]
        [Column("id_proveedor")]
        public int IdProveedor { get; set; }

        [Column("nombre_empresa")]
        [Required]
        [StringLength(100)]
        public string NombreEmpresa { get; set; } = string.Empty;

        [Column("contacto_nombre")]
        [StringLength(100)]
        public string? ContactoNombre { get; set; }

        [Column("telefono")]
        [StringLength(8)]
        public string? Telefono { get; set; }

        [Column("direccion")]
        public string? Direccion { get; set; }
    }
}