using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class TipoComprobanteController : Controller
    {
        private TipoComprobanteService.TipoComprobanteServiceClient? tipoComprobanteService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoComprobanteService = new TipoComprobanteService.TipoComprobanteServiceClient(canal);
            var request = new Empty();
            var mensaje = await tipoComprobanteService.GetAllAsync(request);
            List<TipoComprobanteModel> tipoComprobanteModels = new List<TipoComprobanteModel>();
            foreach (var item in mensaje.TipoComprobantes_)
            {
                tipoComprobanteModels.Add(new TipoComprobanteModel()
                {
                    id = item.Id,
                    tipo = item.Tipo
                });
            }
            return View(tipoComprobanteModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
