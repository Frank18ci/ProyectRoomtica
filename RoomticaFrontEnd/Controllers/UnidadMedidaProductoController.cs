using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using System.Threading.Channels;

namespace RoomticaFrontEnd.Controllers
{
    public class UnidadMedidaProductoController : Controller
    {
        private UnidadMedidaProductoService.UnidadMedidaProductoServiceClient? UnidadMedidaProductoService;
        private GrpcChannel? chanal;
        public UnidadMedidaProductoController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            UnidadMedidaProductoService = new UnidadMedidaProductoService.UnidadMedidaProductoServiceClient(chanal);
        }

        async Task<IEnumerable<UnidadMedidaProductoModel>> listarUnidadMedidaProducto()
        {
            List<UnidadMedidaProductoModel> UnidadMedidaProductoModel = new List<UnidadMedidaProductoModel>();
            var request = new Empty();
            var mensaje = await UnidadMedidaProductoService.GetAllAsync(request);

            foreach (var item in mensaje.UnidadMedidaProductos_)
            {
                UnidadMedidaProductoModel.Add(new UnidadMedidaProductoModel()
                {
                    Id = item.Id,
                    Unidad = item.Unidad
                    
                });
            }
            return UnidadMedidaProductoModel;
        }
        async Task<string> guardarUnidadMedidaProducto(UnidadMedidaProductoModel rpt)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new UnidadMedidaProducto()
                {
                    Id = rpt.Id,
                    Unidad = rpt.Unidad
                };
                var mensajeRespuesta = await UnidadMedidaProductoService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Unidad Medida Producto Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<UnidadMedidaProductoModel> buscarUnidadMedidaProductoPorId(int id)
        {
            UnidadMedidaProductoModel ump = null;
            try
            {
                var request = new UnidadMedidaProductoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await UnidadMedidaProductoService.GetByIdAsync(request);
                ump = new UnidadMedidaProductoModel()
                {
                    Id = mensajeRespuesta.Id,
                    Unidad = mensajeRespuesta.Unidad,
                    
                };
            }
            catch (Exception ex) { return null; }
            return ump;
        }
        async Task<string> actualizarUnidadMedidaProducto(UnidadMedidaProductoModel ump)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new UnidadMedidaProducto()
                {
                    Id = ump.Id,
                    Unidad = ump.Unidad

                };
                var mensajeRespuesta = await UnidadMedidaProductoService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Unidad Medida Producto Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<string> eliminarUnidadMedidaProducto(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new UnidadMedidaProductoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await UnidadMedidaProductoService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Unidad Medida ProductoService Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<UnidadMedidaProductoModel> temporal = await listarUnidadMedidaProducto();
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.Id + " " + c.Unidad)
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
            return View(new UnidadMedidaProductoModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(UnidadMedidaProductoModel ump)
        {
            ViewBag.mensaje = await guardarUnidadMedidaProducto(ump);
            return View(ump);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            UnidadMedidaProductoModel ump = await buscarUnidadMedidaProductoPorId(id);
            return View(ump);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(UnidadMedidaProductoModel ump)
        {
            ViewBag.mensaje = await actualizarUnidadMedidaProducto(ump);
            return View(ump);
        }
        public async Task<ActionResult> Details(int id = 0)
        {
            UnidadMedidaProductoModel ump = await buscarUnidadMedidaProductoPorId(id);
            return View(ump);
        }
        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarUnidadMedidaProducto(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }

}
