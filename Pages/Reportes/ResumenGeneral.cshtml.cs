using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Reportes
{
    public class ResumenGeneralModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ResumenGeneralModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalProductos { get; set; }
        public int TotalClientes { get; set; }
        public int TotalFacturas { get; set; }
        public int TotalCategorias { get; set; }
        public int ProductosStockBajo { get; set; }
        public decimal IngresoTotal { get; set; }

        public async Task OnGetAsync()
        {
            TotalProductos = await _context.Productos.CountAsync();
            TotalClientes = await _context.Clientes.CountAsync();
            TotalFacturas = await _context.Facturas.CountAsync();
            TotalCategorias = await _context.Categorias.CountAsync();
            ProductosStockBajo = await _context.Productos.CountAsync(p => p.stock_actual <= p.stock_minimo);
            IngresoTotal = await _context.Facturas.SumAsync(f => f.total_final);
        }
    }
}