using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class TipoEstacionamientoController : Controller
    {
        private TipoEstacionamientoService.TipoEstacionamientoServiceClient? tipoEstacionamientoService;
        private GrpcChannel? chanal;
        public TipoEstacionamientoController()
        {
            chanal = GrpcChannel.ForAddress("https://localhost:5001");
            tipoEstacionamientoService = new TipoEstacionamientoService.TipoEstacionamientoServiceClient(chanal);
        }

        async Task<IEnumerable<TipoEstacionamientoModel>> listarTipoEstacionamiento()
        {
            List<TipoEstacionamientoModel> tipoEstacionamientoModels = new List<TipoEstacionamientoModel>();
            var request = new Empty();
            var mensaje = await tipoEstacionamientoService.GetAllAsync(request);

            foreach (var item in mensaje.TipoEstacionamientos_)
            {
                tipoEstacionamientoModels.Add(new TipoEstacionamientoModel()
                {
                    Id = item.Id,
                    Tipo = item.Tipo,
                    Costo = item.Costo
                });
            }
            return tipoEstacionamientoModels;
        }
        async Task<string> guardarTipoEstacionamiento(TipoEstacionamientoModel tipoEstacionamiento)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoEstacionamiento()
                {
                    Id = tipoEstacionamiento.Id,
                    Tipo = tipoEstacionamiento.Tipo,
                    Costo = tipoEstacionamiento.Costo

                };
                var mensajeRespuesta = await tipoEstacionamientoService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Estacionamiento Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<TipoEstacionamientoModel> buscarTipoEstacionamientoPorId(int id)
        {
            TipoEstacionamientoModel tipoEstacionamiento = null;
            try
            {
                var request = new TipoEstacionamientoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoEstacionamientoService.GetByIdAsync(request);
                tipoEstacionamiento = new TipoEstacionamientoModel()
                {
                    Id = mensajeRespuesta.Id,
                    Tipo = mensajeRespuesta.Tipo,
                    Costo = mensajeRespuesta.Costo,
                   
                };

            }
            catch (Exception ex) { return null; }
            return tipoEstacionamiento;
        }
        async Task<string> actualizarTipoEstacionamiento(TipoEstacionamientoModel tipoEstacionamiento)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoEstacionamiento()
                {
                    Id = tipoEstacionamiento.Id,
                    Tipo = tipoEstacionamiento.Tipo,
                    Costo = tipoEstacionamiento.Costo

                };
                var mensajeRespuesta = await tipoEstacionamientoService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Estacionamiento Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<string> eliminarTipoEstacionamiento(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoEstacionamientoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoEstacionamientoService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Estacionamiento Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<TipoEstacionamientoModel> temporal = await listarTipoEstacionamiento();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.Tipo )
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
            
            return View(new TipoEstacionamientoModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(TipoEstacionamientoModel tipoEstacionamiento)
        {
            ViewBag.mensaje = await guardarTipoEstacionamiento(tipoEstacionamiento);
           
            return View(tipoEstacionamiento);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            TipoEstacionamientoModel tipoEstacionamiento = await buscarTipoEstacionamientoPorId(id);
            return View(tipoEstacionamiento);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(TipoEstacionamientoModel tipoEstacionamiento)
        {
            ViewBag.mensaje = await actualizarTipoEstacionamiento(tipoEstacionamiento);
           
            return View(tipoEstacionamiento);
        }
        public async Task<ActionResult> Details(int id = 0)
        {
            TipoEstacionamientoModel tipoEstacionamiento = await buscarTipoEstacionamientoPorId(id);
            return View(tipoEstacionamiento);
        }
        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarTipoEstacionamiento(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }
}
