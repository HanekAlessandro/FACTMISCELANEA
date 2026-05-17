using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactMiscelanea.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int id_usuario { get; set; }

        [Column("nombre_completo")]
        public string nombre_completo { get; set; } = string.Empty;

        [Column("nombre_usuario")]
        public string nombre_usuario { get; set; } = string.Empty;

        [Column("email")]
        public string email { get; set; } = string.Empty;

        [Column("password_hash")]
        public byte[] password_hash { get; set; } = new byte[32];  // ← Cambiado a byte[]

        [Column("rol")]
        public string rol { get; set; } = "Cajero";

        [Column("estado")]
        public string estado { get; set; } = "Activo";

        [Column("intentos_fallidos")]
        public int intentos_fallidos { get; set; }

        [Column("ultimo_acceso")]
        public DateTime? ultimo_acceso { get; set; }

        [Column("ultimo_cambio_password")]
        public DateTime? ultimo_cambio_password { get; set; }

        [Column("token_recuperacion")]
        public string? token_recuperacion { get; set; }

        [Column("token_expiracion")]
        public DateTime? token_expiracion { get; set; }

        [Column("id_empresa")]
        public int? id_empresa { get; set; } = 1;

        [Column("registrado_por")]
        public int? registrado_por { get; set; }

        [Column("fecha_registro")]
        public DateTime fecha_registro { get; set; } = DateTime.Now;
    }
}