using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;


namespace RoomticaFrontEnd.Controllers
{
    public class TipoNacionalidadController : Controller
    {
        private TipoNacionalidadService.TipoNacionalidadServiceClient? tipoNacionalidadService;
        private GrpcChannel? chanal;
        public TipoNacionalidadController()
        {
            chanal = GrpcChannel.ForAddress("https://localhost:5001");
            tipoNacionalidadService = new TipoNacionalidadService.TipoNacionalidadServiceClient(chanal);
        }

        async Task<IEnumerable<TipoNacionalidadModel>> listarTipoNacionalidad()
        {
            List<TipoNacionalidadModel> tipoNacionalidadModel = new List<TipoNacionalidadModel>();
            var request = new Empty();
            var mensaje = await tipoNacionalidadService.GetAllAsync(request);

            foreach (var item in mensaje.TipoNacionalidades_)
            {
                tipoNacionalidadModel.Add(new TipoNacionalidadModel()
                {
                    Id = item.Id,
                    tipo = item.Tipo
                });
            }
            return tipoNacionalidadModel;
        }
        async Task<string> guardarTipoNacionalidad(TipoNacionalidadModel tipoNacionalidad)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoNacionalidad()
                {
                    Id = tipoNacionalidad.Id,
                    Tipo = tipoNacionalidad.tipo
                };
                var mensajeRespuesta = await tipoNacionalidadService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Nacionalidad Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<TipoNacionalidadModel> buscarTipoNacionalidadPorId(int id)
        {
            TipoNacionalidadModel tipoNacionalidad = null;
            try
            {
                var request = new TipoNacionalidadId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoNacionalidadService.GetByIdAsync(request);
                tipoNacionalidad = new TipoNacionalidadModel()
                {
                    Id = mensajeRespuesta.Id,
                    tipo = mensajeRespuesta.Tipo
                };

            }
            catch (Exception ex) { return null; }
            return tipoNacionalidad;
        }
        async Task<string> actualizarTipoNacionalidad(TipoNacionalidadModel tipoNacionalidad)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoNacionalidad()
                {
                    Id = tipoNacionalidad.Id,
                    
                };
                var mensajeRespuesta = await tipoNacionalidadService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Nacionalidad Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<string> eliminarTipoNacionalidad(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoNacionalidadId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoNacionalidadService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Nacionalidad Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<TipoNacionalidadModel> temporal = await listarTipoNacionalidad();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.tipo)
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
            return View(new TipoNacionalidadModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(TipoNacionalidadModel tipoNacionalidad)
        {
            ViewBag.mensaje = await guardarTipoNacionalidad(tipoNacionalidad);
           
            return View(tipoNacionalidad);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            TipoNacionalidadModel tipoNacionalidad = await buscarTipoNacionalidadPorId(id);
            
            return View(tipoNacionalidad);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(TipoNacionalidadModel tipoNacionalidad)
        {
            ViewBag.mensaje = await actualizarTipoNacionalidad(tipoNacionalidad);
            
            return View(tipoNacionalidad);
        }
        public async Task<ActionResult> Details(int id = 0)
        {
            TipoNacionalidadModel tipoNacionalidad = await buscarTipoNacionalidadPorId(id);
            return View(tipoNacionalidad);
        }
        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarTipoNacionalidad(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }
}
