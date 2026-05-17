using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Compras")]
    public class Compra
    {
        [Key]
        [Column("id_compra")]
        public int id_compra { get; set; }

        [Column("folio")]
        public string folio { get; set; } = string.Empty;

        [Column("fecha_hora")]
        public DateTime fecha_hora { get; set; } = DateTime.Now;

        [Column("id_proveedor")]
        public int id_proveedor { get; set; }

        [Column("id_usuario")]
        public int id_usuario { get; set; }

        [Column("id_empresa")]
        public int? id_empresa { get; set; } = 1;

        [Column("subtotal")]
        public decimal subtotal { get; set; }

        [Column("iva_total")]
        public decimal iva_total { get; set; }

        [Column("total_compra")]
        public decimal total_compra { get; set; }

        [Column("factura_proveedor")]
        public string factura_proveedor { get; set; } = string.Empty;

        [Column("tipo_comprobante")]
        public string tipo_comprobante { get; set; } = "Factura";

        [Column("estado")]
        public string estado { get; set; } = "Completada";

        [Column("observaciones")]
        public string observaciones { get; set; } = string.Empty;

        // Propiedades de navegación
        [ForeignKey("id_proveedor")]
        public virtual Proveedor? Proveedor { get; set; }

        [ForeignKey("id_usuario")]
        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<DetalleCompra>? Detalles { get; set; }
    }
}