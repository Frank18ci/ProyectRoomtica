using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using System.Text.Json;

namespace RoomticaFrontEnd.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        TrabajadorDTOModel LoginEmpleado(string Username = "", string Clave = "")
        {
            return new TrabajadorDTOModel();
        }

        [HttpPost]
        public ActionResult Acceso(string Username = "", string Clave = "") {
            TrabajadorDTOModel trabajador = LoginEmpleado(Username, Clave);
            if (trabajador != null)
            {
                string trabajadorJson = JsonSerializer.Serialize(trabajador);
                byte[] trabajadorBytes = System.Text.Encoding.UTF8.GetBytes(trabajadorJson);
                HttpContext.Session.Set("empleado", trabajadorBytes);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.mensaje = "Trabajador no encontrado";
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("empleado");
            return RedirectToAction("Auth", "Acceso");
        }
    }
}
