using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;

namespace RoomticaFrontEnd.Permisos
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            if (session.GetString("trabajador") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            else
            {
                var empleadoJson = session.GetString("trabajador");
                var empleado = System.Text.Json.JsonSerializer.Deserialize<TrabajadorModel>(empleadoJson);
                context.HttpContext.Items["trabajador"] = empleado;
            }
            base.OnActionExecuting(context);
        }
    }
}
