using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace FactMiscelanea.Pages.Facturas
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
            Factura = new Factura();
            ClientesList = new List<SelectListItem>();
            MetodosPagoList = new List<SelectListItem>();
            ProductosList = new List<SelectListItem>();
            ProductosDataJson = "[]";
        }

        [BindProperty]
        public Factura Factura { get; set; }

        public List<SelectListItem> ClientesList { get; set; }
        public List<SelectListItem> MetodosPagoList { get; set; }
        public List<SelectListItem> ProductosList { get; set; }
        public string ProductosDataJson { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Clientes
            ClientesList = await _context.Clientes
                .Select(c => new SelectListItem { Value = c.id_cliente.ToString(), Text = c.nombre })
                .ToListAsync();

            // Métodos de pago
            MetodosPagoList = await _context.MetodosPago
                .Select(m => new SelectListItem { Value = m.id_metodo.ToString(), Text = m.nombre_metodo })
                .ToListAsync();

            // Productos con precio y stock
            var productosData = await (from p in _context.Productos
                                       join pr in _context.PreciosPromociones on p.id_producto equals pr.id_producto into precios
                                       from pr in precios.DefaultIfEmpty()
                                       where p.activo
                                       select new
                                       {
                                           p.id_producto,
                                           p.nombre,
                                           p.stock_actual,
                                           Precio = pr != null ? pr.precio_final : 0
                                       }).ToListAsync();

            ProductosList = productosData.Select(p => new SelectListItem
            {
                Value = p.id_producto.ToString(),
                Text = $"{p.nombre} - Stock: {p.stock_actual} - Precio: ₡{p.Precio:N2}"
            }).ToList();

            // Serializar datos para JavaScript
            var productosJson = productosData.Select(p => new
            {
                id = p.id_producto,
                precio = p.Precio,
                stock = p.stock_actual,
                nombre = p.nombre
            }).ToList();

            ProductosDataJson = JsonSerializer.Serialize(productosJson);

            // Folio
            var ultimaFactura = await _context.Facturas.OrderByDescending(f => f.id_factura).FirstOrDefaultAsync();
            int nuevoNumero = (ultimaFactura?.id_factura ?? 0) + 1;
            Factura.folio = $"FAC-{nuevoNumero:D8}";
            Factura.fecha_hora = DateTime.Now;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarListas();
                return Page();
            }

            if (Factura.id_cliente <= 0)
            {
                ModelState.AddModelError("Factura.id_cliente", "Seleccione un cliente");
                await CargarListas();
                return Page();
            }

            if (Factura.id_metodo_pago <= 0)
            {
                ModelState.AddModelError("Factura.id_metodo_pago", "Seleccione un método de pago");
                await CargarListas();
                return Page();
            }

            // CORRECCIÓN DE FECHA - Formato válido para SQL Server
            var now = DateTime.Now;
            var fechaSegura = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            // Crear la factura con valores seguros
            var nuevaFactura = new Factura
            {
                folio = Factura.folio,
                fecha_hora = fechaSegura,
                id_cliente = Factura.id_cliente,
                id_metodo_pago = Factura.id_metodo_pago,
                id_usuario = 1,
                subtotal_sin_iva = Factura.subtotal_sin_iva > 0 ? Factura.subtotal_sin_iva : 0,
                total_iva = Factura.total_iva > 0 ? Factura.total_iva : 0,
                total_final = Factura.total_final > 0 ? Factura.total_final : 0,
                anulada = false
            };

            _context.Facturas.Add(nuevaFactura);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Factura {nuevaFactura.folio} creada exitosamente";
            return RedirectToPage("./Index");
        }

        private async Task CargarListas()
        {
            ClientesList = await _context.Clientes
                .Select(c => new SelectListItem { Value = c.id_cliente.ToString(), Text = c.nombre })
                .ToListAsync();

            MetodosPagoList = await _context.MetodosPago
                .Select(m => new SelectListItem { Value = m.id_metodo.ToString(), Text = m.nombre_metodo })
                .ToListAsync();

            var productosData = await (from p in _context.Productos
                                       join pr in _context.PreciosPromociones on p.id_producto equals pr.id_producto into precios
                                       from pr in precios.DefaultIfEmpty()
                                       where p.activo
                                       select new
                                       {
                                           p.id_producto,
                                           p.nombre,
                                           p.stock_actual,
                                           Precio = pr != null ? pr.precio_final : 0
                                       }).ToListAsync();

            ProductosList = productosData.Select(p => new SelectListItem
            {
                Value = p.id_producto.ToString(),
                Text = $"{p.nombre} - Stock: {p.stock_actual} - Precio: ₡{p.Precio:N2}"
            }).ToList();
        }
    }
}