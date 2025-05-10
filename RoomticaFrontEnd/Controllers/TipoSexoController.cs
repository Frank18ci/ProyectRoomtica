using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;


namespace RoomticaFrontEnd.Controllers
{
    public class TipoSexoController : Controller
    {
        private TipoSexoService.TipoSexoServiceClient? tipoSexoService;
        private GrpcChannel? chanal;

        public TipoSexoController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoSexoService = new TipoSexoService.TipoSexoServiceClient(chanal);
        }
        async Task<IEnumerable<TipoSexoModel>> listarTipoSexo()
        {
            List<TipoSexoModel> tipoSexo = new List<TipoSexoModel>();
            var request = new Empty();
            var mensaje = await tipoSexoService.GetAllAsync(request);

            foreach (var item in mensaje.TipoSexos_)
            {
                tipoSexo.Add(new TipoSexoModel()
                {
                    Id = item.Id,
                    tipo = item.Tipo

                });
            }
            return tipoSexo;
        }
        async Task<string> guardarTipoSexo(TipoSexoModel tipoSexo)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoSexo()
                {
                    Id = tipoSexo.Id,
                    Tipo = tipoSexo.tipo
                };
                var mensajeRespuesta = await tipoSexoService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Sexo Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<TipoSexoModel> buscarSexoPorId(int id)
        {
            TipoSexoModel tipoSexo = null;
            try
            {
                var request = new TipoSexoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoSexoService.GetByIdAsync(request);
                tipoSexo = new TipoSexoModel()
                {
                    Id = mensajeRespuesta.Id,
                    tipo = mensajeRespuesta.Tipo
                };
            }
            catch (Exception ex) { return null; }
            return tipoSexo;
        }
        async Task<string> actualizarSexo(TipoSexoModel tipoSexo)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoSexo()
                {
                    Id = tipoSexo.Id,
                    Tipo = tipoSexo.tipo
                };
                var mensajeRespuesta = await tipoSexoService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Sexo Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<string> eliminarSexo(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoSexoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoSexoService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Sexo Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<TipoSexoModel> temporal = await listarTipoSexo();

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
            return View(new TipoSexoModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(TipoSexoModel s)
        {
            ViewBag.mensaje = await guardarTipoSexo(s);           
            return View(s);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            TipoSexoModel s = await buscarSexoPorId(id);
            return View(s);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(TipoSexoModel s)
        {
            ViewBag.mensaje = await actualizarSexo(s);            
            return View(s);
        }

        public async Task<ActionResult> Details(int id = 0)
        {
            TipoSexoModel s = await buscarSexoPorId(id);
            return View(s);
        }
        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarSexo(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }

    }
}
