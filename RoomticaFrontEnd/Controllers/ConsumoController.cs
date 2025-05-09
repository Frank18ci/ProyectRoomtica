using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class ConsumoController : Controller
    {
        private ConsumoService.ConsumoServiceClient? consumoService;
        private ReservaService.ReservaServiceClient? reservaService;
        private GrpcChannel? chanal;

        //CONTROLLER
        public ConsumoController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            consumoService = new ConsumoService.ConsumoServiceClient(chanal);
        }

        //LISTAR
        async Task<IEnumerable<ReservaDTOModel>> listarReserva()
        {
            List<ReservaDTOModel> temporal = new List<ReservaDTOModel>();
        }


        async Task<IEnumerable<ConsumoDTOModel>> listarConsumo()
        {
            List<ConsumoDTOModel> consumoDTOModel = new List<ConsumoDTOModel>();
            var request = new Empty();
            var mensaje = await consumoService.GetAllAsync(request);
            foreach (var item in mensaje.Consumo)
            {
                consumoDTOModel.Add(new ConsumoDTOModel()
                {
                    id = item.Id,
                    reserva = item.IdReserva,
                    producto = item.IdProducto,
                    Fecha = item.Fecha.ToDateTime(),
                    Monto = item.Monto
                });
            }
            return consumoDTOModel;
        }
    }
}
