using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class HabitacionController : Controller
    {
        private HabitacionServices.HabitacionServicesClient? habitacionService;
        private TipoHabitacionService.TipoHabitacionServiceClient? tipoHabitacionService;
        private EstadoHabitacionServices.EstadoHabitacionServicesClient? estadoHabitacionService;
        private GrpcChannel? chanal;

        //CONTROLLER
        public HabitacionController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            habitacionService = new HabitacionServices.HabitacionServicesClient(chanal);
            estadoHabitacionService = new EstadoHabitacionServices.EstadoHabitacionServicesClient(chanal);
        }

        //LISTAR

        async Task<IEnumerable<EstadoHabitacionModel>> listarEstadoHabitacion()
        {
            List<EstadoHabitacionModel> temporal = new List<EstadoHabitacionModel>();
            var request = new Empty();
            var mensaje = await estadoHabitacionService.GetAllAsync(request);
            foreach (var item in mensaje.EstadoHabitaciones_)
            {
                temporal.Add(new EstadoHabitacionModel()
                {
                    id = item.Id,
                    estado_habitacion = item.EstadoHabitacion_
                });
            }
            return temporal;
        }

        async Task<IEnumerable<TipoHabitacionModel>> listarTipoHabitacion()
        {
            List<TipoHabitacionModel> temporal = new List<TipoHabitacionModel>();
            var request = new Empty();
            var mensaje = await tipoHabitacionService.GetAllAsync(request);
            foreach (var item in mensaje.TipoHabitaciones_)
            {
                temporal.Add(new TipoHabitacionModel()
                {
                    Id = item.Id,
                    Tipo = item.Tipo,
                    descripccion = item.Descripccion
                });
            }
            return temporal;
        }

        async Task<IEnumerable<HabitacionDTOModel>> listarHabitacion()
        {
            List<HabitacionDTOModel> habitacionModel = new List<HabitacionDTOModel>();
            var request = new Empty();
            var mensaje = await habitacionService.GetAllAsync(request);
            foreach (var item in mensaje.Habitaciones_)
            {
                habitacionModel.Add(new HabitacionDTOModel()
                {
                    id = item.Id,
                    numero = item.Numero,
                    piso = item.Piso,
                    precio_diario = item.PrecioDiario,
                    id_tipo = item.IdTipo,
                    id_estado = item.IdEstado
                });
            }
            return habitacionModel;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<HabitacionDTOModel> temporal = await listarHabitacion();
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.numero)
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

        //CREAR
        async Task<string> guardarHabitacion(HabitacionModel habitacion)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Habitacion()
                {
                    Id = habitacion.id,
                    Numero = habitacion.numero,
                    Piso = habitacion.piso,
                    PrecioDiario = habitacion.precio_diario,
                    IdTipo = habitacion.id_tipo,
                    IdEstado = habitacion.id_estado
                };
                var mensajeRespuesta = await habitacionService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Habitacion Agregada";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        public async Task<ActionResult> Create()
        {
            ViewBag.estado_habitacion = await listarEstadoHabitacion();
            ViewBag.tipo_habitacion = await listarTipoHabitacion();
            return View(new HabitacionModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(HabitacionModel habitacion)
        {
            ViewBag.estado_habitacion = await listarEstadoHabitacion();
            ViewBag.tipo_habitacion = await listarTipoHabitacion();
            ViewBag.mensaje = await guardarHabitacion(habitacion);
            return View(habitacion);
        }

        //DETAIL
        public async Task<ActionResult> Details(int id = 0)
        {
            HabitacionDTOModel habitacion = await buscarHabitacionDTOPorId(id);
            return View(habitacion);
        }

        //EDIT
        async Task<HabitacionDTOModel> buscarHabitacionDTOPorId(int id)
        {
            HabitacionDTOModel habitacion = null;
            try
            {
                var request = new HabitacionId() 
                { 
                    Id = id 
                };
                var mensajeRespuesta = await habitacionService.GetByIdDTOAsync(request);
                habitacion = new HabitacionDTOModel()
                {
                    id = mensajeRespuesta.Id,
                    numero = mensajeRespuesta.Numero,
                    piso = mensajeRespuesta.Piso,
                    precio_diario = mensajeRespuesta.PrecioDiario,
                    id_tipo = mensajeRespuesta.IdTipo,
                    id_estado = mensajeRespuesta.IdEstado
                };
            }
            catch (Exception ex) { throw ex; }
            return habitacion;
        }

        async Task<HabitacionModel> buscarHabitacionPorId(int id)
        {
            HabitacionModel habitacion = null;
            try
            {
                var request = new HabitacionId()
                {
                    Id = id
                };
                var mensajeRespuesta = await habitacionService.GetByIdAsync(request);
                habitacion = new HabitacionModel()
                {
                    id = mensajeRespuesta.Id,
                    numero = mensajeRespuesta.Numero,
                    piso = mensajeRespuesta.Piso,
                    precio_diario = mensajeRespuesta.PrecioDiario,
                    id_tipo = mensajeRespuesta.IdTipo,
                    id_estado = mensajeRespuesta.IdEstado
                };
            }
            catch (Exception ex) { throw ex; }
            return habitacion;
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            HabitacionModel habitacion = await buscarHabitacionPorId(id);
            ViewBag.estado_habitacion = new SelectList(await listarEstadoHabitacion(), "id", "estado_habitacion");
            ViewBag.tipo_habitacion = new SelectList(await listarTipoHabitacion(), "Id", "Tipo");
            return View(habitacion);
        }

        //-------------------------------

        async Task<string> actualizarHabitacion(HabitacionModel habitacion)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Habitacion()
                {
                    Id = habitacion.id,
                    Numero = habitacion.numero,
                    Piso = habitacion.piso,
                    PrecioDiario = habitacion.precio_diario,
                    IdTipo = habitacion.id_tipo,
                    IdEstado = habitacion.id_estado
                };
                var mensajeRespuesta = await habitacionService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Habitacion Actualizada";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        [HttpPost]
        public async Task<ActionResult> Edit(HabitacionModel habitacion)
        {
            ViewBag.mensaje = await actualizarHabitacion(habitacion);
            ViewBag.estado_habitacion = new SelectList(await listarEstadoHabitacion(), "id", "estado_habitacion");
            ViewBag.tipo_habitacion = new SelectList(await listarTipoHabitacion(), "Id", "Tipo");
            return View(habitacion);
        }

        //DELETE
        async Task<string> eliminarHabitacion(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new HabitacionId()
                {
                    Id = id
                };
                var mensajeRespuesta = await habitacionService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Habitacion Eliminada";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarHabitacion(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }
}
