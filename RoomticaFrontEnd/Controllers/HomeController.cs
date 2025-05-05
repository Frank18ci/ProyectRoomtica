using System.Diagnostics;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HabitacionServices.HabitacionServicesClient? habitacionService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
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
            ViewBag.habitaciones = habitacionDTOModels;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
