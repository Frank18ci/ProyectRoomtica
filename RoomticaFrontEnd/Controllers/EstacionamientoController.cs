using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class EstacionamientoController : Controller
    {
        private EstacionamientoService.EstacionamientoServiceClient? estacionamientoService;
        public async Task<ActionResult> Listar()
        {
            var chanal = GrpcChannel.ForAddress("http://localhost:5225");
            estacionamientoService = new EstacionamientoService.EstacionamientoServiceClient(chanal);
            var request = new Empty();
            var mensaje = await estacionamientoService.GetAllAsync(request);
            List<EstacionamientoDTOModel> estacionamientoDTOModels = new List<EstacionamientoDTOModel>();
            foreach (var item in mensaje.Estacionamientos_)
            {
                estacionamientoDTOModels.Add(new EstacionamientoDTOModel()
                {
                    id = item.Id,
                    lugar = item.Lugar,
                    largo = item.Largo,
                    alto = item.Alto,
                    ancho = item.Ancho,
                    //id_tipo_estacionamiento = item.IdTipoEstacionamiento
                });
            }
            return View(estacionamientoDTOModels);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
