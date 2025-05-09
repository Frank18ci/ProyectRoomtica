using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace RoomticaFrontEnd.Controllers
{
    public class TipoHabitacionController : Controller
    {
        private TipoHabitacionService.TipoHabitacionServiceClient? tipoHabitacionService;
        private GrpcChannel? chanal;
        public TipoHabitacionController()
        {
            chanal = GrpcChannel.ForAddress("https://localhost:5001");
            tipoHabitacionService = new TipoHabitacionService.TipoHabitacionServiceClient(chanal);
        }
        async Task<IEnumerable<TipoHabitacionModel>> listarTipoHabitacion()
        {
            List<TipoHabitacionModel> tipoHabitacionOModels = new List<TipoHabitacionModel>();
            var request = new Empty();
            var mensaje = await tipoHabitacionService.GetAllAsync(request);

            foreach (var item in mensaje.TipoHabitaciones_)
            {
                tipoHabitacionOModels.Add(new TipoHabitacionModel()
                {
                    Id = item.Id,
                    Tipo = item.Tipo,
                    descripccion = item.Descripccion
                                  
                });
            }
            return tipoHabitacionOModels;
        }
        async Task<string> guardarTipoHabitacion(TipoHabitacionModel tipoHabitacion)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoHabitacion()
                {
                    Id = tipoHabitacion.Id,
                    Tipo = tipoHabitacion.Tipo,
                    Descripccion = tipoHabitacion.descripccion                    
                };
                var mensajeRespuesta = await tipoHabitacionService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Habitacion Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<TipoHabitacionModel> buscarTipoHabitacionPorId(int id)
        {
            TipoHabitacionModel tipoHabitacion = null;
            try
            {
                var request = new TipoHabitacionId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoHabitacionService.GetByIdAsync(request);
                tipoHabitacion = new TipoHabitacionModel()
                {
                    Id = mensajeRespuesta.Id,
                    Tipo = mensajeRespuesta.Tipo,
                    descripccion = mensajeRespuesta.Descripccion
                   
                };
            }
            catch (Exception ex) { return null; }
            return tipoHabitacion;
        }
        async Task<string> actualizarTipoHabitacion(TipoHabitacionModel tipoHabitacion)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoHabitacion()
                {
                    Id = tipoHabitacion.Id,
                    Tipo = tipoHabitacion.Tipo,
                    Descripccion = tipoHabitacion.descripccion
                   
                };
                var mensajeRespuesta = await tipoHabitacionService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Habitacion Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<string> eliminarTipoHabitacion(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoHabitacionId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoHabitacionService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Habitacion Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<TipoHabitacionModel> temporal = await listarTipoHabitacion();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.Tipo)
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
        public async Task<ActionResult> Create()
        {            
            return View(new TipoHabitacionModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(TipoHabitacionModel tipoHabitacion)
        {
            ViewBag.mensaje = await guardarTipoHabitacion(tipoHabitacion);          
            return View(tipoHabitacion);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            TipoHabitacionModel tipoHabitacion = await buscarTipoHabitacionPorId(id);            
            return View(tipoHabitacion);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(TipoHabitacionModel tipoHabitacion)
        {
            ViewBag.mensaje = await actualizarTipoHabitacion(tipoHabitacion);           
            return View(tipoHabitacion);
        }
        public async Task<ActionResult> Details(int id = 0)
        {
            TipoHabitacionModel tipoHabitacion = await buscarTipoHabitacionPorId(id);
            return View(tipoHabitacion);
        }
        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarTipoHabitacion(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }

    }
}
