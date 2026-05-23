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

        [Column("nit")]
        [StringLength(50)]
        public string? Nit { get; set; }

        [Column("contacto_nombre")]
        [StringLength(100)]
        public string? ContactoNombre { get; set; }

        [Column("contacto_email")]
        [StringLength(100)]
        public string? ContactoEmail { get; set; }

        [Column("telefono")]
        [StringLength(8)]
        public string? Telefono { get; set; }

        [Column("telefono_alterno")]
        [StringLength(8)]
        public string? TelefonoAlterno { get; set; }

        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("direccion_alterna")]
        public string? DireccionAlterna { get; set; }

        [Column("sitio_web")]
        [StringLength(255)]
        public string? SitioWeb { get; set; }

        [Column("condiciones_pago")]
        [StringLength(50)]
        public string CondicionesPago { get; set; } = "Contado";

        [Column("credito_maximo")]
        public decimal CreditoMaximo { get; set; }

        [Column("saldo_pendiente")]
        public decimal SaldoPendiente { get; set; }

        [Column("calificacion")]
        public int Calificacion { get; set; } = 3;

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("id_usuario_registro")]
        public int? IdUsuarioRegistro { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}