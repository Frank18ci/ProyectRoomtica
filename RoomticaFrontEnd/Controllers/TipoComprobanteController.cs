using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class TipoComprobanteController : Controller
    {
        private TipoComprobanteService.TipoComprobanteServiceClient? tipoComprobanteService;
        private GrpcChannel? chanal;
        public TipoComprobanteController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoComprobanteService = new TipoComprobanteService.TipoComprobanteServiceClient(chanal);
        }

        async Task<IEnumerable<TipoComprobanteModel>> listarTipoComprobante()
        {
            List<TipoComprobanteModel> tipoComprobanteModels = new List<TipoComprobanteModel>();
            var request = new Empty();
            var mensaje = await tipoComprobanteService.GetAllAsync(request);

            foreach (var item in mensaje.TipoComprobantes_)
            {
                tipoComprobanteModels.Add(new TipoComprobanteModel()
                {
                    Id = item.Id,
                    Tipo = item.Tipo
                });
            }
            return tipoComprobanteModels;
        }
        async Task<string> guardarTipoComprobante(TipoComprobanteModel tipoComprobante)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoComprobante()
                {
                    Id = tipoComprobante.Id,
                    Tipo = tipoComprobante.Tipo
                };
                var mensajeRespuesta = await tipoComprobanteService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Cliente Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<TipoComprobanteModel> buscarTipoComprobantePorId(int id)
        {
            TipoComprobanteModel tipoComprobante = null;
            try
            {
                var request = new TipoComprobanteId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoComprobanteService.GetByIdAsync(request);
                tipoComprobante = new TipoComprobanteModel()
                {
                    Id = mensajeRespuesta.Id,
                    Tipo = mensajeRespuesta.Tipo
                };

            }
            catch (Exception ex) { return null; }
            return tipoComprobante;
        }
        async Task<string> actualizarTipoComprobante(TipoComprobanteModel tipoComprobante)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoComprobante()
                {
                    Id = tipoComprobante.Id,
                    Tipo = tipoComprobante.Tipo
                };
                var mensajeRespuesta = await tipoComprobanteService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Comprobante Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<string> eliminarTipoComprobante(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoComprobanteId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoComprobanteService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Comprobante Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<TipoComprobanteModel> temporal = await listarTipoComprobante();

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
           
            return View(new TipoComprobanteModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(TipoComprobanteModel tipoComprobante)
        {
            ViewBag.mensaje = await guardarTipoComprobante(tipoComprobante);            
            return View(tipoComprobante);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            TipoComprobanteModel tipoComprobante = await buscarTipoComprobantePorId(id);            
            return View(tipoComprobante);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(TipoComprobanteModel tipoComprobante)
        {
            ViewBag.mensaje = await actualizarTipoComprobante(tipoComprobante);            
            return View(tipoComprobante);
        }
        public async Task<ActionResult> Details(int id = 0)
        {
            TipoComprobanteModel tipoComprobante = await buscarTipoComprobantePorId(id);
            return View(tipoComprobante);
        }
        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarTipoComprobante(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }

    }
}
