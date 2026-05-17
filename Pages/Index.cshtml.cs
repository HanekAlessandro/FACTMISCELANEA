using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FactMiscelanea.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Propiedades para las estadísticas
        public int TotalProducts { get; set; }
        public int TotalClientes { get; set; }
        public int TotalFacturas { get; set; }
        public int TotalCategorias { get; set; }
        
        // Propiedad para el ingreso total
        public decimal TotalIngresos { get; set; }
        
        // Productos con stock bajo
        public List<Producto> ProductsStockBajo { get; set; } = new List<Producto>();
        
        // Últimas facturas
        public List<Factura> UltimasFacturas { get; set; } = new List<Factura>();

        public async Task OnGetAsync()
        {
            // Verificar si hay sesión activa (protección)
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
            {
                return;
            }

            // Obtener estadísticas
            TotalProducts = await _context.Productos.CountAsync();
            TotalClientes = await _context.Clientes.CountAsync();
            TotalFacturas = await _context.Facturas.CountAsync();
            TotalCategorias = await _context.Categorias.CountAsync();
            
            // Obtener ingreso total (suma de todas las facturas)
            TotalIngresos = await _context.Facturas.SumAsync(f => f.total_final);

            // Obtener productos con stock bajo
            ProductsStockBajo = await _context.Productos
                .Where(p => p.stock_actual <= p.stock_minimo)
                .OrderBy(p => p.stock_actual)
                .Take(10)
                .Include(p => p.Categoria)
                .ToListAsync();

            // Obtener últimas 5 facturas
            UltimasFacturas = await _context.Facturas
                .OrderByDescending(f => f.fecha_hora)
                .Take(5)
                .Include(f => f.Cliente)
                .Include(f => f.Usuario)
                .ToListAsync();
        }
    }
}