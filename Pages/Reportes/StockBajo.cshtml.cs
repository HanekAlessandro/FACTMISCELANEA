using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Reportes
{
    public class StockBajoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StockBajoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Producto> ProductosStockBajo { get; set; } = new();

        public async Task OnGetAsync()
        {
            ProductosStockBajo = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.stock_actual <= p.stock_minimo)
                .OrderBy(p => p.stock_actual)
                .ToListAsync();
        }
    }
}