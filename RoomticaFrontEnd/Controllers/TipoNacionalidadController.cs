using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using static RoomticaGrpcServiceBackEnd.CategoriaProductoService;

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

        public async Task<ActionResult> Detail(int id = 0)
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoNacionalidadService = new TipoNacionalidadService.TipoNacionalidadServiceClient(canal);
            var request = new TipoNacionalidadId()
            {
                Id = id,
            };
            var mensaje = await tipoNacionalidadService.GetByIdAsync(request);

            TipoNacionalidadModel tipoNacionalidadModel = new TipoNacionalidadModel()
            {
                Id = mensaje.Id,
                tipo = mensaje.Tipo
            };
            return View(tipoNacionalidadModel);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
