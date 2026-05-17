using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Cliente> Clientes { get; set; } = new();

        public async Task OnGetAsync()
        {
            Clientes = await _context.Clientes
                .OrderBy(c => c.nombre) 
                .ToListAsync();
        }
    }
}