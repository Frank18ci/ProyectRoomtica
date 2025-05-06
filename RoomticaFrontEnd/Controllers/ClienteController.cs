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

namespace RoomticaFrontEnd.Controllers
{
    public class ClienteController : Controller
    {
        private ClienteService.ClienteServiceClient? clienteService;
        private GrpcChannel? chanal;
        public ClienteController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
        }
        async Task<IEnumerable<ClienteDTOModel>> listarClientes()
        {
            List<ClienteDTOModel> clienteDTOModels = new List<ClienteDTOModel>();

            clienteService = new ClienteService.ClienteServiceClient(chanal);
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
                    fecha_nacimiento = item.FechaNacimiento,
                    tipo_nacionalidad = item.IdTipoNacionalidad,
                    tipo_sexo = item.IdTipoSexo
                });
            }
            return clienteDTOModels;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "")
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
            return View(temporal.Skip(p * fila).Take(fila));
        }


    }
}
