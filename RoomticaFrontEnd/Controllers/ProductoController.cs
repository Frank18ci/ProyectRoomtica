using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class ProductoController : Controller
    {
        private ProductoService.ProductoServiceClient? ProductoServiceClient;
        public async Task<ActionResult> Index()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            ProductoServiceClient = new ProductoService.ProductoServiceClient(canal);
            var request = new Empty();
            var mensaje = await ProductoServiceClient.GetAllAsync(request);
            List<ProductoDTOModel> productoDTOs = new List<ProductoDTOModel>();
            foreach (var item in mensaje.Productos_)
            {
                productoDTOs.Add(new ProductoDTOModel()
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    unidad = item.UnidadMedidaProducto,
                    precioU = item.PrecioUnico,
                    Cantidad = item.Cantidad
                });
            }
            return View(productoDTOs);
        }
    }
}
