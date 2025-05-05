using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class PagoController : Controller
    {
        private PagoService.PagoServiceClient? pagoService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            pagoService = new PagoService.PagoServiceClient(canal);
            var request = new Empty();
            var mensaje = await pagoService.GetAllAsync(request);
            List<PagoDTOModel> pagoDTOModels = new List<PagoDTOModel>();
            foreach (var item in mensaje.Pagos_)
            {
                pagoDTOModels.Add(new PagoDTOModel()
                {
                    id = item.Id,
                    id_reserva = item.IdReserva,
                    id_tipo_comprobante = item.IdTipoComprobante,
                    igv = item.Igv,
                    total_pago = item.TotalPago,
                    fecha_emision = item.FechaEmision,
                    fecha_pago = item.FechaPago
                });
            }
            return View(pagoDTOModels);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
