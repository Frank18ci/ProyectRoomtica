using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class ConsumoController : Controller
    {
        private ConsumoService.ConsumoServiceClient? consumoService;
        private ReservaService.ReservaServiceClient? reservaService;
        private ProductoService.ProductoServiceClient? productoService;
        private GrpcChannel? chanal;

        //CONTROLLER
        public ConsumoController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            consumoService = new ConsumoService.ConsumoServiceClient(chanal);
            reservaService = new ReservaService.ReservaServiceClient(chanal);
            productoService = new ProductoService.ProductoServiceClient(chanal);
        }

        //LISTAR
        async Task<IEnumerable<ReservaDTOModel>> listarReservaDTO()
        {
            List<ReservaDTOModel> reservaDTOModel = new List<ReservaDTOModel>();
            var request = new Empty();
            var mensaje = await reservaService.GetAllAsync(request);
            foreach (var item in mensaje.Reservas_)
            {
                reservaDTOModel.Add(new ReservaDTOModel()
                {
                    id = item.Id,
                    id_habitacion = item.IdHabitacion,
                    id_cliente = item.IdCliente,
                    id_trabajador = item.IdTrabajador,
                    id_tipo_reserva = item.IdTipoReserva,
                    fecha_ingreso = item.FechaIngreso.ToDateTime(),
                    fecha_salida = item.FechaSalida.ToDateTime(),
                    costo_alojamiento = item.CostoAlojamiento
                });
            }
            return reservaDTOModel;
        }

        async Task<IEnumerable<ProductoDTOModel>> listarProductoDTO()
        {
            List<ProductoDTOModel> productoDTOModel = new List<ProductoDTOModel>();
            var request = new Empty();
            var mensaje = await productoService.GetAllAsync(request);
            foreach (var item in mensaje.Productos_)
            {
                productoDTOModel.Add(new ProductoDTOModel()
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Unidad = item.UnidadMedidaProducto,
                    Categoria = item.CategoriaProducto,
                    Cantidad = item.Cantidad,
                    PrecioU = item.PrecioUnico
                });
            }
            return productoDTOModel;
        }

        async Task<IEnumerable<ConsumoDTOModel>> listarConsumo()
        {
            List<ConsumoDTOModel> consumoDTOModel = new List<ConsumoDTOModel>();
            var request = new Empty();
            var mensaje = await consumoService.GetAllAsync(request);
            foreach (var item in mensaje.Consumo)
            {
                consumoDTOModel.Add(new ConsumoDTOModel()
                {
                    id = item.Id,
                    reserva = item.IdReserva,
                    producto = item.IdProducto,
                    cantidad = item.Cantidad,
                    precio_venta = item.PrecioVenta
                });
            }
            return consumoDTOModel;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<ConsumoDTOModel> temporal = await listarConsumo();
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.reserva)
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
        async Task<string> guardarConsumo(ConsumoModel consumo)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Consumo()
                {
                    Id = consumo.id,
                    IdReserva = consumo.id_reserva,
                    IdProducto = consumo.id_producto,
                    Cantidad = consumo.cantidad,
                    PrecioVenta = consumo.precio_venta
                };
                var mensajeRespuesta = await consumoService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Consumo Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.reserva = new SelectList(await listarReservaDTO(), "id", "id_habitacion");
            ViewBag.producto = new SelectList(await listarProductoDTO(), "Id", "Nombre");
            return View(new ConsumoModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(ConsumoModel consumo)
        {
            ViewBag.reserva = new SelectList(await listarReservaDTO(), "id", "id_habitacion");
            ViewBag.producto = new SelectList(await listarProductoDTO(), "Id", "Nombre");
            ViewBag.mensaje = await guardarConsumo(consumo);
            return View(consumo);
        }

        //DETAIL
        public async Task<ActionResult> Details(int id = 0)
        {
            ConsumoDTOModel consumo = await buscarConsumoDTOPorId(id);
            return View(consumo);
        }

        //EDIT
        async Task<ConsumoDTOModel> buscarConsumoDTOPorId(int id)
        {
            ConsumoDTOModel consumo = null;
            try
            {
                var request = new ConsumoId()
                {
                    Id = id
                };
                var mensaje = await consumoService.GetByIdDTOAsync(request);
                consumo = new ConsumoDTOModel()
                {
                    id = mensaje.Id,
                    reserva = mensaje.IdReserva,
                    producto = mensaje.IdProducto,
                    cantidad = mensaje.Cantidad,
                    precio_venta = mensaje.PrecioVenta
                };
            }
            catch (Exception ex) { throw ex; }
            return consumo;
        }

        async Task<ConsumoModel> buscarConsumoPorId(int id)
        {
            ConsumoModel consumo = null;
            try
            {
                var request = new ConsumoId()
                {
                    Id = id
                };
                var mensaje = await consumoService.GetByIdAsync(request);
                consumo = new ConsumoModel()
                {
                    id = mensaje.Id,
                    id_reserva = mensaje.IdReserva,
                    id_producto = mensaje.IdProducto,
                    cantidad = mensaje.Cantidad,
                    precio_venta = mensaje.PrecioVenta
                };
            }
            catch (Exception ex) { throw ex; }
            return consumo;
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            ConsumoModel consumo = await buscarConsumoPorId(id);
            ViewBag.reserva = new SelectList(await listarReservaDTO(), "id", "id_habitacion");
            ViewBag.producto = new SelectList(await listarProductoDTO(), "Id", "Nombre");
            return View(consumo);
        }

        //-------------------------------
        async Task<string> actualizarConsumo(ConsumoModel consumo)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Consumo()
                {
                    Id = consumo.id,
                    IdReserva = consumo.id_reserva,
                    IdProducto = consumo.id_producto,
                    Cantidad = consumo.cantidad,
                    PrecioVenta = consumo.precio_venta
                };
                var mensajeRespuesta = await consumoService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Consumo Actualizado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ConsumoModel consumo)
        {
            ViewBag.mensaje = await actualizarConsumo(consumo);
            ViewBag.reserva = new SelectList(await listarReservaDTO(), "id", "id_habitacion");
            ViewBag.producto = new SelectList(await listarProductoDTO(), "Id", "Nombre");
            return View(consumo);
        }

        //DELETE
        async Task<string> eliminarConsumo(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new ConsumoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await consumoService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Consumo Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarConsumo(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }
}
