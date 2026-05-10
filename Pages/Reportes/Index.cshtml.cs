using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;

namespace FactMiscelanea.Pages.Reportes
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetGetStockBajoCount()
        {
            var count = await _context.Productos
                .CountAsync(p => p.stock_actual <= p.stock_minimo);
            return Content(count.ToString());
        }

        public async Task<IActionResult> OnGetGetVentasCount()
        {
            var count = await _context.Facturas
                .GroupBy(f => new { f.fecha_hora.Year, f.fecha_hora.Month })
                .CountAsync();
            return Content(count.ToString());
        }

        public async Task<IActionResult> OnGetGetTopProductosCount()
        {
            var count = await _context.FacturaDetalles
                .GroupBy(d => d.id_producto)
                .CountAsync();
            var topCount = count > 10 ? 10 : count;
            return Content(topCount.ToString());
        }
    }
}