using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System;
using System.Threading.Tasks;

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

        [BindProperty]
        public decimal Precio { get; set; }

        public SelectList CategoriasList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var categorias = await _context.Categorias.ToListAsync();
            CategoriasList = new SelectList(categorias, "id_categoria", "nombre_categoria");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validaciones
            if (Precio <= 0)
            {
                ModelState.AddModelError("Precio", "El precio debe ser mayor que cero");
            }

            if (!ModelState.IsValid)
            {
                var categorias = await _context.Categorias.ToListAsync();
                CategoriasList = new SelectList(categorias, "id_categoria", "nombre_categoria");
                return Page();
            }

            try
            {
                // 1. Guardar el producto
                Producto.activo = true;
                Producto.fecha_registro = DateTime.Now;
                _context.Productos.Add(Producto);
                await _context.SaveChangesAsync();

                // 2. Guardar el precio asociado
                var precioPromo = new PrecioPromocion
                {
                    id_producto = Producto.id_producto,
                    costo_compra = Precio,
                    precio_sugerido = Precio,
                    precio_final = Precio,
                    es_promocional = false,
                    fecha_actualizacion = DateTime.Now
                };
                _context.PreciosPromociones.Add(precioPromo);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Producto '{Producto.nombre}' creado exitosamente.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al guardar: {ex.Message}";
                var categorias = await _context.Categorias.ToListAsync();
                CategoriasList = new SelectList(categorias, "id_categoria", "nombre_categoria");
                return Page();
            }
        }
    }
}