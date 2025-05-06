using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class CategoriaProductoController : Controller
    {
        CategoriaProductoService.CategoriaProductoServiceClient? CategoriaProductoServiceClient;
        public async Task<ActionResult> Index()
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            CategoriaProductoServiceClient = new CategoriaProductoService.CategoriaProductoServiceClient(canal);
            var request = new Empty();
            var mensaje = await CategoriaProductoServiceClient.GetAllAsync(request);
            List<CategoriaProductoModel> categoriaProductoModels = new List<CategoriaProductoModel>();
            foreach (var item in mensaje.CategoriaProductos_)
            {
                categoriaProductoModels.Add(new CategoriaProductoModel()
                {
                    Id = item.Id,
                    Categoria = item.Categoria
                });
            }
            return View(categoriaProductoModels);
        }
        public async Task<ActionResult> Detail(int id = 0)
        {
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            CategoriaProductoServiceClient = new CategoriaProductoService.CategoriaProductoServiceClient(canal);
            var request = new CategoriaProductoId() { Id = id, };
            var mensaje = await CategoriaProductoServiceClient.GetByIdAsync(request);

            CategoriaProductoModel categoriaProductoModel = new CategoriaProductoModel()
            {
                Id = mensaje.Id,
                Categoria = mensaje.Categoria
            };
            return View(categoriaProductoModel);
        }
            
    }
       
}
