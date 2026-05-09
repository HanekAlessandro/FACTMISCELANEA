using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    public class Factura
    {
        [Key]
        public int id_factura { get; set; }

        [Required]
        [StringLength(20)]
        public string folio { get; set; } = string.Empty;

        public DateTime fecha_hora { get; set; } = DateTime.Now;

        public int? id_cliente { get; set; }

        public int? id_metodo_pago { get; set; }

        public int? id_usuario { get; set; }

        public decimal? subtotal_sin_iva { get; set; }

        public decimal? total_iva { get; set; }

        public decimal? total_final { get; set; }

        [ForeignKey("id_cliente")]
        public Cliente? Cliente { get; set; }

        [ForeignKey("id_metodo_pago")]
        public MetodoPago? MetodoPago { get; set; }

        [ForeignKey("id_usuario")]
        public Usuario? Usuario { get; set; }

        public ICollection<FacturaDetalle>? Detalles { get; set; }

        public ICollection<Devolucion>? Devoluciones { get; set; }
    }
}