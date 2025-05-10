using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class ReservaEstacionamientoController : Controller
    {
        private ReservaEstacionamientoService.ReservaEstacionamientoServiceClient? reservaEstacionamientoService;
        private ReservaService.ReservaServiceClient? reservaService;
        private EstacionamientoService.EstacionamientoServiceClient? estacionamientoService;
        private GrpcChannel? chanal;
        public ReservaEstacionamientoController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            reservaEstacionamientoService = new ReservaEstacionamientoService.ReservaEstacionamientoServiceClient(chanal);
            reservaService = new ReservaService.ReservaServiceClient(chanal);
            estacionamientoService = new EstacionamientoService.EstacionamientoServiceClient(chanal);
        }
        async Task<IEnumerable<ReservaEstacionamientoDTOModel>> listarReservaEstacionamiento()
        {
            List<ReservaEstacionamientoDTOModel> reservaEstacionamientoDTOModel = new List<ReservaEstacionamientoDTOModel>();
            var request = new Empty();
            var mensaje = await reservaEstacionamientoService.GetAllAsync(request);

            foreach (var item in mensaje.ReservaEstacionamientos_)
            {
                reservaEstacionamientoDTOModel.Add(new ReservaEstacionamientoDTOModel()
                {
                    id_reserva = item.IdReserva,
                    id_estacionamiento = item.IdEstacionamiento,
                    cantidad = item.Cantidad,
                    precio_estacionamiento = item.PrecioEstacionamiento
                });
            }
            return reservaEstacionamientoDTOModel;
        }
        async Task<IEnumerable<ReservaModel>> listarReserva()
        {
            List<ReservaModel> temporal = new List<ReservaModel>();


            var request = new Empty();
            var mensaje = await reservaService.GetAllAsync(request);

            foreach (var item in mensaje.Reservas_)
            {
                temporal.Add(new ReservaModel()
                {
                    id = item.Id,
                    
                });
            }
            return temporal;
        }
    }
}
