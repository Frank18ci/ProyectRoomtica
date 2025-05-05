using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class TipoReservaController : Controller
    {
        private TipoReservaService.TipoReservaServiceClient? tipoReservaService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoReservaService = new TipoReservaService.TipoReservaServiceClient(canal);
            var request = new Empty();
            var mensaje = await tipoReservaService.GetAllAsync(request);
            List<TipoReservaModel> tipoReservaModels = new List<TipoReservaModel>();
            foreach (var item in mensaje.TipoReservas_)
            {
                tipoReservaModels.Add(new TipoReservaModel()
                {
                    id = item.Id,
                    tipo = item.Tipo
                });
            }
            return View(tipoReservaModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
