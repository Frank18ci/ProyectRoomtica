using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using static RoomticaGrpcServiceBackEnd.CategoriaProductoService;
using static RoomticaGrpcServiceBackEnd.TipoDocumentoService;

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

        public async Task<ActionResult> Detail(int id = 0)
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoDocumentoService = new TipoDocumentoService.TipoDocumentoServiceClient(canal);
            var request = new TipoDocumentoId()
            {
                Id = id,
            };
            var mensaje = await tipoDocumentoService.GetByIdAsync(request);

            TipoDocumentoModel tipoDocumentoModel = new TipoDocumentoModel()
            {
                Id = mensaje.Id,
                tipo = mensaje.Tipo
            };
            return View(tipoDocumentoModel);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
