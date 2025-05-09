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
        private GrpcChannel? chanal;
        public ReservaEstacionamientoController()
        {
            chanal = GrpcChannel.ForAddress("https://localhost:5001");
            reservaEstacionamientoService = new ReservaEstacionamientoService.ReservaEstacionamientoServiceClient(chanal);
        }        
    }
}
