using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UsuarioId") != null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.nombre_usuario == Input.NombreUsuario);

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Usuario o contraseña incorrectos";
                return Page();
            }

            if (usuario.estado != "Activo")
            {
                TempData["ErrorMessage"] = "Usuario bloqueado. Contacte al administrador";
                return Page();
            }

            // Verificar contraseña - Comparar byte[]
            var passwordHash = HashPasswordToBytes(Input.Password);
            if (!CompareByteArrays(usuario.password_hash, passwordHash))
            {
                usuario.intentos_fallidos++;
                if (usuario.intentos_fallidos >= 5)
                {
                    usuario.estado = "Bloqueado";
                    TempData["ErrorMessage"] = "Usuario bloqueado por múltiples intentos fallidos";
                }
                await _context.SaveChangesAsync();
                
                if (usuario.estado != "Bloqueado")
                {
                    TempData["ErrorMessage"] = "Usuario o contraseña incorrectos";
                }
                return Page();
            }

            usuario.intentos_fallidos = 0;
            usuario.ultimo_acceso = System.DateTime.Now;
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UsuarioId", usuario.id_usuario);
            HttpContext.Session.SetString("UsuarioNombre", usuario.nombre_completo);
            HttpContext.Session.SetString("UsuarioRol", usuario.rol);
            HttpContext.Session.SetString("UsuarioNombreUsuario", usuario.nombre_usuario);

            TempData["SuccessMessage"] = $"¡Bienvenido {usuario.nombre_completo}!";
            
            return RedirectToPage("/Index");
        }

        private byte[] HashPasswordToBytes(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            return sha256.ComputeHash(bytes);
        }

        private bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length) return false;
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i]) return false;
            }
            return true;
        }
    }

    public class LoginInputModel
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}