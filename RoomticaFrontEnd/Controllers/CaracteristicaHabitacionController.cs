using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using Google.Protobuf.WellKnownTypes;

namespace RoomticaFrontEnd.Controllers
{
    public class CaracteristicaHabitacionController : Controller
    {
        private CaracteristicaHabitacionService.CaracteristicaHabitacionServiceClient ? caracteristicaHabitacionService;
        public async Task<ActionResult> Listar()
        {
            var canal  = GrpcChannel.ForAddress("http://localhost:5225");
            caracteristicaHabitacionService = new CaracteristicaHabitacionService.CaracteristicaHabitacionServiceClient(canal);
            var request = new Empty();
            var mensaje = await caracteristicaHabitacionService.GetAllAsync(request);
            List<CaracteristicaHabitacionModel> caracteristicaHabitacionModels = new List<CaracteristicaHabitacionModel>();
            foreach (var item in mensaje.CaracteristicaHabitaciones_)
            {
                caracteristicaHabitacionModels.Add(new CaracteristicaHabitacionModel()
                {
                    Id = item.Id,
                    Caracteristica = item.Caracteristica,
                    estado = item.Estado
                });
            }
            return View(caracteristicaHabitacionModels);
        }
        // GET: CaracteristicaHabitacion
        public ActionResult Index()
        {
            return View();
        }

        // GET: CaracteristicaHabitacion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CaracteristicaHabitacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CaracteristicaHabitacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CaracteristicaHabitacion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CaracteristicaHabitacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CaracteristicaHabitacion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CaracteristicaHabitacion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
