using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using System.Text.Json;

namespace RoomticaFrontEnd.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        private TrabajadorService.TrabajadorServiceClient? trabajadorService;
        async Task<TrabajadorModel> LoginEmpleado(string Username = "", string Clave = "")
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            trabajadorService = new TrabajadorService.TrabajadorServiceClient(canal);
            var request = new DatosLoginTrabajador()
            {
                Username = Username,
                Password = Clave
            };
            var mensaje = await trabajadorService.LoginAsync(request);
            if (mensaje.Id != 0)
            {
                TrabajadorModel trabajadorModel = new TrabajadorModel()
                {
                    id = mensaje.Id,
                    primer_nombre = mensaje.PrimerNombre,
                    segundo_nombre = mensaje.SegundoNombre,
                    primer_apellido = mensaje.PrimerApellido,
                    segundo_apellido = mensaje.SegundoApellido,
                    username = mensaje.Username,
                    password = mensaje.Password,
                    sueldo = mensaje.Sueldo,
                    id_tipo_documento = mensaje.IdTipoDocumento,
                    numero_documento = mensaje.NumeroDocumento,
                    telefono = mensaje.Telefono,
                    email = mensaje.Email,
                    id_rol = mensaje.IdRol
                };
                return trabajadorModel;
            }
            return null;
        }

        [HttpPost]
        public async Task<ActionResult> Login(string Username = "", string Clave = "") {
            TrabajadorModel trabajador = await LoginEmpleado(Username, Clave);
            if (trabajador != null)
            {
                string trabajadorJson = JsonSerializer.Serialize(trabajador);
                byte[] trabajadorBytes = System.Text.Encoding.UTF8.GetBytes(trabajadorJson);
                HttpContext.Session.Set("trabajador", trabajadorBytes);
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
            HttpContext.Session.Remove("trabajador");
            return RedirectToAction("Login", "Auth");
        }
    }
}
