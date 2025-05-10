using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Channels;

namespace RoomticaFrontEnd.Controllers
{
    public class CaracteristicaHabitacionController : Controller
    {
        private CaracteristicaHabitacionService.CaracteristicaHabitacionServiceClient ? caracteristicaHabitacionService;
        private GrpcChannel? chanal;

        //CONTROLLER
        public CaracteristicaHabitacionController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            caracteristicaHabitacionService = new CaracteristicaHabitacionService.CaracteristicaHabitacionServiceClient(chanal);
        }

        //LISTAR
        async Task<IEnumerable<CaracteristicaHabitacionModel>> listarCaracteristicaHabitacion()
        {
            List<CaracteristicaHabitacionModel> caracteristicaHabitacionModel = new List<CaracteristicaHabitacionModel>();
            var request = new Empty();
            var mensaje = await caracteristicaHabitacionService.GetAllAsync(request);

            foreach (var item in mensaje.CaracteristicaHabitaciones_)
            {
                caracteristicaHabitacionModel.Add(new CaracteristicaHabitacionModel()
                {
                    Id = item.Id,
                    Caracteristica = item.Caracteristica
                });
            }
            return caracteristicaHabitacionModel;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<CaracteristicaHabitacionModel> temporal = await listarCaracteristicaHabitacion();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.Caracteristica)
                    .ToLower()
                    .Contains(nombre.ToLower()));
            }

            int fila = 5;
            int c = temporal.Count();
            int pags = c % fila == 0 ? c / fila : c / fila + 1;
            ViewBag.p = p;
            ViewBag.pags = pags;
            ViewBag.nombre = nombre;
            ViewBag.mensaje = mensaje;
            return View(temporal.Skip(p * fila).Take(fila));
        }

        //CREATE
        async Task<string> guardarCaracteristicaHabitacion(CaracteristicaHabitacionModel caracteristicaHabitacion)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new CaracteristicaHabitacion()
                {
                    Id = caracteristicaHabitacion.Id,
                    Caracteristica = caracteristicaHabitacion.Caracteristica
                };
                var mensajeRespuesta = await caracteristicaHabitacionService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Caracteristica Habitacion Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Create()
        {
            return View(new CaracteristicaHabitacionModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CaracteristicaHabitacionModel caracteristicaHabitacion)
        {
            ViewBag.mensaje = await guardarCaracteristicaHabitacion(caracteristicaHabitacion);
            return View(caracteristicaHabitacion);
        }

        // DETAIL
        public async Task<ActionResult> Details(int id = 0)
        {
            CaracteristicaHabitacionModel caracteristicaHabitacion = await buscarCaracteristicaHabitacionPorId(id);
            return View(caracteristicaHabitacion);
        }

        //EDIT
        async Task<CaracteristicaHabitacionModel> buscarCaracteristicaHabitacionPorId(int id)
        {
            CaracteristicaHabitacionModel caracteristicaHabitacion = null;
            try
            {
                var request = new CaracteristicaHabitacionId()
                {
                    Id = id
                };
                var mensajeRespuesta = await caracteristicaHabitacionService.GetByIdAsync(request);
                caracteristicaHabitacion = new CaracteristicaHabitacionModel()
                {
                    Id = mensajeRespuesta.Id,
                    Caracteristica = mensajeRespuesta.Caracteristica
                };

            }
            catch (Exception ex) { return null; }
            return caracteristicaHabitacion;
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            CaracteristicaHabitacionModel caracteristicaHabitacion = await buscarCaracteristicaHabitacionPorId(id);
            return View(caracteristicaHabitacion);
        }
        //----------------------------------- EDIT TAMBIEN
        async Task<string> actualizarCaracteristicaHabitacion(CaracteristicaHabitacionModel caracteristicaHabitacion)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new CaracteristicaHabitacion()
                {
                    Id = caracteristicaHabitacion.Id,
                    Caracteristica = caracteristicaHabitacion.Caracteristica
                };
                var mensajeRespuesta = await caracteristicaHabitacionService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Caracteristica de Habitacion Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CaracteristicaHabitacionModel caracteristicaHabitacion)
        {
            ViewBag.mensaje = await actualizarCaracteristicaHabitacion(caracteristicaHabitacion);
            return View(caracteristicaHabitacion);
        }

        //DELETE
        async Task<string> eliminarCaracteristicaHabitacion(int id)
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

        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarCaracteristicaHabitacion(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }

    }
}
