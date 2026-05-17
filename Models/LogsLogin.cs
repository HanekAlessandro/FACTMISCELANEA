using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Logs_Login")]
    public class LogsLogin
    {
        [Key]
        [Column("id_log")]
        public int id_log { get; set; }

        [Column("id_usuario")]
        public int? id_usuario { get; set; }

        [Column("nombre_usuario")]
        public string nombre_usuario { get; set; } = string.Empty;

        [Column("ip_address")]
        public string ip_address { get; set; } = string.Empty;

        [Column("user_agent")]
        public string user_agent { get; set; } = string.Empty;

        [Column("resultado")]
        public string resultado { get; set; } = string.Empty;

        [Column("fecha_intento")]
        public DateTime fecha_intento { get; set; } = DateTime.Now;

        // Propiedades de navegación
        [ForeignKey("id_usuario")]
        public virtual Usuario? Usuario { get; set; }
    }
}