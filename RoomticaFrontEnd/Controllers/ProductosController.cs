using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class ProductosController : Controller
    {
        private ProductoService.ProductoServiceClient? ProductoService;
        private UnidadMedidaProductoService.UnidadMedidaProductoServiceClient? UnidadMedidaProductoService;
        private CategoriaProductoService.CategoriaProductoServiceClient? CategoriaProductoService;
        private GrpcChannel? chanal;

        //CONTROLLER
        public ProductosController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            ProductoService = new ProductoService.ProductoServiceClient(chanal);
            UnidadMedidaProductoService = new UnidadMedidaProductoService.UnidadMedidaProductoServiceClient(chanal);
            CategoriaProductoService = new CategoriaProductoService.CategoriaProductoServiceClient(chanal);
        }

        //LISTAR
        async Task<IEnumerable<UnidadMedidaProductoModel>> listarUnidadMedidaProducto()
        {
            List<UnidadMedidaProductoModel> unidadMedidaProductoModel = new List<UnidadMedidaProductoModel>();
            var request = new Empty();
            var mensaje = await UnidadMedidaProductoService.GetAllAsync(request);
            foreach (var item in mensaje.UnidadMedidaProductos_)
            {
                unidadMedidaProductoModel.Add(new UnidadMedidaProductoModel()
                {
                    Id = item.Id,
                    Unidad = item.Unidad
                });
            }
            return unidadMedidaProductoModel;
        }

        async Task<IEnumerable<CategoriaProductoModel>> listarCategoriaProducto()
        {
            List<CategoriaProductoModel> categoriaProductoModel = new List<CategoriaProductoModel>();
            var request = new Empty();
            var mensaje = await CategoriaProductoService.GetAllAsync(request);
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

        async Task<IEnumerable<ProductoDTOModel>> listarProducto()
        {
            List<ProductoDTOModel> productoModel = new List<ProductoDTOModel>();
            var request = new Empty();
            var mensaje = await ProductoService.GetAllAsync(request);
            foreach (var item in mensaje.Productos_)
            {
                productoModel.Add(new ProductoDTOModel()
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Unidad = item.UnidadMedidaProducto,
                    Categoria = item.CategoriaProducto,
                    Cantidad = item.Cantidad,
                    PrecioU = item.PrecioUnico
                });
            }
            return productoModel;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<ProductoDTOModel> temporal = await listarProducto();
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.Nombre)
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

        //CREATE
        async Task<string> guardarProducto(ProductoModel producto)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Producto()
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    IdUnidadMedidaProducto = producto.Unidad,
                    IdCategoriaProducto = producto.Categoria,
                    Cantidad = producto.Cantidad,
                    PrecioUnico = producto.PrecioU
                };
                var mensajeRespuesta = await ProductoService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Producto Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message;}
            return mensaje;
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.UnidadMedidaProducto = new SelectList(await listarUnidadMedidaProducto(), "Id", "Unidad");
            ViewBag.CategoriaProducto = new SelectList(await listarCategoriaProducto(), "Id", "Categoria");
            return View(new ProductoModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductoModel producto)
        {
            ViewBag.UnidadMedidaProducto = new SelectList(await listarUnidadMedidaProducto(), "Id", "Unidad");
            ViewBag.CategoriaProducto = new SelectList(await listarCategoriaProducto(), "Id", "Categoria");
            ViewBag.mensaje = await guardarProducto(producto);
            return View(producto);
        }

        //DETAIL
        public async Task<ActionResult> Details(int id = 0)
        {
            ProductoDTOModel producto = await buscaProductoDTOPorId(id);
            return View(producto);
        }

        //EDIT
        async Task<ProductoDTOModel> buscaProductoDTOPorId(int id)
        {
            ProductoDTOModel producto = null;
            try
            {
                var request = new ProductoId()
                {
                    Id = id
                };
                var mensaje = await ProductoService.GetByIdDTOAsync(request);
                producto = new ProductoDTOModel()
                {
                    Id = mensaje.Id,
                    Nombre = mensaje.Nombre,
                    Unidad = mensaje.UnidadMedidaProducto,
                    Categoria = mensaje.CategoriaProducto,
                    Cantidad = mensaje.Cantidad,
                    PrecioU = mensaje.PrecioUnico
                };
            }
            catch (Exception ex) { throw ex; }
            return producto;
        }

        async Task<ProductoModel> buscaProductoPorId(int id)
        {
            ProductoModel producto = null;
            try
            {
                var request = new ProductoId()
                {
                    Id = id
                };
                var mensaje = await ProductoService.GetByIdAsync(request);
                producto = new ProductoModel()
                {
                    Id = mensaje.Id,
                    Nombre = mensaje.Nombre,
                    Unidad = mensaje.IdUnidadMedidaProducto,
                    Categoria = mensaje.IdCategoriaProducto,
                    Cantidad = mensaje.Cantidad,
                    PrecioU = mensaje.PrecioUnico
                };
            }
            catch (Exception ex) { throw ex; }
            return producto;
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            ProductoModel producto = await buscaProductoPorId(id);
            ViewBag.UnidadMedidaProducto = new SelectList(await listarUnidadMedidaProducto(), "Id", "Unidad");
            ViewBag.CategoriaProducto = new SelectList(await listarCategoriaProducto(), "Id", "Categoria");
            return View(producto);
        }

        //-------------------------------
        async Task<string> actualizarProducto(ProductoModel producto)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Producto()
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    IdUnidadMedidaProducto = producto.Unidad,
                    IdCategoriaProducto = producto.Categoria,
                    Cantidad = producto.Cantidad,
                    PrecioUnico = producto.PrecioU
                };
                var mensajeRespuesta = await ProductoService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Producto Actualizado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ProductoModel producto)
        {
            ViewBag.mensaje = await actualizarProducto(producto);
            ViewBag.UnidadMedidaProducto = new SelectList(await listarUnidadMedidaProducto(), "Id", "Unidad");
            ViewBag.CategoriaProducto = new SelectList(await listarCategoriaProducto(), "Id", "Categoria");
            return View(producto);
        }

        //DELETE
        async Task<string> eliminarProducto(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new ProductoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await ProductoService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Producto Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarProducto(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }
}
