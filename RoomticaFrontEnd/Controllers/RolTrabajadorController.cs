using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using static RoomticaGrpcServiceBackEnd.CategoriaProductoService;
using static RoomticaGrpcServiceBackEnd.RolTrabajadorService;

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

        public async Task<ActionResult> Detail(int id = 0)
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            rolTrabajadorService = new RolTrabajadorService.RolTrabajadorServiceClient(canal);
            var request = new RolTrabajadorId()
            {
                Id = id,
            };
            var mensaje = await rolTrabajadorService.GetByIdAsync(request);

            RolTrabajadorModel rolTrabajadorModel = new RolTrabajadorModel()
            {
                Id = mensaje.Id,
                rol = mensaje.Rol
            };
            return View(rolTrabajadorModel);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
