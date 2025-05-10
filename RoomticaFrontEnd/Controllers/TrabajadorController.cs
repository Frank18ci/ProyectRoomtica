using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class TrabajadorController : Controller
    {
        private TrabajadorService.TrabajadorServiceClient? trabajadorService;
        private TipoDocumentoService.TipoDocumentoServiceClient? tipoDocumentoService;
        private RolTrabajadorService.RolTrabajadorServiceClient? rolTrabajadorService;
        private GrpcChannel? chanal;
        public TrabajadorController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            trabajadorService = new TrabajadorService.TrabajadorServiceClient(chanal);
            tipoDocumentoService = new TipoDocumentoService.TipoDocumentoServiceClient(chanal);
            rolTrabajadorService = new RolTrabajadorService.RolTrabajadorServiceClient(chanal);
        }
        async Task<IEnumerable<TrabajadorDTOModel>> listarTrabajador()
        {
            List<TrabajadorDTOModel> trabajadorDTOModels = new List<TrabajadorDTOModel>();
            var request = new Empty();
            var mensaje = await trabajadorService.GetAllAsync(request);

            foreach (var item in mensaje.Trabajadores_)
            {
                trabajadorDTOModels.Add(new TrabajadorDTOModel()
                {
                    id = item.Id,
                    primer_nombre = item.PrimerNombre,
                    segundo_nombre = item.SegundoNombre,
                    primer_apellido = item.PrimerApellido,
                    segundo_apellido = item.SegundoApellido,
                    username = item.Username,
                    password = item.Password,
                    sueldo = item.Sueldo,
                    tipo_documento = item.IdTipoDocumento,
                    numero_documento = item.NumeroDocumento,
                    telefono = item.Telefono,
                    email = item.Email,
                    rol = item.IdRol
                });
            }
            return trabajadorDTOModels;
        }
        async Task<IEnumerable<TipoDocumentoModel>> listarTipoDocumento()
        {
            List<TipoDocumentoModel> temporal = new List<TipoDocumentoModel>();
            var request = new Empty();
            var mensaje = await tipoDocumentoService.GetAllAsync(request);
            foreach (var item in mensaje.TipoDocumentos_)
            {
                temporal.Add(new TipoDocumentoModel()
                {
                    Id = item.Id,
                    Tipo = item.Tipo
                });
            }
            return temporal;
        }
        async Task<IEnumerable<RolTrabajador>> listarTipoRol()
        {
            List<RolTrabajador> temporal = new List<RolTrabajador>();
            var request = new Empty();
            var mensaje = await rolTrabajadorService.GetAllAsync(request);
            foreach (var item in mensaje.RolTrabajadores_)
            {
                temporal.Add(new RolTrabajador()
                {
                    Id = item.Id,
                    Rol = item.Rol
                });
            }
            return temporal;
        }
        async Task<string> guardarTrabajador(TrabajadorModel trabajador)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Trabajador()
                {
                    Id = trabajador.id,
                    PrimerNombre = trabajador.primer_nombre,
                    SegundoNombre = trabajador.segundo_nombre,
                    PrimerApellido = trabajador.primer_apellido,
                    SegundoApellido = trabajador.segundo_apellido,
                    Username = trabajador.username,
                    Password = trabajador.password,
                    Sueldo = trabajador.sueldo,
                    IdTipoDocumento = trabajador.id_tipo_documento,
                    NumeroDocumento = trabajador.numero_documento,
                    Telefono = trabajador.telefono,
                    Email = trabajador.email,
                    IdRol = trabajador.id_rol
                };
                var mensajeRespuesta = await trabajadorService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Trabajador Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<TrabajadorModel> buscarTrabajadorPorId(int id)
        {
            TrabajadorModel trabajador = null;
            try
            {
                var request = new TrabajadorId()
                {
                    Id = id
                };
                var mensajeRespuesta = await trabajadorService.GetByIdAsync(request);
                trabajador = new TrabajadorModel()
                {
                    id = mensajeRespuesta.Id,
                    primer_nombre = mensajeRespuesta.PrimerNombre,
                    segundo_nombre = mensajeRespuesta.SegundoNombre,
                    primer_apellido = mensajeRespuesta.PrimerApellido,
                    segundo_apellido = mensajeRespuesta.SegundoApellido,
                    username = mensajeRespuesta.Username,
                    password = mensajeRespuesta.Password,
                    sueldo = mensajeRespuesta.Sueldo,
                    id_tipo_documento = mensajeRespuesta.IdTipoDocumento,
                    numero_documento = mensajeRespuesta.NumeroDocumento,
                    telefono = mensajeRespuesta.Telefono,
                    email = mensajeRespuesta.Email,
                    id_rol = mensajeRespuesta.IdRol
                };
            }
            catch (Exception ex) { return null; }
            return trabajador;
        }
        async Task<TrabajadorDTOModel> buscarTrabajadorDTOPorId(int id)
        {
            TrabajadorDTOModel trabajador = null;
            try
            {
                var request = new TrabajadorId()
                {
                    Id = id
                };
                var mensajeRespuesta = await trabajadorService.GetByIdDTOAsync(request);
                trabajador = new TrabajadorDTOModel()
                {
                    id = mensajeRespuesta.Id,
                    primer_nombre = mensajeRespuesta.PrimerNombre,
                    segundo_nombre = mensajeRespuesta.SegundoNombre,
                    primer_apellido = mensajeRespuesta.PrimerApellido,
                    segundo_apellido = mensajeRespuesta.SegundoApellido,
                    username = mensajeRespuesta.Username,
                    password = mensajeRespuesta.Password,
                    sueldo = mensajeRespuesta.Sueldo,
                    tipo_documento = mensajeRespuesta.IdTipoDocumento,
                    numero_documento = mensajeRespuesta.NumeroDocumento,
                    telefono = mensajeRespuesta.Telefono,
                    email = mensajeRespuesta.Email,
                    rol = mensajeRespuesta.IdRol,
                };

            }
            catch (Exception ex) { return null; }
            return trabajador;
        }
        async Task<string> actualizarTrabajador(TrabajadorModel trabajador)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Trabajador()
                {
                    Id = trabajador.id,
                    PrimerNombre = trabajador.primer_nombre,
                    SegundoNombre = trabajador.segundo_nombre,
                    PrimerApellido = trabajador.primer_apellido,
                    SegundoApellido = trabajador.segundo_apellido,
                    Username = trabajador.username,
                    Password = trabajador.password,
                    Sueldo = trabajador.sueldo,
                    IdTipoDocumento = trabajador.id_tipo_documento,
                    NumeroDocumento = trabajador.numero_documento,
                    Telefono = trabajador.telefono,
                    Email = trabajador.email,
                    IdRol = trabajador.id_rol
                };
                var mensajeRespuesta = await trabajadorService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Trabajador Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<string> eliminarTrabajador(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new TrabajadorId()
                {
                    Id = id
                };
                var mensajeRespuesta = await trabajadorService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Trabajador Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<TrabajadorDTOModel> temporal = await listarTrabajador();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.primer_nombre + " " + c.primer_apellido)
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
            ViewBag.TipoDocumento = new SelectList(await listarTipoDocumento(), "Id", "Tipo");
            ViewBag.TipoRol = new SelectList(await listarTipoRol(), "Id", "Rol");
            return View(new TrabajadorModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(TrabajadorModel trabajador)
        {
            ViewBag.mensaje = await guardarTrabajador(trabajador);
            ViewBag.tipoDocumento = new SelectList(await listarTipoDocumento(), "Id", "Tipo", trabajador.id_tipo_documento);
            ViewBag.TipoRol = new SelectList(await listarTipoRol(), "Id", "Rol", trabajador.id_rol);
            return View(trabajador);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            TrabajadorModel trabajador = await buscarTrabajadorPorId(id);
            ViewBag.tipoDocumento = new SelectList(await listarTipoDocumento(), "Id", "Tipo", trabajador.id_tipo_documento);
            ViewBag.TipoRol = new SelectList(await listarTipoRol(), "Id", "Rol", trabajador.id_rol);
            return View(trabajador);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(TrabajadorModel trabajador)
        {
            ViewBag.mensaje = await actualizarTrabajador(trabajador);
            ViewBag.tipoDocumento = new SelectList(await listarTipoDocumento(), "Id", "Tipo", trabajador.id_tipo_documento);
            ViewBag.TipoRol = new SelectList(await listarTipoRol(), "Id", "Rol", trabajador.id_rol);
            return View(trabajador);
        }
        public async Task<ActionResult> Details(int id = 0)
        {
            TrabajadorDTOModel trabajador = await buscarTrabajadorDTOPorId(id);
            return View(trabajador);
        }
        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarTrabajador(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }
}
