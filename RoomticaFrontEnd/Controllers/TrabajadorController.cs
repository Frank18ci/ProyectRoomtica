using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class TrabajadorController : Controller
    {
        private TrabajadorService.TrabajadorServiceClient? trabajadorService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            trabajadorService = new TrabajadorService.TrabajadorServiceClient(canal);
            var request = new Empty();
            var mensaje = await trabajadorService.GetAllAsync(request);
            List<TrabajadorDTOModel> trabajadorDTOModels = new List<TrabajadorDTOModel>();
            foreach (var item in mensaje.Trabajadores_)
            {
                trabajadorDTOModels.Add(new TrabajadorDTOModel()
                {
                    id = item.Id,
                    primer_nombre = item.PrimerNombre,
                    segundo_nombre = item.SegundoNombre,
                    primer_apellido = item.PrimerApellido,
                    segundo_apellido = item.SegundoApellido,
                    username = item.Username,
                    password = item.Password,
                    sueldo = item.Sueldo,
                    id_tipo_documento = item.IdTipoDocumento,
                    numero_documento = item.NumeroDocumento,
                    telefono = item.Telefono,
                    email = item.Email,
                    id_rol = item.IdRol
                });
            }
            return View(trabajadorDTOModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
