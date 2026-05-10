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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar nombres de tablas (coinciden con tu script SQL)
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
        }
    }
}