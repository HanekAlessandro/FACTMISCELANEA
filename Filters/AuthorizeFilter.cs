using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FactMiscelanea.Filters
{
    public class AuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Rutas que NO requieren autenticación
            var publicPaths = new[]
            {
                "/Account/Login",
                "/Account/Register",
                "/Account/Logout"
            };

            var currentPath = context.HttpContext.Request.Path;

            // Verificar si la ruta actual es pública
            foreach (var path in publicPaths)
            {
                if (currentPath.StartsWithSegments(path))
                {
                    return; // Permite acceso sin autenticación
                }
            }

            // Verificar si el usuario está autenticado
            var userId = context.HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null)
            {
                // Redirigir al login
                context.Result = new RedirectToPageResult("/Account/Login");
            }
        }
    }
}