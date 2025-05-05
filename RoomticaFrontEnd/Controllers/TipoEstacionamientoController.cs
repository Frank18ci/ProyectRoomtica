using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class TipoEstacionamientoController : Controller
    {
        private TipoEstacionamientoService.TipoEstacionamientoServiceClient? tipoEstacionamientoService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoEstacionamientoService = new TipoEstacionamientoService.TipoEstacionamientoServiceClient(canal);
            var request = new Empty();
            var mensaje = await tipoEstacionamientoService.GetAllAsync(request);
            List<TipoEstacionamientoModel> tipoEstacionamientoModels = new List<TipoEstacionamientoModel>();
            foreach (var item in mensaje.TipoEstacionamientos_)
            {
                tipoEstacionamientoModels.Add(new TipoEstacionamientoModel()
                {
                    id = item.Id,
                    tipo = item.Tipo,
                    costo = item.Costo
                });
            }
            return View(tipoEstacionamientoModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
