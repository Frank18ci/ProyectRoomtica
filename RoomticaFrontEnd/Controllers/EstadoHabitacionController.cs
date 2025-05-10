using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaFrontEnd.Permisos;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class EstadoHabitacionController : Controller
    {
        private EstadoHabitacionServices.EstadoHabitacionServicesClient? estadoHabitacionService;
        private GrpcChannel? chanal;

        //CONTROLLER
        public EstadoHabitacionController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            estadoHabitacionService = new EstadoHabitacionServices.EstadoHabitacionServicesClient(chanal);
        }

        //LISTAR
        async Task<IEnumerable<EstadoHabitacionModel>> listarEstadoHabitacion()
        {
            List<EstadoHabitacionModel> estadoHabitacionModel = new List<EstadoHabitacionModel>();
            var request = new Empty();
            var mensaje = await estadoHabitacionService.GetAllAsync(request);
            foreach (var item in mensaje.EstadoHabitaciones_)
            {
                estadoHabitacionModel.Add(new EstadoHabitacionModel()
                {
                    id = item.Id,
                    estado_habitacion = item.EstadoHabitacion_
                });
            }
            return estadoHabitacionModel;
        }
        [ValidarSesion]
        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<EstadoHabitacionModel> temporal = await listarEstadoHabitacion();
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.estado_habitacion)
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

       
        async Task<string> guardarEstadoHabitacion(EstadoHabitacionModel estadoHabitacion)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new EstadoHabitacion()
                {
                    Id = estadoHabitacion.id,
                    EstadoHabitacion_ = estadoHabitacion.estado_habitacion
                };
                var mensajeRespuesta = await estadoHabitacionService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Estado Habitacion Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        [ValidarSesion]
        public async Task<ActionResult> Create()
        {
            return View(new EstadoHabitacionModel());
        }
        [ValidarSesion]
        [HttpPost]
        public async Task<ActionResult> Create(EstadoHabitacionModel estadoHabitacion)
        {
            ViewBag.mensaje = await guardarEstadoHabitacion(estadoHabitacion);
            return View(estadoHabitacion);
        }

        [ValidarSesion]
        public async Task<ActionResult> Details(int id = 0)
        {
            EstadoHabitacionModel estadoHabitacion = await buscarEstadoHabitacionPorId(id);
            return View(estadoHabitacion);
        }

        //EDIT
        async Task<EstadoHabitacionModel> buscarEstadoHabitacionPorId(int id)
        {
            EstadoHabitacionModel estadoHabitacion = null;
            try
            {
                var request = new EstadoHabitacionId()
                {
                    Id = id
                };
                var mensajeRespuesta = await estadoHabitacionService.GetByIdAsync(request);
                estadoHabitacion = new EstadoHabitacionModel()
                {
                    id = mensajeRespuesta.Id,
                    estado_habitacion = mensajeRespuesta.EstadoHabitacion_
                };
            }
            catch (Exception ex) { throw null; }
            return estadoHabitacion;
        }
        [ValidarSesion]
        public async Task<ActionResult> Edit(int id = 0)
        {
            EstadoHabitacionModel estadoHabitacion = await buscarEstadoHabitacionPorId(id);
            return View(estadoHabitacion);
        }


        async Task<string> actualizarEstadoHabitacion(EstadoHabitacionModel estadoHabitacion)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new EstadoHabitacion()
                {
                    Id = estadoHabitacion.id,
                    EstadoHabitacion_ = estadoHabitacion.estado_habitacion
                };
                var mensajeRespuesta = await estadoHabitacionService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Estado Habitacion Actualizado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        [ValidarSesion]
        [HttpPost]
        public async Task<ActionResult> Edit(EstadoHabitacionModel estadoHabitacion)
        {
            ViewBag.mensaje = await actualizarEstadoHabitacion(estadoHabitacion);
            return View(estadoHabitacion);
        }

        //DELETE
        async Task<string> eliminarEstadoHabitacion(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new EstadoHabitacionId()
                {
                    Id = id
                };
                var mensajeRespuesta = await estadoHabitacionService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Estado Habitacion Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarEstadoHabitacion(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }
}
