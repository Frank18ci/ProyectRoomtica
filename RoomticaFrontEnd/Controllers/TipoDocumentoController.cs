using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class TipoDocumentoController : Controller
    {
        private TipoDocumentoService.TipoDocumentoServiceClient? tipoDocumentoService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoDocumentoService = new TipoDocumentoService.TipoDocumentoServiceClient(canal);
            var request = new Empty();
            var mensaje = await tipoDocumentoService.GetAllAsync(request);
            List<TipoDocumentoModel> tipoDocumentoModels = new List<TipoDocumentoModel>();
            foreach (var item in mensaje.TipoDocumentos_)
            {
                tipoDocumentoModels.Add(new TipoDocumentoModel()
                {
                    Id = item.Id,
                    tipo = item.Tipo
                });
            }
            return View(tipoDocumentoModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
