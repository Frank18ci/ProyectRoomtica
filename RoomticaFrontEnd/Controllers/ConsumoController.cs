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
            List<ConsumoDTO> consumoModels = new List<ConsumoDTO>();
            foreach (var item in mensaje.Consumo)
            {
                consumoModels.Add(new ConsumoDTO()
                {
                    Id= item.Id,
                    IdReserva= item.IdReserva,
                    Cantidad = item.Cantidad,
                    PrecioVenta= item.PrecioVenta,
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
