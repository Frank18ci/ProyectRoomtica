using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using static RoomticaGrpcServiceBackEnd.CategoriaProductoService;
using static RoomticaGrpcServiceBackEnd.TipoDocumentoService;

namespace RoomticaFrontEnd.Controllers
{
    public class TipoDocumentoController : Controller
    {
        private TipoDocumentoService.TipoDocumentoServiceClient? tipoDocumentoService;
        private GrpcChannel? chanal;
        public TipoDocumentoController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoDocumentoService = new TipoDocumentoService.TipoDocumentoServiceClient(chanal);
        }
        async Task<IEnumerable<TipoDocumentoModel>> listarTipoDocumento()
        {
            List<TipoDocumentoModel> tipoDocumentoModel = new List<TipoDocumentoModel>();
            var request = new Empty();
            var mensaje = await tipoDocumentoService.GetAllAsync(request);

            foreach (var item in mensaje.TipoDocumentos_)
            {
                tipoDocumentoModel.Add(new TipoDocumentoModel()
                {
                    Id = item.Id,
                    Tipo = item.Tipo,
                   
                });
            }
            return tipoDocumentoModel;
        }
        async Task<string> guardarTipoDocumento(TipoDocumentoModel tipoDocumento)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoDocumento()
                {
                    Id = tipoDocumento.Id,
                    Tipo = tipoDocumento.Tipo
                   
                };
                var mensajeRespuesta = await tipoDocumentoService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Documento Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<TipoDocumentoModel> buscarTipoDocumentoPorId(int id)
        {
            TipoDocumentoModel tipoDocumento = null;
            try
            {
                var request = new TipoDocumentoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoDocumentoService.GetByIdAsync(request);
                tipoDocumento = new TipoDocumentoModel()
                {
                    Id = mensajeRespuesta.Id,
                    Tipo = mensajeRespuesta.Tipo,
                    
                };
            }
            catch (Exception ex) { return null; }
            return tipoDocumento;
        }
        async Task<string> actualizarTipoDocumento(TipoDocumentoModel tipoDocumento)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoDocumento()
                {
                    Id = tipoDocumento.Id,
                    Tipo = tipoDocumento.Tipo,                    
                };
                var mensajeRespuesta = await tipoDocumentoService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Documento Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<string> eliminarTipoDocumento(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoDocumentoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoDocumentoService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Documento Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<TipoDocumentoModel> temporal = await listarTipoDocumento();

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
            
            return View(new TipoDocumentoModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(TipoDocumentoModel tipoDocumento)
        {
            ViewBag.mensaje = await guardarTipoDocumento(tipoDocumento);            
            return View(tipoDocumento);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            TipoDocumentoModel tipoDocumento = await buscarTipoDocumentoPorId(id);           
            return View(tipoDocumento);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(TipoDocumentoModel tipoDocumento)
        {
            ViewBag.mensaje = await actualizarTipoDocumento(tipoDocumento);           
            return View(tipoDocumento);
        }
        public async Task<ActionResult> Details(int id = 0)
        {
            TipoDocumentoModel tipoDocumento = await buscarTipoDocumentoPorId(id);
            return View(tipoDocumento);
        }
        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarTipoDocumento(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }
}
