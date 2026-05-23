using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Facturas")]
    public class Factura
    {
        [Key]
        [Column("id_factura")]
        public int id_factura { get; set; }

        [Column("folio")]
        public string folio { get; set; } = string.Empty;

        [Column("fecha_hora")]
        public DateTime fecha_hora { get; set; }

        [Column("id_cliente")]
        public int? id_cliente { get; set; }

        [Column("id_metodo_pago")]
        public int? id_metodo_pago { get; set; }

        [Column("id_usuario")]
        public int? id_usuario { get; set; }

        [Column("subtotal_sin_iva")]
        public decimal subtotal_sin_iva { get; set; }

        [Column("total_iva")]
        public decimal total_iva { get; set; }

        [Column("total_final")]
        public decimal total_final { get; set; }

        [Column("anulada")]
        public bool anulada { get; set; }

        [Column("motivo_anulacion")]
        public string? motivo_anulacion { get; set; }

        [ForeignKey("id_cliente")]
        public virtual Cliente? Cliente { get; set; }

        [ForeignKey("id_metodo_pago")]
        public virtual MetodoPago? MetodoPago { get; set; }

        [ForeignKey("id_usuario")]
        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();
    }
}