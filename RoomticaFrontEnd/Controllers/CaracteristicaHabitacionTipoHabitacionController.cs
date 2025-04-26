using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaGrpcServiceBackEnd;
using Google.Protobuf.WellKnownTypes;
using RoomticaFrontEnd.Models;
namespace RoomticaFrontEnd.Controllers
{
    public class CaracteristicaHabitacionTipoHabitacionController : Controller
    {
        private readonly CaracteristicaHabitacionTipoHabitacionService.CaracteristicaHabitacionTipoHabitacionServiceClient caracteristicaHabitacionTipoHabitacionService;

        public CaracteristicaHabitacionTipoHabitacionController()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            caracteristicaHabitacionTipoHabitacionService = new CaracteristicaHabitacionTipoHabitacionService.CaracteristicaHabitacionTipoHabitacionServiceClient(canal);
        }

        public async Task<IActionResult> Index()
        {
            if (caracteristicaHabitacionTipoHabitacionService == null)
            {
                return BadRequest("El servicio no está disponible.");
            }

            var request = new Empty();
            var respuesta = await caracteristicaHabitacionTipoHabitacionService.GetAllAsync(request);

            if (respuesta == null || respuesta.CaracteristicaHabitacionTipoHabitaciones_ == null)
            {
                return NotFound("No se encontraron datos.");
            }

            var modelos = respuesta.CaracteristicaHabitacionTipoHabitaciones_
                .Select(item => new CaracteristicaHabitacionTipoHabitacionModel
                {
                    IdCaracteristicaHabitacion = item.IdCaracteristicaHabitacion,
                    IdTipoHabitacion = item.IdTipoHabitacion,
                    Estado = item.Estado
                })
                .ToList();

            return View(modelos);
        }
    }
}
