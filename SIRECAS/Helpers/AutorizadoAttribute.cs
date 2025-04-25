using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SIRECAS.Helpers
{
    public class AutorizadoAttribute : ActionFilterAttribute
    {
        private readonly int[] _rolesPermitidos;

        public AutorizadoAttribute(params int[] rolesPermitidos)
        {
            _rolesPermitidos = rolesPermitidos;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            int? idUsuario = httpContext.Session.GetInt32("IdUsuario");
            int? idRol = httpContext.Session.GetInt32("IdRol");

            if (idUsuario == null || idRol == null || !_rolesPermitidos.Contains(idRol.Value))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
