using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaFrontEnd.Permisos;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class CategoriaProductoController : Controller
    {
        private CategoriaProductoService.CategoriaProductoServiceClient? categoriaProductoService;
        private GrpcChannel? chanal;

        //CONTROLLER
        public CategoriaProductoController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            categoriaProductoService = new CategoriaProductoService.CategoriaProductoServiceClient(chanal);
        }

        //LISTAR
        async Task<IEnumerable<CategoriaProductoModel>> listarCategoriaProducto()
        {
            List<CategoriaProductoModel> categoriaProductoModel = new List<CategoriaProductoModel>();
            var request = new Empty();
            var mensaje = await categoriaProductoService.GetAllAsync(request);

            foreach (var item in mensaje.CategoriaProductos_)
            {
                categoriaProductoModel.Add(new CategoriaProductoModel()
                {
                    Id = item.Id,
                    Categoria = item.Categoria
                });
            }
            return categoriaProductoModel;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<CategoriaProductoModel> temporal = await listarCategoriaProducto();
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.Categoria)
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
        async Task<string> guardarCategoriaProducto(CategoriaProductoModel categoriaProducto)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new CategoriaProducto()
                {
                    Id = categoriaProducto.Id,
                    Categoria = categoriaProducto.Categoria
                };
                var mensajeRespuesta = await categoriaProductoService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Caracteristica Producto Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        [ValidarSesion]
        public async Task<ActionResult> Create()
        {
            return View(new CategoriaProductoModel());
        }
        [ValidarSesion]
        [HttpPost]
        public async Task<ActionResult> Create(CategoriaProductoModel categoriaProducto)
        {
            ViewBag.mensaje = await guardarCategoriaProducto(categoriaProducto);
            return View(categoriaProducto);
        }

        //DETAIL
        [ValidarSesion]
        public async Task<ActionResult> Details(int id = 0)
        {
            CategoriaProductoModel categoriaProducto = await buscarCategoriaProductoPorId(id);
            return View(categoriaProducto);
        }

        //EDIT
        async Task<CategoriaProductoModel> buscarCategoriaProductoPorId(int id)
        {
            CategoriaProductoModel categoriaProducto = null;
            try
            {
                var request = new CategoriaProductoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await categoriaProductoService.GetByIdAsync(request);
                categoriaProducto = new CategoriaProductoModel()
                {
                    Id = mensajeRespuesta.Id,
                    Categoria = mensajeRespuesta.Categoria
                };
            }
            catch (Exception ex) { return null; }
            return categoriaProducto;
        }
        [ValidarSesion]
        public async Task<ActionResult> Edit(int id = 0)
        {
            CategoriaProductoModel categoriaProducto = await buscarCategoriaProductoPorId(id);
            return View(categoriaProducto);
        }


        async Task<string> actualizarCategoriaProducto(CategoriaProductoModel categoriaProducto)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new CategoriaProducto()
                {
                    Id = categoriaProducto.Id,
                    Categoria = categoriaProducto.Categoria
                };
                var mensajeRespuesta = await categoriaProductoService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Caracteristica Producto Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        [ValidarSesion]
        [HttpPost]
        public async Task<ActionResult> Edit(CategoriaProductoModel categoriaProducto)
        {
            ViewBag.mensaje = await actualizarCategoriaProducto(categoriaProducto);
            return View(categoriaProducto);
        }

        
        async Task<string> eliminarCategoriaProducto(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new CategoriaProductoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await categoriaProductoService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Caracteristica Producto Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarCategoriaProducto(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }
}
