using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class UnidadMedidaProductoController : Controller
    {
        private UnidadMedidaProductoService.UnidadMedidaProductoServiceClient? UnidadMedidaProductoService;
        public async Task<IActionResult> ListarUnidadMedidaProducto()
        {
            var chanal = GrpcChannel.ForAddress("http://localhost:5225");
            UnidadMedidaProductoService = new UnidadMedidaProductoService.UnidadMedidaProductoServiceClient(chanal);
            var request = new Empty();
            var mensaje = await UnidadMedidaProductoService.GetAllAsync(request);
            List<UnidadMedidaProductoModel> unidadMedidaProductoModels = new List<UnidadMedidaProductoModel>();
            foreach (var item in mensaje.UnidadMedidaProductos_)
            {
                unidadMedidaProductoModels.Add(new UnidadMedidaProductoModel()
                {
                    Id = item.Id,
                    Unidad = item.Unidad,
                    Estado = item.Estado
                });
            }
            return View(unidadMedidaProductoModels);
        }

    }
}
