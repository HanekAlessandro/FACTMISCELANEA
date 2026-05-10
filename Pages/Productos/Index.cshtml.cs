using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Producto> Productos { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Cargar productos REALES desde la base de datos
            Productos = await _context.Productos
                .Include(p => p.Categoria)
                .ToListAsync();
        }
    }
}