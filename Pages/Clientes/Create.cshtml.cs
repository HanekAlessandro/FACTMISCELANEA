using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Clientes
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cliente cliente { get; set; } = new Cliente();  // ← Inicializado

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cliente creado exitosamente";
            return RedirectToPage("./Index");
        }
    }
}