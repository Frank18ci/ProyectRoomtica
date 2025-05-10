using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class EstacionamientoController : Controller
    {
        private EstacionamientoService.EstacionamientoServiceClient? estacionamientoService;
        private TipoEstacionamientoService.TipoEstacionamientoServiceClient? tipoEstacionamientoService;
        private GrpcChannel? chanal;

        //CONTROLLER
        public EstacionamientoController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            estacionamientoService = new EstacionamientoService.EstacionamientoServiceClient(chanal);
            tipoEstacionamientoService = new TipoEstacionamientoService.TipoEstacionamientoServiceClient(chanal);
        }

        //LISTAR
        async Task<IEnumerable<TipoEstacionamientoModel>> listarTipoEstacionamiento()
        {
            List<TipoEstacionamientoModel> temporal = new List<TipoEstacionamientoModel>();
            var request = new Empty();
            var mensaje = await tipoEstacionamientoService.GetAllAsync(request);
            foreach (var item in mensaje.TipoEstacionamientos_)
            {
                temporal.Add(new TipoEstacionamientoModel()
                {
                    Id = item.Id,
                    Tipo = item.Tipo,
                    Costo = item.Costo
                });
            }
            return temporal;
        }

        async Task<IEnumerable<EstacionamientoDTOModel>> listarEstacionamiento()
        {
            List<EstacionamientoDTOModel> estacionamientoDTOModel = new List<EstacionamientoDTOModel>();
            var request = new Empty();
            var mensaje = await estacionamientoService.GetAllAsync(request);
            foreach (var item in mensaje.Estacionamientos_)
            {
                estacionamientoDTOModel.Add(new EstacionamientoDTOModel()
                {
                    id = item.Id,
                    lugar = item.Lugar,
                    largo = item.Largo,
                    alto = item.Alto,
                    ancho = item.Ancho,
                    tipo_estacionamiento = item.IdTipoEstacionamiento
                });
            }
            return estacionamientoDTOModel;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<EstacionamientoDTOModel> temporal = await listarEstacionamiento();
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.lugar)
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
        async Task<string> guardarEstacionamiento(EstacionamientoModel estacionamiento)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Estacionamiento()
                {
                    Id = estacionamiento.id,
                    Lugar = estacionamiento.lugar,
                    Largo = estacionamiento.largo,
                    Alto = estacionamiento.alto,
                    Ancho = estacionamiento.ancho,
                    IdTipoEstacionamiento = estacionamiento.id_tipo_estacionamiento
                };
                var mensajeRespuesta = await estacionamientoService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Estacionamiento Agregado";
            }
            catch (Exception ex){mensaje = ex.Message;}
            return mensaje;
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.tipoEstacionamiento = new SelectList(await listarTipoEstacionamiento(), "id", "tipo");
            return View(new EstacionamientoModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(EstacionamientoModel estacionamiento)
        {
            ViewBag.tipoEstacionamiento = new SelectList(await listarTipoEstacionamiento(), "id", "tipo");
            ViewBag.mensaje = await guardarEstacionamiento(estacionamiento);
            return View(estacionamiento);
        }

        //DETAIL
        public async Task<ActionResult> Details(int id = 0)
        {
            EstacionamientoDTOModel estacionamiento = await buscarEstacionamientoDTOPorId(id);
            return View(estacionamiento);
        }

        //EDIT
        async Task<EstacionamientoDTOModel> buscarEstacionamientoDTOPorId(int id)
        {
            EstacionamientoDTOModel estacionamiento = null;
            try
            {
                var request = new EstacionamientoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await estacionamientoService.GetByIdDTOAsync(request);
                estacionamiento = new EstacionamientoDTOModel()
                {
                    id = mensajeRespuesta.Id,
                    lugar = mensajeRespuesta.Lugar,
                    largo = mensajeRespuesta.Largo,
                    alto = mensajeRespuesta.Alto,
                    ancho = mensajeRespuesta.Ancho,
                    tipo_estacionamiento = mensajeRespuesta.IdTipoEstacionamiento
                };
            }
            catch (Exception ex) { throw ex; }
            return estacionamiento;
        }

        async Task<EstacionamientoModel> buscarEstacionamientoPorId(int id)
        {
            EstacionamientoModel estacionamiento = null;
            try
            {
                var request = new EstacionamientoId()
                {
                    Id = id
                };
                var mensaje = await estacionamientoService.GetByIdAsync(request);
                estacionamiento = new EstacionamientoModel()
                {
                    id = mensaje.Id,
                    lugar = mensaje.Lugar,
                    largo = mensaje.Largo,
                    alto = mensaje.Alto,
                    ancho = mensaje.Ancho,
                    id_tipo_estacionamiento = mensaje.IdTipoEstacionamiento
                };
            }
            catch (Exception ex) { throw ex; }
            return estacionamiento;
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            EstacionamientoModel estacionamiento = await buscarEstacionamientoPorId(id);
            ViewBag.tipoEstacionamiento = new SelectList(await listarTipoEstacionamiento(), "id", "tipo");
            return View(estacionamiento);
        }

        //------------------- EDIT TAMBIEN
        async Task<string> actualizarEstacionamiento(EstacionamientoModel estacionamiento)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Estacionamiento()
                {
                    Id = estacionamiento.id,
                    Lugar = estacionamiento.lugar,
                    Largo = estacionamiento.largo,
                    Alto = estacionamiento.alto,
                    Ancho = estacionamiento.ancho,
                    IdTipoEstacionamiento = estacionamiento.id_tipo_estacionamiento
                };
                var mensajeRespuesta = await estacionamientoService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Estacionamiento Actualizado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EstacionamientoModel estacionamiento)
        {
            ViewBag.mensaje = await actualizarEstacionamiento(estacionamiento);
            ViewBag.tipoEstacionamiento = new SelectList(await listarTipoEstacionamiento(), "id", "tipo");
            return View(estacionamiento);
        }

        //DELETE
        async Task<string> eliminarEstacionamiento(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new EstacionamientoId()
                {
                    Id = id
                };
                var mensajeRespuesta = await estacionamientoService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Estacionamiento Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarEstacionamiento(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta});
        }
    }
}
