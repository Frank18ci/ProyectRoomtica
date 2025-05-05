using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class ReservaEstacionamientoController : Controller
    {
        private ReservaEstacionamientoService.ReservaEstacionamientoServiceClient? reservaEstacionamientoService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            reservaEstacionamientoService = new ReservaEstacionamientoService.ReservaEstacionamientoServiceClient(canal);
            var request = new Empty();
            var mensaje = await reservaEstacionamientoService.GetAllAsync(request);
            List<ReservaEstacionamientoDTOModel> reservaEstacionamientoDTOModels = new List<ReservaEstacionamientoDTOModel>();
            foreach (var item in mensaje.ReservaEstacionamientos_)
            {
                reservaEstacionamientoDTOModels.Add(new ReservaEstacionamientoDTOModel()
                {
                    id_reserva = item.IdReserva,
                    id_estacionamiento = item.IdEstacionamiento,
                    cantidad = item.Cantidad,
                    precio_estacionamiento = item.PrecioEstacionamiento
                });
            }
            return View(reservaEstacionamientoDTOModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
