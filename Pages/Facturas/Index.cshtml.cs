using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Facturas
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Factura> Facturas { get; set; } = new();

        public async Task OnGetAsync()
        {
            Facturas = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Usuario)
                .OrderByDescending(f => f.fecha_hora)
                .ToListAsync();
        }
    }
}