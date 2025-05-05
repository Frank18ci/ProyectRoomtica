using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class EstadoHabitacionController : Controller
    {
        EstadoHabitacionServices.EstadoHabitacionServicesClient? estadoHabitacionService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            estadoHabitacionService = new EstadoHabitacionServices.EstadoHabitacionServicesClient(canal);
            var request = new Empty();
            var mensaje = await estadoHabitacionService.GetAllAsync(request);
            List<EstadoHabitacionModel> estadoHabitacionModels = new List<EstadoHabitacionModel>();
            foreach (var item in mensaje.EstadoHabitaciones_)
            {
                estadoHabitacionModels.Add(new EstadoHabitacionModel()
                {
                    id = item.Id,
                    estado_habitacion = item.EstadoHabitacion_
                });
            }
            return View(estadoHabitacionModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
