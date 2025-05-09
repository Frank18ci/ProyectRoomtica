using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using RoomticaGrpcServiceBackEnd;
using Google.Protobuf.WellKnownTypes;
using RoomticaFrontEnd.Models;
namespace RoomticaFrontEnd.Controllers
{
    public class CaracteristicaHabitacionTipoHabitacionController : Controller
    {
        private readonly CaracteristicaHabitacionTipoHabitacionService.CaracteristicaHabitacionTipoHabitacionServiceClient caracteristicaHabitacionTipoHabitacionService;
        private readonly TipoHabitacionService.TipoHabitacionServiceClient tipoHabitacionService;
        private GrpcChannel? chanal;

        //CONTROLLER
        public CaracteristicaHabitacionTipoHabitacionController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            caracteristicaHabitacionTipoHabitacionService = new CaracteristicaHabitacionTipoHabitacionService.CaracteristicaHabitacionTipoHabitacionServiceClient(chanal);
        }

        //LISTAR
        async Task<IEnumerable<CaracteristicaHabitacionTipoHabitacionModel>> listarCHTipoHabitacion()
        {
            List<CaracteristicaHabitacionTipoHabitacionModel> caracteristicaHabitacionTipoHabitacion = new List<CaracteristicaHabitacionTipoHabitacionModel>();
            var request = new Empty();
            var mensaje = await caracteristicaHabitacionTipoHabitacionService.GetAllAsync(request);

            foreach (var item in mensaje.CaracteristicaHabitacionTipoHabitaciones_)
            {
                caracteristicaHabitacionTipoHabitacion.Add(new CaracteristicaHabitacionTipoHabitacionModel()
                {
                    IdCaracteristicaHabitacion = item.IdCaracteristicaHabitacion,
                    IdTipoHabitacion = item.IdTipoHabitacion
                });
            }
            return caracteristicaHabitacionTipoHabitacion;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<CaracteristicaHabitacionTipoHabitacionModel> temporal = await listarCHTipoHabitacion();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c => c.IdCaracteristicaHabitacion.ToString().ToLower().Contains(nombre));
            }

            int fila = 5;
            int c = temporal.Count();
            int pags = c % fila == 0 ? c / fila : c / fila + 1;
            ViewBag.p = p;
            ViewBag.pags = pags;
            ViewBag.nombre = nombre;
            ViewBag.mensaje = mensaje;
            return View(temporal.Skip(p * fila).Take(fila));
        }

        //CREATE
        async Task<string> guardarCHTipoHabitacion(CaracteristicaHabitacionTipoHabitacionModel chth)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new CaracteristicaHabitacionTipoHabitacion()
                {
                    IdCaracteristicaHabitacion = chth.IdCaracteristicaHabitacion,
                    IdTipoHabitacion = chth.IdTipoHabitacion
                };
                var mensajeRespuesta = await caracteristicaHabitacionTipoHabitacionService.CreateAsync(request);
                mensaje = $"{mensajeRespuesta} Caracteristica Habitacion Tipo Habitacion Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Create()
        {
            return View(new CaracteristicaHabitacionTipoHabitacionModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CaracteristicaHabitacionTipoHabitacionModel chth)
        {
            ViewBag.mensaje = await guardarCHTipoHabitacion(chth);
            return View(chth);
        }

        //EDIT

    }
}
