using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

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

        public IActionResult Index()
        {
            return View();
        }
    }
}
