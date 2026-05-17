using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Models;

namespace FactMiscelanea.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablas existentes
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<MetodoPago> MetodosPago { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Devolucion> Devoluciones { get; set; }
        public DbSet<Kardex> Kardex { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<PrecioPromocion> PreciosPromociones { get; set; }

        // =========================================================
        // NUEVAS TABLAS (Agregar estos DbSets)
        // =========================================================
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }
        public DbSet<LogsLogin> LogsLogin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tablas existentes
            modelBuilder.Entity<Categoria>().ToTable("Categorias");
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Producto>().ToTable("Productos");
            modelBuilder.Entity<Factura>().ToTable("Facturas");
            modelBuilder.Entity<FacturaDetalle>().ToTable("Factura_Detalle");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<MetodoPago>().ToTable("Metodos_Pago");
            modelBuilder.Entity<Proveedor>().ToTable("Proveedores");
            modelBuilder.Entity<Devolucion>().ToTable("Devoluciones");
            modelBuilder.Entity<Kardex>().ToTable("Kardex");
            modelBuilder.Entity<Empresa>().ToTable("Empresa");
            modelBuilder.Entity<PrecioPromocion>().ToTable("Precios_Promociones");

            // =========================================================
            // NUEVAS TABLAS
            // =========================================================
            modelBuilder.Entity<Compra>().ToTable("Compras");
            modelBuilder.Entity<DetalleCompra>().ToTable("Detalle_Compras");
            modelBuilder.Entity<LogsLogin>().ToTable("Logs_Login");

            // =========================================================
            // CONFIGURACIONES ADICIONALES PARA LAS NUEVAS TABLAS
            // =========================================================
            
            // Configuración para Compra
            modelBuilder.Entity<Compra>()
                .HasKey(c => c.id_compra);
            
            modelBuilder.Entity<Compra>()
                .Property(c => c.folio)
                .IsRequired()
                .HasMaxLength(20);
            
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Proveedor)
                .WithMany()
                .HasForeignKey(c => c.id_proveedor);
            
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.id_usuario);

            // Configuración para DetalleCompra
            modelBuilder.Entity<DetalleCompra>()
                .HasKey(d => d.id_detalle_compra);
            
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.Compra)
                .WithMany(c => c.Detalles)
                .HasForeignKey(d => d.id_compra)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.id_producto);

            // Configuración para LogsLogin
            modelBuilder.Entity<LogsLogin>()
                .HasKey(l => l.id_log);
            
            modelBuilder.Entity<LogsLogin>()
                .HasOne(l => l.Usuario)
                .WithMany()
                .HasForeignKey(l => l.id_usuario)
                .IsRequired(false);
        }
    }
}