using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class HabitacionController : Controller
    {
        private HabitacionServices.HabitacionServicesClient? habitacionService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            habitacionService = new HabitacionServices.HabitacionServicesClient(canal);
            var request = new Empty();
            var mensaje = await habitacionService.GetAllAsync(request);
            List<HabitacionDTOModel> habitacionDTOModels = new List<HabitacionDTOModel>();
            foreach (var item in mensaje.Habitaciones_)
            {
                habitacionDTOModels.Add(new HabitacionDTOModel()
                {
                    id = item.Id,
                    numero = item.Numero,
                    piso = item.Piso,
                    precio_diario = item.PrecioDiario,
                    id_tipo = item.IdTipo,
                    id_estado = item.IdEstado
                });
            }
            return View(habitacionDTOModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
