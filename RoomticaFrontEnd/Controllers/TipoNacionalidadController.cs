using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class TipoNacionalidadController : Controller
    {
        private TipoNacionalidadService.TipoNacionalidadServiceClient? tipoNacionalidadService;
        public async Task<ActionResult> Listar()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoNacionalidadService = new TipoNacionalidadService.TipoNacionalidadServiceClient(canal);
            var request = new Empty();
            var mensaje = await tipoNacionalidadService.GetAllAsync(request);
            List<TipoNacionalidadModel> tipoNacionalidadModels = new List<TipoNacionalidadModel>();
            foreach (var item in mensaje.TipoNacionalidades_)
            {
                tipoNacionalidadModels.Add(new TipoNacionalidadModel()
                {
                    Id = item.Id,
                    tipo = item.Tipo
                });
            }
            return View(tipoNacionalidadModels);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
