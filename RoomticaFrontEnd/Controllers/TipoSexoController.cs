using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using static RoomticaGrpcServiceBackEnd.CategoriaProductoService;

namespace RoomticaFrontEnd.Controllers
{
    public class TipoSexoController : Controller
    {
        private TipoSexoService.TipoSexoServiceClient? tipoSexoService;
        public async Task<ActionResult> Listar()
        {
            var chanal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoSexoService = new TipoSexoService.TipoSexoServiceClient(chanal);
            var request = new Empty();
            var mensaje = await tipoSexoService.GetAllAsync(request);
            List<TipoSexoModel> tipoSexoModels = new List<TipoSexoModel>();
            foreach (var item in mensaje.TipoSexos_)
            {
                tipoSexoModels.Add(new TipoSexoModel()
                {
                    Id = item.Id,
                    tipo = item.Tipo,
                });
            }
            return View(tipoSexoModels);
        }

        public async Task<ActionResult> Detail(int id = 0)
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoSexoService = new TipoSexoService.TipoSexoServiceClient(canal);
            var request = new TipoSexoId()
            {
                Id = id,
            };
            var mensaje = await tipoSexoService.GetByIdAsync(request);

            TipoSexoModel tipoSexoModel = new TipoSexoModel()
            {
                Id = mensaje.Id,
                tipo = mensaje.Tipo
            };
            return View(tipoSexoModel);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
