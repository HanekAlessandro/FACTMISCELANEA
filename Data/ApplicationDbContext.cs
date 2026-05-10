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

        // =============================================
        // DBSETS - ENTIDADES
        // =============================================

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

        // =============================================
        // CONFIGURACIÓN DE MODELOS
        // =============================================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =============================================
            // CONFIGURACIÓN DE DECIMALES (Precisión)
            // =============================================

            // Facturas
            modelBuilder.Entity<Factura>()
                .Property(f => f.subtotal_sin_iva)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Factura>()
                .Property(f => f.total_iva)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Factura>()
                .Property(f => f.total_final)
                .HasPrecision(18, 2);

            // Factura Detalle
            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.precio_unitario_aplicado)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.iva_aplicado)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.subtotal_linea)
                .HasPrecision(18, 2);

            // Precios y Promociones
            modelBuilder.Entity<PrecioPromocion>()
                .Property(p => p.costo_compra)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PrecioPromocion>()
                .Property(p => p.precio_sugerido)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PrecioPromocion>()
                .Property(p => p.precio_final)
                .HasPrecision(18, 2);

            // Productos (IVA)
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

            // =============================================
            // CONFIGURACIÓN DE ÍNDICES Y RESTRICCIONES
            // =============================================

            // Índices únicos
            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.nombre_categoria)
                .IsUnique();

            modelBuilder.Entity<MetodoPago>()
                .HasIndex(mp => mp.nombre_metodo)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.nombre_usuario)
                .IsUnique();

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.identificacion)
                .IsUnique();

            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.codigo_barras)
                .IsUnique();

            modelBuilder.Entity<Factura>()
                .HasIndex(f => f.folio)
                .IsUnique();

            // =============================================
            // CONFIGURACIÓN DE VALORES POR DEFECTO
            // =============================================

            modelBuilder.Entity<Cliente>()
                .Property(c => c.nombre)
                .HasDefaultValue("Público General");

            modelBuilder.Entity<Cliente>()
                .Property(c => c.puntos_lealtad)
                .HasDefaultValue(0);

            modelBuilder.Entity<Producto>()
                .Property(p => p.iva_porcentaje)
                .HasDefaultValue(15.00m);

            modelBuilder.Entity<Producto>()
                .Property(p => p.stock_actual)
                .HasDefaultValue(0);

            modelBuilder.Entity<Producto>()
                .Property(p => p.stock_minimo)
                .HasDefaultValue(5);

            modelBuilder.Entity<PrecioPromocion>()
                .Property(pp => pp.es_promocional)
                .HasDefaultValue(false);

            modelBuilder.Entity<Factura>()
                .Property(f => f.fecha_hora)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Devolucion>()
                .Property(d => d.fecha_devolucion)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Kardex>()
                .Property(k => k.fecha)
                .HasDefaultValueSql("GETDATE()");

            // =============================================
            // CONFIGURACIÓN DE RELACIONES
            // =============================================

            // Categoria - Producto (relación uno a muchos)
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.id_categoria)
                .OnDelete(DeleteBehavior.Restrict);

            // Producto - PrecioPromocion (relación uno a uno)
            modelBuilder.Entity<PrecioPromocion>()
                .HasOne(pp => pp.Producto)
                .WithOne(p => p.PrecioPromocion)
                .HasForeignKey<PrecioPromocion>(pp => pp.id_producto)
                .OnDelete(DeleteBehavior.Cascade);

            // Factura - Cliente
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cliente)
                .WithMany(c => c.Facturas)
                .HasForeignKey(f => f.id_cliente)
                .OnDelete(DeleteBehavior.Restrict);

            // Factura - MetodoPago
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.MetodoPago)
                .WithMany(mp => mp.Facturas)
                .HasForeignKey(f => f.id_metodo_pago)
                .OnDelete(DeleteBehavior.Restrict);

            // Factura - Usuario
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.Facturas)
                .HasForeignKey(f => f.id_usuario)
                .OnDelete(DeleteBehavior.Restrict);

            // FacturaDetalle - Factura
            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(fd => fd.Factura)
                .WithMany(f => f.FacturaDetalles)
                .HasForeignKey(fd => fd.id_factura)
                .OnDelete(DeleteBehavior.Cascade);

            // FacturaDetalle - Producto
            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(fd => fd.Producto)
                .WithMany(p => p.FacturaDetalles)
                .HasForeignKey(fd => fd.id_producto)
                .OnDelete(DeleteBehavior.Restrict);

            // Kardex - Producto
            modelBuilder.Entity<Kardex>()
                .HasOne(k => k.Producto)
                .WithMany(p => p.Kardex)
                .HasForeignKey(k => k.id_producto)
                .OnDelete(DeleteBehavior.Restrict);

            // Devolucion - Factura
            modelBuilder.Entity<Devolucion>()
                .HasOne(d => d.Factura)
                .WithMany(f => f.Devoluciones)
                .HasForeignKey(d => d.id_factura)
                .OnDelete(DeleteBehavior.Restrict);

            // Devolucion - Producto
            modelBuilder.Entity<Devolucion>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.Devoluciones)
                .HasForeignKey(d => d.id_producto)
                .OnDelete(DeleteBehavior.Restrict);

            // Devolucion - Usuario
            modelBuilder.Entity<Devolucion>()
                .HasOne(d => d.UsuarioAutoriza)
                .WithMany(u => u.Devoluciones)
                .HasForeignKey(d => d.id_usuario_autoriza)
                .OnDelete(DeleteBehavior.Restrict);
        }

        // =============================================
        // MÉTODO AUXILIAR PARA MARCAR MIGRACIÓN APLICADA
        // =============================================
        
        public void MarkMigrationAsApplied(string migrationId = "20260510034559_InitialCreate")
        {
            try
            {
                Database.ExecuteSqlRaw(
                    "IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = {0}) " +
                    "INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ({0}, '8.0.0')",
                    migrationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al marcar migración: {ex.Message}");
            }
        }

        // =============================================
        // MÉTODO PARA VERIFICAR CONEXIÓN
        // =============================================
        
        public bool CanConnect()
        {
            return Database.CanConnect();
        }
    }
}