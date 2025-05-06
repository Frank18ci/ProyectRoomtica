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

namespace RoomticaFrontEnd.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IConfiguration _config;
        public ClienteController(IConfiguration config)
        {
            _config = config;
        }

        private ClienteService.ClienteServiceClient? clienteService;
        public async Task<ActionResult> Listar()
        {
            var chanal = GrpcChannel.ForAddress("http://localhost:5225");
            clienteService = new ClienteService.ClienteServiceClient(chanal);
            var request = new Empty();
            var mensaje = await clienteService.GetAllAsync(request);
            List<ClienteDTOModel> clienteDTOModels = new List<ClienteDTOModel>();
            foreach (var item in mensaje.Clientes_)
            {
                clienteDTOModels.Add(new ClienteDTOModel()
                {
                    Id = item.Id,
                    primer_nombre = item.PrimerNombre,
                    segundo_nombre = item.SegundoNombre,
                    primer_apellido = item.PrimerApellido,
                    segundo_apellido = item.SegundoApellido,
                    id_tipo_documento = item.IdTipoDocumento,
                    numero_documento = item.NumeroDocumento,
                    telefono = item.Telefono,
                    email = item.Email,
                    fecha_nacimiento = item.FechaNacimiento,
                    id_tipo_nacionalidad = item.IdTipoNacionalidad,
                    id_tipo_sexo = item.IdTipoSexo
                });
            }
            return View(clienteDTOModels);
        }

        public async Task<ActionResult> Detail(int id = 0)
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            clienteService = new ClienteService.ClienteServiceClient(canal);
            var request = new ClienteId() { Id = id, };
            var mensaje = await clienteService.GetByIdDTOAsync(request);

            ClienteDTOModel clienteDTOModel = new ClienteDTOModel()
            {
                Id = mensaje.Id,
                primer_nombre = mensaje.PrimerNombre,
                segundo_nombre = mensaje.SegundoNombre,
                primer_apellido = mensaje.PrimerApellido,
                segundo_apellido = mensaje.SegundoApellido,
                id_tipo_documento = mensaje.IdTipoDocumento.ToString(),
                numero_documento = mensaje.NumeroDocumento,
                telefono = mensaje.Telefono,
                email = mensaje.Email,
                fecha_nacimiento = mensaje.FechaNacimiento,
                id_tipo_nacionalidad = mensaje.IdTipoNacionalidad.ToString(),
                id_tipo_sexo = mensaje.IdTipoSexo.ToString()
            };
            return View(clienteDTOModel);
        }

        IEnumerable<TipoDocumentoModel> tipoDocumento()
        {
            List<TipoDocumentoModel> temporal = new List<TipoDocumentoModel>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionString:sql"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_documento", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new TipoDocumentoModel
                    {
                        Id = dr.GetInt32(0),
                        tipo = dr.GetString(1)
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        IEnumerable<TipoNacionalidadModel> tipoNacionalidad()
        {
            List<TipoNacionalidadModel> temporal = new List<TipoNacionalidadModel>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionString:sql"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_nacionalidades", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new TipoNacionalidadModel
                    {
                        Id = dr.GetInt32(0),
                        tipo = dr.GetString(1)
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        IEnumerable<TipoSexoModel> tipoSexo()
        {
            List<TipoSexoModel> temporal = new List<TipoSexoModel>();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionString:sql"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_sexo", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new TipoSexoModel
                    {
                        Id = dr.GetInt32(0),
                        tipo = dr.GetString(1)
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        public async Task<ActionResult> Edit(int? id = null)
        {
            if (id == null)
                return RedirectToAction("Listar");

            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            clienteService = new ClienteService.ClienteServiceClient(canal);
            var request = new ClienteId() { Id = id.Value };
            var mensaje = await clienteService.GetByIdAsync(request);

            ClienteModel clienteModel = new ClienteModel()
            {
                Id = mensaje.Id,
                primer_nombre = mensaje.PrimerNombre,
                segundo_nombre = mensaje.SegundoNombre,
                primer_apellido = mensaje.PrimerApellido,
                segundo_apellido = mensaje.SegundoApellido,
                id_tipo_documento = mensaje.IdTipoDocumento,
                numero_documento = mensaje.NumeroDocumento,
                telefono = mensaje.Telefono,
                email = mensaje.Email,
                fecha_nacimiento = mensaje.FechaNacimiento,
                id_tipo_nacionalidad = mensaje.IdTipoNacionalidad,
                id_tipo_sexo = mensaje.IdTipoSexo
            };

            ViewBag.tipoDocumentos = new SelectList(tipoDocumento(), "Id", "tipo", mensaje.IdTipoDocumento);
            ViewBag.tipoNacionalidades = new SelectList(tipoNacionalidad(), "Id", "tipo", mensaje.IdTipoNacionalidad);
            ViewBag.tipoSexos = new SelectList(tipoSexo(), "Id", "tipo", mensaje.IdTipoSexo);

            return View(await Task.Run(() => mensaje));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cliente mensaje)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.tipoDocumentos = new SelectList(tipoDocumento(), "Id", "tipo", mensaje.IdTipoDocumento);
                ViewBag.tipoNacionalidades = new SelectList(tipoNacionalidad(), "Id", "tipo", mensaje.IdTipoNacionalidad);
                ViewBag.tipoSexos = new SelectList(tipoSexo(), "Id", "tipo", mensaje.IdTipoSexo);
                return View(await Task.Run(() => mensaje));
            }

            ViewBag.mensaje = ClienteServiceClient.Update(mensaje);
        }
    }
}
