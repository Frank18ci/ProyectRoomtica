using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class ReservaController : Controller
    {
        private ReservaService.ReservaServiceClient? reservaService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            reservaService = new ReservaService.ReservaServiceClient(canal);
            var request = new Empty();
            var mensaje = await reservaService.GetAllAsync(request);
            List<ReservaDTOModel> reservaDTOModels = new List<ReservaDTOModel>();
            foreach (var item in mensaje.Reservas_)
            {
                reservaDTOModels.Add(new ReservaDTOModel()
                {
                    id = item.Id,
                    id_habitacion = item.IdHabitacion,
                    id_cliente = item.IdCliente,
                    id_trabajador = item.IdTrabajador,
                    id_tipo_reserva = item.IdTipoReserva,
                    fecha_ingreso = item.FechaIngreso,
                    fecha_salida = item.FechaSalida,
                    costo_alojamiento = item.CostoAlojamiento
                });
            }
            return View(reservaDTOModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
