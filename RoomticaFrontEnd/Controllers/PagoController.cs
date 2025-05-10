using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class PagoController : Controller
    {
        private PagoService.PagoServiceClient? pagoService;
        private ReservaService.ReservaServiceClient? reservaService;
        private TipoComprobanteService.TipoComprobanteServiceClient? tipoComprobanteService;
        private GrpcChannel? chanal;

        //CONTROLLER
        public PagoController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            pagoService = new PagoService.PagoServiceClient(chanal);
            reservaService = new ReservaService.ReservaServiceClient(chanal);
            tipoComprobanteService = new TipoComprobanteService.TipoComprobanteServiceClient(chanal);
        }

        //LISTAR
        async Task<IEnumerable<ReservaDTOModel>> listarReserva()
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
                    id_trabajador = item.IdTrabajador,
                    id_tipo_reserva = item.IdTipoReserva,
                    fecha_ingreso = item.FechaIngreso?.ToDateTime(),
                    fecha_salida = item.FechaSalida?.ToDateTime(),
                    costo_alojamiento = item.CostoAlojamiento
                });
            }
            return reservaDTOModel;
        }

        async Task<IEnumerable<TipoComprobanteModel>> listarTipoComprobante()
        {
            List<TipoComprobanteModel> temporal = new List<TipoComprobanteModel>();
            var request = new Empty();
            var mensaje = await tipoComprobanteService.GetAllAsync(request);
            foreach (var item in mensaje.TipoComprobantes_)
            {
                temporal.Add(new TipoComprobanteModel()
                {
                    Id = item.Id,
                    Tipo = item.Tipo
                });
            }
            return temporal;
        }
        
        async Task<IEnumerable<PagoDTOModel>> listarPago()
        {
            List<PagoDTOModel> pagoDTOModel = new List<PagoDTOModel>();
            var request = new Empty();
            var mensaje = await pagoService.GetAllAsync(request);
            foreach (var item in mensaje.Pagos_)
            {
                pagoDTOModel.Add(new PagoDTOModel()
                {
                    id = item.Id,
                    id_reserva = item.IdReserva,
                    id_tipo_comprobante = item.IdTipoComprobante,
                    igv = item.Igv,
                    total_pago = item.TotalPago,
                    fecha_emision = item.FechaEmision.ToDateTime(),
                    fecha_pago = item.FechaPago.ToDateTime()
                });
            }
            return pagoDTOModel;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<PagoDTOModel> temporal = await listarPago();
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.id_reserva)
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
        async Task<string> guardarPago(PagoModel pago)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Pago()
                {
                    Id = pago.id,
                    IdReserva = pago.id_reserva,
                    IdTipoComprobante = pago.id_tipo_comprobante,
                    Igv = pago.igv,
                    TotalPago = pago.total_pago,
                    FechaEmision = pago.fecha_emision.HasValue ? Timestamp.FromDateTime(pago.fecha_emision.Value.ToUniversalTime()) : null,
                    FechaPago = pago.fecha_pago.HasValue ? Timestamp.FromDateTime(pago.fecha_pago.Value.ToUniversalTime()) : null,

                };
                var mensajeRespuesta = await pagoService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Pago Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.reservaService = new SelectList(await listarReserva(), "id", "id_habitacion");
            ViewBag.tipoComprobanteService = new SelectList(await listarTipoComprobante(), "Id", "Tipo");
            return View(new PagoModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(PagoModel pago)
        {
            ViewBag.reservaService = new SelectList(await listarReserva(), "id", "id_habitacion", pago.id_reserva);
            ViewBag.tipoComprobanteService = new SelectList(await listarTipoComprobante(), "Id", "Tipo", pago.id_tipo_comprobante);
            ViewBag.mensaje = await guardarPago(pago);
            return View(pago);
        }

        //DETAIL
        public async Task<ActionResult> Details(int id = 0)
        {
            PagoDTOModel pago = await buscarPagoDTOPorId(id);
            return View(pago);
        }

        //EDIT
        async Task<PagoDTOModel> buscarPagoDTOPorId(int id)
        {
            PagoDTOModel pago = null;
            try
            {
                var request = new PagoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await pagoService.GetByIdDTOAsync(request);
                pago = new PagoDTOModel()
                {
                    id = mensajeRespuesta.Id,
                    id_reserva = mensajeRespuesta.IdReserva,
                    id_tipo_comprobante = mensajeRespuesta.IdTipoComprobante,
                    igv = mensajeRespuesta.Igv,
                    total_pago = mensajeRespuesta.TotalPago,
                    fecha_emision = mensajeRespuesta.FechaEmision.ToDateTime(),
                    fecha_pago = mensajeRespuesta.FechaPago.ToDateTime()
                };
            }
            catch (Exception ex) { throw ex; }
            return pago;
        }

        async Task<PagoModel> buscarPagoPorId(int id)
        {
            PagoModel pago = null;
            try
            {
                var request = new PagoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await pagoService.GetByIdAsync(request);
                pago = new PagoModel()
                {
                    id = mensajeRespuesta.Id,
                    id_reserva = mensajeRespuesta.IdReserva,
                    id_tipo_comprobante = mensajeRespuesta.IdTipoComprobante,
                    igv = mensajeRespuesta.Igv,
                    total_pago = mensajeRespuesta.TotalPago,
                    fecha_emision = mensajeRespuesta.FechaEmision.ToDateTime(),
                    fecha_pago = mensajeRespuesta.FechaPago.ToDateTime()
                };
            }
            catch (Exception ex) { throw ex; }
            return pago;
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            PagoModel pago = await buscarPagoPorId(id);
            ViewBag.reservaService = new SelectList(await listarReserva(), "id", "id_habitacion", pago.id_reserva);
            ViewBag.tipoComprobanteService = new SelectList(await listarTipoComprobante(), "Id", "Tipo", pago.id_tipo_comprobante);
            return View(pago);
        }

        //-------------------------------
        async Task<string> actualizarPago(PagoModel pago)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Pago()
                {
                    Id = pago.id,
                    IdReserva = pago.id_reserva,
                    IdTipoComprobante = pago.id_tipo_comprobante,
                    Igv = pago.igv,
                    TotalPago = pago.total_pago,
                    FechaEmision = pago.fecha_emision.HasValue ? Timestamp.FromDateTime(pago.fecha_emision.Value.ToUniversalTime()) : null,
                    FechaPago = pago.fecha_pago.HasValue ? Timestamp.FromDateTime(pago.fecha_pago.Value.ToUniversalTime()) : null,
                };
                var mensajeRespuesta = await pagoService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Pago Actualizado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        [HttpPost]
        public async Task<ActionResult> Edit(PagoModel pago)
        {
            ViewBag.mensaje = await actualizarPago(pago);
            ViewBag.reservaService = new SelectList(await listarReserva(), "id", "id_habitacion", pago.id_reserva);
            ViewBag.tipoComprobanteService = new SelectList(await listarTipoComprobante(), "Id", "Tipo", pago.id_tipo_comprobante);
            return View(pago);
        }

        //DELETE
        async Task<string> eliminarPago(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new PagoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await pagoService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Pago Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarPago(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }
}
