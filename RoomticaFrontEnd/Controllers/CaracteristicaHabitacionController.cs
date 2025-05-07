using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using Google.Protobuf.WellKnownTypes;

namespace RoomticaFrontEnd.Controllers
{
    public class CaracteristicaHabitacionController : Controller
    {
        private CaracteristicaHabitacionService.CaracteristicaHabitacionServiceClient? caracteristicaHabitacionService;
        private GrpcChannel? canal;

        public CaracteristicaHabitacionController()
        {
            canal = GrpcChannel.ForAddress("http://localhost:5225");
            caracteristicaHabitacionService = new CaracteristicaHabitacionService.CaracteristicaHabitacionServiceClient(canal);
        }
        private async Task<IEnumerable<CaracteristicaHabitacionModel>> listarCaracteristicasHabitacion()
        {
            List<CaracteristicaHabitacionModel> lista = new List<CaracteristicaHabitacionModel>();
            var request = new Empty();
            var mensaje = await caracteristicaHabitacionService.GetAllAsync(request);

            foreach (var item in mensaje.CaracteristicaHabitaciones_)
            {
                lista.Add(new CaracteristicaHabitacionModel
                {
                    Id = item.Id,
                    Caracteristica = item.Caracteristica
                });
            }
            return lista;
        }

        private async Task<CaracteristicaHabitacionModel?> buscarCaracteristicaHabitacionPorId(int id)
        {
            try
            {
                var request = new CaracteristicaHabitacionId { Id = id };
                var respuesta = await caracteristicaHabitacionService.GetByIdAsync(request);

                return new CaracteristicaHabitacionModel
                {
                    Id = respuesta.Id,
                    Caracteristica = respuesta.Caracteristica
                };
            }
            catch
            {
                return null;
            }
        }

        private async Task<string> guardarCaracteristicaHabitacion(CaracteristicaHabitacionModel model)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new CaracteristicaHabitacion()
                {
                    Id = model.Id,
                    Caracteristica = model.Caracteristica

                };
                var mensajeRespuesta = await caracteristicaHabitacionService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Caracteristica Habitacion Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        private async Task<string> actualizarCaracteristicaHabitacion(CaracteristicaHabitacionModel model)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new CaracteristicaHabitacion()
                {
                    Id = model.Id,
                    Caracteristica = model.Caracteristica
                };
                var mensajeRespuesta = await caracteristicaHabitacionService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Caracteristica Habitacion Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        private async Task<string> eliminarCaracteristicaHabitacion(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new CaracteristicaHabitacionId()
                {
                    Id = id
                };
                var mensajeRespuesta = await caracteristicaHabitacionService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Caracteristica Habitacion Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }





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
                    Caracteristica = item.Caracteristica
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
