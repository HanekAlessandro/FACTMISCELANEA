using FactMiscelanea.Models;
using Microsoft.EntityFrameworkCore;

namespace FactMiscelanea.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Empresa> Empresa { get; set; }

        public DbSet<MetodoPago> MetodosPago { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Proveedor> Proveedores { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<PrecioPromocion> PreciosPromociones { get; set; }

        public DbSet<Factura> Facturas { get; set; }

        public DbSet<FacturaDetalle> FacturaDetalles { get; set; }

        public DbSet<Devolucion> Devoluciones { get; set; }

        public DbSet<Kardex> Kardex { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =============================================
            // CONFIGURACIÓN DE DECIMALES
            // =============================================

            modelBuilder.Entity<Factura>()
                .Property(f => f.subtotal_sin_iva)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Factura>()
                .Property(f => f.total_iva)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Factura>()
                .Property(f => f.total_final)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.precio_unitario_aplicado)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.iva_aplicado)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.subtotal_linea)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PrecioPromocion>()
                .Property(p => p.costo_compra)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PrecioPromocion>()
                .Property(p => p.precio_sugerido)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PrecioPromocion>()
                .Property(p => p.precio_final)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.iva_porcentaje)
                .HasPrecision(5, 2);

            // =============================================
            // MAPEO EXACTO DE TABLAS SQL
            // =============================================

            modelBuilder.Entity<Categoria>()
                .ToTable("Categorias");

            modelBuilder.Entity<Cliente>()
                .ToTable("Clientes");

            modelBuilder.Entity<Empresa>()
                .ToTable("Empresa");

            modelBuilder.Entity<MetodoPago>()
                .ToTable("Metodos_Pago");

            modelBuilder.Entity<Usuario>()
                .ToTable("Usuarios");

            modelBuilder.Entity<Proveedor>()
                .ToTable("Proveedores");

            modelBuilder.Entity<Producto>()
                .ToTable("Productos");

            modelBuilder.Entity<PrecioPromocion>()
                .ToTable("Precios_Promociones");

            modelBuilder.Entity<Factura>()
                .ToTable("Facturas");

            modelBuilder.Entity<FacturaDetalle>()
                .ToTable("Factura_Detalle");

            modelBuilder.Entity<Devolucion>()
                .ToTable("Devoluciones");

            modelBuilder.Entity<Kardex>()
                .ToTable("Kardex");
        }
    }
}