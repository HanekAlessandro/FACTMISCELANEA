using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Facturas")]
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

        public decimal subtotal_sin_iva { get; set; } = 0;

        public decimal total_iva { get; set; } = 0;

        public decimal total_final { get; set; } = 0;

        [ForeignKey("id_cliente")]
        public virtual Cliente? Cliente { get; set; }

        [ForeignKey("id_metodo_pago")]
        public virtual MetodoPago? MetodoPago { get; set; }

        [ForeignKey("id_usuario")]
        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<FacturaDetalle>? FacturaDetalles { get; set; }
    }
}