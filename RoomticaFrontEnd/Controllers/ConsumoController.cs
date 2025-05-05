using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class ConsumoController : Controller
    {
        private ConsumoService.ConsumoServiceClient? consumoService;
        public async Task<ActionResult> Listar()
        {
            var chanal = GrpcChannel.ForAddress("http://localhost:5225");
            consumoService = new ConsumoService.ConsumoServiceClient(chanal);
            var request = new Empty();
            var mensaje = await consumoService.GetAllAsync(request);
            List<ConsumoModel> consumoModels = new List<ConsumoModel>();
            foreach (var item in mensaje.Consumos_)
            {
                consumoModels.Add(new ConsumoModel()
                {
                    id = item.Id,
                    id_reserva = item.IdReserva,
                    id_producto = item.IdProducto,
                    cantidad = item.Cantidad,
                    precio_venta = item.PrecioVenta,
                });
            }
            return View(consumoModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
