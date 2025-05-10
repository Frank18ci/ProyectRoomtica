using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using static RoomticaGrpcServiceBackEnd.CategoriaProductoService;
using static RoomticaGrpcServiceBackEnd.RolTrabajadorService;

namespace RoomticaFrontEnd.Controllers
{
    public class RolTrabajadorController : Controller
    {
        private RolTrabajadorService.RolTrabajadorServiceClient? rolTrabajadorService;
        private GrpcChannel? chanal;
        public RolTrabajadorController()
        {
            chanal = GrpcChannel.ForAddress("https://localhost:5001");
            rolTrabajadorService = new RolTrabajadorService.RolTrabajadorServiceClient(chanal);
        }
        async Task<IEnumerable<RolTrabajadorModel>> listarRolTrabajador()
        {
            List<RolTrabajadorModel> rolTrabajadorModels = new List<RolTrabajadorModel>();
            var request = new Empty();
            var mensaje = await rolTrabajadorService.GetAllAsync(request);

            foreach (var item in mensaje.RolTrabajadores_)
            {
                rolTrabajadorModels.Add(new RolTrabajadorModel()
                {
                    Id = item.Id,
                    Rol = item.Rol
                });
            }
            return rolTrabajadorModels;
        }
        async Task<string> guardarRolTrabajador(RolTrabajadorModel rolTrabajador)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new RolTrabajador()
                {
                    Id = rolTrabajador.Id,
                    Rol = rolTrabajador.Rol

                };
                var mensajeRespuesta = await rolTrabajadorService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Rol Trabajador Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<RolTrabajadorModel> buscarRolTrabajadorPorId(int id)
        {
            RolTrabajadorModel rolTrabajador = null;
            try
            {
                var request = new RolTrabajadorId()
                {
                    Id = id
                };
                var mensajeRespuesta = await rolTrabajadorService.GetByIdAsync(request);
                rolTrabajador = new RolTrabajadorModel()
                {
                    Id = mensajeRespuesta.Id,
                    Rol = mensajeRespuesta.Rol                    
                };
            }
            catch (Exception ex) { return null; }
            return rolTrabajador;
        }
        async Task<string> actualizarRolTrabajador(RolTrabajadorModel rolTrabajador)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new RolTrabajador()
                {
                    Id = rolTrabajador.Id,
                    Rol = rolTrabajador.Rol
                };
                var mensajeRespuesta = await rolTrabajadorService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Rol Trabajador Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<string> eliminarRolTrabajador(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new RolTrabajadorId()
                {
                    Id = id
                };
                var mensajeRespuesta = await rolTrabajadorService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Rol Trabajador Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<RolTrabajadorModel> temporal = await listarRolTrabajador();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.Rol)
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
            return View(new RolTrabajadorModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(RolTrabajadorModel rolTrabajador)
        {
            ViewBag.mensaje = await guardarRolTrabajador(rolTrabajador);            
            return View(rolTrabajador);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            RolTrabajadorModel rolTrabajador = await buscarRolTrabajadorPorId(id);           
            return View(rolTrabajador);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(RolTrabajadorModel rolTrabajador)
        {
            ViewBag.mensaje = await actualizarRolTrabajador(rolTrabajador);           
            return View(rolTrabajador);
        }
        public async Task<ActionResult> Details(int id = 0)
        {
            RolTrabajadorModel rolTrabajador = await buscarRolTrabajadorPorId(id);
            return View(rolTrabajador);
        }
        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarRolTrabajador(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }

    }
}
