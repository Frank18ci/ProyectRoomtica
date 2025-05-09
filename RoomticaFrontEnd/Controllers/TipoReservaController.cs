using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class TipoReservaController : Controller
    {
        private TipoReservaService.TipoReservaServiceClient? tipoReservaService;
        private GrpcChannel? chanal;
        public TipoReservaController()
        {
            chanal = GrpcChannel.ForAddress("https://localhost:5001");
            tipoReservaService = new TipoReservaService.TipoReservaServiceClient(chanal);
        }
        async Task<IEnumerable<TipoReservaModel>> listarTipoReserva()
        {
            List<TipoReservaModel> tipoReservaModel = new List<TipoReservaModel>();
            var request = new Empty();
            var mensaje = await tipoReservaService.GetAllAsync(request);

            foreach (var item in mensaje.TipoReservas_)
            {
                tipoReservaModel.Add(new TipoReservaModel()
                {
                    id = item.Id,
                    tipo = item.Tipo
                   
                });
            }
            return tipoReservaModel;
        }
        async Task<string> guardarTipoReserva(TipoReservaModel tipoReserva)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoReserva()
                {
                    Id = tipoReserva.id,
                    Tipo = tipoReserva.tipo,
                    
                };
                var mensajeRespuesta = await tipoReservaService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Cliente Tipo Reserva";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<TipoReservaModel> buscarTipoReservaPorId(int id)
        {
            TipoReservaModel tipoReserva = null;
            try
            {
                var request = new TipoReservaId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoReservaService.GetByIdAsync(request);
                tipoReserva = new TipoReservaModel()
                {
                    id = mensajeRespuesta.Id,
                    tipo = mensajeRespuesta.Tipo
                   
                };

            }
            catch (Exception ex) { return null; }
            return tipoReserva;
        }
        async Task<string> actualizarTipoReserva(TipoReservaModel tipoReserva)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoReserva()
                {
                    Id = tipoReserva.id,
                    Tipo = tipoReserva.tipo,
                };
                var mensajeRespuesta = await tipoReservaService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Reserva Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<string> eliminarTipoReserva(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TipoReservaId()
                {
                    Id = id
                };
                var mensajeRespuesta = await tipoReservaService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Tipo Reserva Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<TipoReservaModel> temporal = await listarTipoReserva();

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
            return View(new TipoReservaModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(TipoReservaModel tipoReserva)
        {
            ViewBag.mensaje = await guardarTipoReserva(tipoReserva);           
            return View(tipoReserva);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            TipoReservaModel tipoReserva = await buscarTipoReservaPorId(id);           
            return View(tipoReserva);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(TipoReservaModel tipoReserva)
        {
            ViewBag.mensaje = await actualizarTipoReserva(tipoReserva);
           
            return View(tipoReserva);
        }
        public async Task<ActionResult> Details(int id = 0)
        {
            TipoReservaModel tipoReserva = await buscarTipoReservaPorId(id);
            return View(tipoReserva);
        }
        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarTipoReserva(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }

    }
}
