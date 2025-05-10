using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Data.SqlClient;
using static RoomticaGrpcServiceBackEnd.CategoriaProductoService;
using static RoomticaGrpcServiceBackEnd.ClienteService;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Channels;
using NuGet.Configuration;

namespace RoomticaFrontEnd.Controllers
{
    public class ClienteController : Controller
    {
        private ClienteService.ClienteServiceClient? clienteService;
        private TipoNacionalidadService.TipoNacionalidadServiceClient? tipoNacionalidadService;
        private TipoDocumentoService.TipoDocumentoServiceClient? tipoDocumentoService;
        private TipoSexoService.TipoSexoServiceClient? tipoSexoService;
        private GrpcChannel? chanal;
        public ClienteController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            clienteService = new ClienteService.ClienteServiceClient(chanal);
            tipoNacionalidadService = new TipoNacionalidadService.TipoNacionalidadServiceClient(chanal);
            tipoDocumentoService = new TipoDocumentoService.TipoDocumentoServiceClient(chanal);
            tipoSexoService = new TipoSexoService.TipoSexoServiceClient(chanal);
        }
        async Task<IEnumerable<ClienteDTOModel>> listarClientes()
        {
            List<ClienteDTOModel> clienteDTOModels = new List<ClienteDTOModel>();
            var request = new Empty();
            var mensaje = await clienteService.GetAllAsync(request);

            foreach (var item in mensaje.Clientes_)
            {
                clienteDTOModels.Add(new ClienteDTOModel()
                {
                    Id = item.Id,
                    primer_nombre = item.PrimerNombre,
                    segundo_nombre = item.SegundoNombre,
                    primer_apellido = item.PrimerApellido,
                    segundo_apellido = item.SegundoApellido,
                    tipo_documento = item.IdTipoDocumento,
                    numero_documento = item.NumeroDocumento,
                    telefono = item.Telefono,
                    email = item.Email,
                    fecha_nacimiento = item.FechaNacimiento?.ToDateTime(),
                    tipo_nacionalidad = item.IdTipoNacionalidad,
                    tipo_sexo = item.IdTipoSexo
                });
            }
            return clienteDTOModels;
        }

        async Task<IEnumerable<TipoNacionalidadModel>> listarTipoNacionalidad()
        {
            List<TipoNacionalidadModel> temporal = new List<TipoNacionalidadModel>();

            
            var request = new Empty();
            var mensaje = await tipoNacionalidadService.GetAllAsync(request);

            foreach (var item in mensaje.TipoNacionalidades_)
            {
                temporal.Add(new TipoNacionalidadModel()
                {
                    Id = item.Id,
                    tipo = item.Tipo
                });
            }
            return temporal;
        }
        async Task<IEnumerable<TipoSexoModel>> listarTipoSexo()
        {
            List<TipoSexoModel> temporal = new List<TipoSexoModel>();


            var request = new Empty();
            var mensaje = await tipoSexoService.GetAllAsync(request);

            foreach (var item in mensaje.TipoSexos_)
            {
                temporal.Add(new TipoSexoModel()
                {
                    Id = item.Id,
                    tipo = item.Tipo
                });
            }
            return temporal;
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
        async Task<string> guardarCliente(ClienteModel cliente)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Cliente()
                {
                    Id = cliente.Id,
                    PrimerNombre = cliente.primer_nombre,
                    SegundoNombre = cliente.segundo_nombre,
                    PrimerApellido = cliente.primer_apellido,
                    SegundoApellido = cliente.segundo_apellido,
                    IdTipoDocumento = cliente.id_tipo_documento,
                    NumeroDocumento = cliente.numero_documento,
                    Telefono = cliente.telefono,
                    Email = cliente.email,
                    FechaNacimiento = cliente.fecha_nacimiento.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(cliente.fecha_nacimiento.Value.ToUniversalTime()) : null,
                    IdTipoNacionalidad = cliente.id_tipo_nacionalidad,
                    IdTipoSexo = cliente.id_tipo_sexo
                };
                var mensajeRespuesta = await clienteService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Cliente Agregado";
            }
            catch(Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<ClienteModel> buscarClientePorId(int id)
        {
            ClienteModel cliente = null;
            try
            {
                var request = new ClienteId()
                {
                    Id = id
                };
                var mensajeRespuesta = await clienteService.GetByIdAsync(request);
                cliente = new ClienteModel() 
                {
                    Id = mensajeRespuesta.Id,
                    primer_nombre = mensajeRespuesta.PrimerNombre,
                    segundo_nombre = mensajeRespuesta.SegundoNombre,
                    primer_apellido = mensajeRespuesta.PrimerApellido,
                    segundo_apellido = mensajeRespuesta.SegundoApellido,
                    id_tipo_documento = mensajeRespuesta.IdTipoDocumento,
                    numero_documento = mensajeRespuesta.NumeroDocumento,
                    telefono = mensajeRespuesta.Telefono,
                    email = mensajeRespuesta.Email,
                    fecha_nacimiento = mensajeRespuesta.FechaNacimiento?.ToDateTime(),
                    id_tipo_nacionalidad = mensajeRespuesta.IdTipoNacionalidad,
                    id_tipo_sexo = mensajeRespuesta.IdTipoSexo,
                };
              
            } catch(Exception ex) { return null; }
            return cliente;
        }
        async Task<ClienteDTOModel> buscarClienteDTOPorId(int id)
        {
            ClienteDTOModel cliente = null;
            try
            {
                var request = new ClienteId()
                {
                    Id = id
                };
                var mensajeRespuesta = await clienteService.GetByIdDTOAsync(request);
                cliente = new ClienteDTOModel()
                {
                    Id = mensajeRespuesta.Id,
                    primer_nombre = mensajeRespuesta.PrimerNombre,
                    segundo_nombre = mensajeRespuesta.SegundoNombre,
                    primer_apellido = mensajeRespuesta.PrimerApellido,
                    segundo_apellido = mensajeRespuesta.SegundoApellido,
                    tipo_documento = mensajeRespuesta.IdTipoDocumento,
                    numero_documento = mensajeRespuesta.NumeroDocumento,
                    telefono = mensajeRespuesta.Telefono,
                    email = mensajeRespuesta.Email,
                    fecha_nacimiento = mensajeRespuesta.FechaNacimiento?.ToDateTime(),
                    tipo_nacionalidad = mensajeRespuesta.IdTipoNacionalidad,
                    tipo_sexo = mensajeRespuesta.IdTipoSexo,
                };

            }
            catch (Exception ex) { return null; }
            return cliente;
        }
        async Task<string> actualizarCliente(ClienteModel cliente)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new Cliente()
                {
                    Id = cliente.Id,
                    PrimerNombre = cliente.primer_nombre,
                    SegundoNombre = cliente.segundo_nombre,
                    PrimerApellido = cliente.primer_apellido,
                    SegundoApellido = cliente.segundo_apellido,
                    IdTipoDocumento = cliente.id_tipo_documento,
                    NumeroDocumento = cliente.numero_documento,
                    Telefono = cliente.telefono,
                    Email = cliente.email,
                    FechaNacimiento = cliente.fecha_nacimiento.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(cliente.fecha_nacimiento.Value.ToUniversalTime()) : null,
                    IdTipoNacionalidad = cliente.id_tipo_nacionalidad,
                    IdTipoSexo = cliente.id_tipo_sexo
                };
                var mensajeRespuesta = await clienteService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Cliente Agregado";
            }
            catch(Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }
        async Task<string> eliminarCliente(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new ClienteId()
                {
                    Id = id
                };
                var mensajeRespuesta = await clienteService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Cliente Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        { 
            IEnumerable<ClienteDTOModel> temporal = await listarClientes();

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
            ViewBag.tipoNacionalidad = new SelectList(await listarTipoNacionalidad(), "Id", "tipo");
            ViewBag.tipoDocumento = new SelectList(await listarTipoDocumento(), "Id", "Tipo");
            ViewBag.tipoSexo = new SelectList(await listarTipoSexo(), "Id", "tipo");
            return View(new ClienteModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(ClienteModel cliente)
        {
            ViewBag.mensaje = await guardarCliente(cliente);
            ViewBag.tipoNacionalidad = new SelectList(await listarTipoNacionalidad(), "Id", "tipo", cliente.id_tipo_nacionalidad);
            ViewBag.tipoDocumento = new SelectList(await listarTipoDocumento(), "Id", "Tipo", cliente.id_tipo_documento);
            ViewBag.tipoSexo = new SelectList(await listarTipoSexo(), "Id", "tipo", cliente.id_tipo_sexo);
            return View(cliente);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            ClienteModel cliente = await buscarClientePorId(id);
            ViewBag.tipoNacionalidad = new SelectList(await listarTipoNacionalidad(), "Id", "tipo", cliente.id_tipo_nacionalidad);
            ViewBag.tipoDocumento = new SelectList(await listarTipoDocumento(), "Id", "Tipo", cliente.id_tipo_documento);
            ViewBag.tipoSexo = new SelectList(await listarTipoSexo(), "Id", "tipo", cliente.id_tipo_sexo);
            return View(cliente);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ClienteModel cliente)
        {
            ViewBag.mensaje = await actualizarCliente(cliente);
            ViewBag.tipoNacionalidad = new SelectList(await listarTipoNacionalidad(), "Id", "tipo", cliente.id_tipo_nacionalidad);
            ViewBag.tipoDocumento = new SelectList(await listarTipoDocumento(), "Id", "Tipo", cliente.id_tipo_documento);
            ViewBag.tipoSexo = new SelectList(await listarTipoSexo(), "Id", "tipo", cliente.id_tipo_sexo);
            return View(cliente);
        }
        public async Task<ActionResult> Details(int id = 0)
        {
            ClienteDTOModel cliente = await buscarClienteDTOPorId(id);
            return View(cliente);
        }
        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarCliente(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }

    }
}
