using FactMiscelanea.Data;
using FactMiscelanea.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FactMiscelanea.Pages.Productos
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new();

        public SelectList Categorias { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            Producto = producto;

            Categorias = new SelectList(
                _context.Categorias,
                "id_categoria",
                "nombre_categoria"
            );

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categorias = new SelectList(
                    _context.Categorias,
                    "id_categoria",
                    "nombre_categoria"
                );

                return Page();
            }

            _context.Attach(Producto).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}