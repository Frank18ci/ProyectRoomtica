using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class RolTrabajadorController : Controller
    {
        private RolTrabajadorService.RolTrabajadorServiceClient? rolTrabajadorService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            rolTrabajadorService = new RolTrabajadorService.RolTrabajadorServiceClient(canal);
            var request = new Empty();
            var mensaje = await rolTrabajadorService.GetAllAsync(request);
            List<RolTrabajadorModel> rolTrabajadorModels = new List<RolTrabajadorModel>();
            foreach (var item in mensaje.RolTrabajadores_)
            {
                rolTrabajadorModels.Add(new RolTrabajadorModel()
                {
                    Id = item.Id,
                    rol = item.Rol
                });
            }
            return View(rolTrabajadorModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
