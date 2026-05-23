using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FactMiscelanea.Filters
{
    public class AuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // rutas publicas
            var publicPaths = new[]
            {
                "/Account/Login",
                "/Account/Register",
                "/Account/Logout"
            };

            var currentPath = context.HttpContext.Request.Path;

            // validar rutas publicas
            foreach (var path in publicPaths)
            {
                if (currentPath.StartsWithSegments(path))
                {
                    return;
                }
            }

            // validar sesion
            var userId = context.HttpContext.Session.GetInt32("UsuarioId");

            if (userId == null)
            {
                context.Result = new RedirectToPageResult("/Account/Login");
            }
        }
    }
}