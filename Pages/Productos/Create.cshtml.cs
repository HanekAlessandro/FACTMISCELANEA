using FactMiscelanea.Data;
using FactMiscelanea.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FactMiscelanea.Pages.Productos
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new();

        public SelectList Categorias { get; set; } = default!;

        public void OnGet()
        {
            Categorias = new SelectList(
                _context.Categorias.ToList(),
                "id_categoria",
                "nombre_categoria"
            );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categorias = new SelectList(
                    _context.Categorias.ToList(),
                    "id_categoria",
                    "nombre_categoria"
                );

                return Page();
            }

            _context.Productos.Add(Producto);

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}