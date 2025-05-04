using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using Google.Protobuf.WellKnownTypes;

namespace RoomticaFrontEnd.Controllers
{
    public class ClienteController : Controller
    {
        private ClienteService.ClienteServiceClient? clienteService;
        public async Task<ActionResult> Listar()
        {
            var chanal = GrpcChannel.ForAddress("http://localhost:5225");
            clienteService = new ClienteService.ClienteServiceClient(chanal);
            var request = new Empty();
            var mensaje = await clienteService.GetAllAsync(request);
            List<ClienteModel> clienteModels = new List<ClienteModel>();
            foreach (var item in mensaje.Clientes_)
            {
                clienteModels.Add(new ClienteModel()
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
            return View(clienteModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
