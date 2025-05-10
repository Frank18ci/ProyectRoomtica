using System.Diagnostics;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Configuration;
using RoomticaFrontEnd.Models;
using RoomticaFrontEnd.Permisos;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HabitacionServices.HabitacionServicesClient? habitacionService;
        private ClienteService.ClienteServiceClient? clienteService;
        private TipoReservaService.TipoReservaServiceClient? tipoReservaService;
        private ReservaService.ReservaServiceClient? reservaService;
        private ClienteReservaService.ClienteReservaServiceClient? clienteReservaService; 
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            habitacionService = new HabitacionServices.HabitacionServicesClient(canal);
            tipoReservaService = new TipoReservaService.TipoReservaServiceClient(canal);
            reservaService = new ReservaService.ReservaServiceClient(canal);
            clienteReservaService = new ClienteReservaService.ClienteReservaServiceClient(canal);
        }

        [ValidarSesion]
        public async Task<IActionResult> Index()
        {
            var request = new Empty();
            var mensaje = await habitacionService.GetAllAsync(request);
            List<HabitacionDTOModel> habitacionDTOModels = new List<HabitacionDTOModel>();
            foreach (var item in mensaje.Habitaciones_)
            {
                habitacionDTOModels.Add(new HabitacionDTOModel()
                {
                    id = item.Id,
                    numero = item.Numero,
                    piso = item.Piso,
                    precio_diario = item.PrecioDiario,
                    id_tipo = item.IdTipo,
                    id_estado = item.IdEstado
                });
            }
            ViewBag.habitaciones = habitacionDTOModels;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        async Task<IEnumerable<TipoReservaModel>> listarTipoReserva()
        {
            List<TipoReservaModel> tipoReservaModel = new List<TipoReservaModel>();
            var request = new Empty();
            var mensaje = await tipoReservaService.GetAllAsync(request);

            foreach (var item in mensaje.TipoReservas_)
            {
                tipoReservaModel.Add(new TipoReservaModel()
                {
                    id = item.Id,
                    tipo = item.Tipo

                });
            }
            return tipoReservaModel;
        }
        async Task<IEnumerable<HabitacionDTOModel>> listarHabitacion()
        {
            List<HabitacionDTOModel> habitacionModel = new List<HabitacionDTOModel>();
            var request = new Empty();
            var mensaje = await habitacionService.GetAllAsync(request);
            foreach (var item in mensaje.Habitaciones_)
            {
                habitacionModel.Add(new HabitacionDTOModel()
                {
                    id = item.Id,
                    numero = item.Numero,
                    piso = item.Piso,
                    precio_diario = item.PrecioDiario,
                    id_tipo = item.IdTipo,
                    id_estado = item.IdEstado
                });
            }
            return habitacionModel;
        }
        [ValidarSesion]
        public async Task<IActionResult> Recepcion()
        {
            ViewBag.tipoReservas = new SelectList(await listarTipoReserva(), "id", "tipo");
            var habitaciones = await listarHabitacion();
            var habitacionesSelect = habitaciones.Select(h => new {
                h.id,
                Texto = $"Numero: {h.numero} - Piso: {h.piso} - Precio Diario: {h.precio_diario} - Tipo: {h.id_tipo}"
            }).ToList();

            ViewBag.habitaciones = new SelectList(habitacionesSelect, "id", "Texto");
            return View();
        }


        async Task<Reserva> guardarReserva(ReservaModel reserva)
        {
            Reserva mensaje = new Reserva();
            try
            {
                var request = new Reserva()
                {
                    Id = reserva.id,
                    IdHabitacion = reserva.id_habitacion,
                    IdTrabajador = reserva.id_trabajador,
                    IdTipoReserva = reserva.id_tipo_reserva,
                    FechaIngreso = reserva.fecha_ingreso.HasValue ? Timestamp.FromDateTime(reserva.fecha_ingreso.Value.ToUniversalTime()) : null,
                    FechaSalida = reserva.fecha_salida.HasValue ? Timestamp.FromDateTime(reserva.fecha_salida.Value.ToUniversalTime()) : null,
                    CostoAlojamiento = reserva.costo_alojamiento
                };
                var mensajeRespuesta = await reservaService.CreateAsync(request);
                mensaje = mensajeRespuesta;
            }
            catch (Exception ex) {  }
            return mensaje;
        }

        async Task<ClienteReserva> guardarClienteReserva(ClienteReservaModel clienteReservaModel )
        {
            ClienteReserva mensaje = new ClienteReserva();
            try
            {
                var request = new ClienteReserva()
                {
                    Id = clienteReservaModel.Id,
                    IdCliente = clienteReservaModel.IdCliente,
                    IdReserva = clienteReservaModel.IdReserva,
                };
                var mensajeRespuesta = await clienteReservaService.CreateAsync(request);
                mensaje = mensajeRespuesta;
            }
            catch (Exception ex) {
                Console.WriteLine("Error al guardar cliente reserva: " + ex.Message);
            }
            return mensaje;
        }


        [HttpPost]
        public async Task<IActionResult> Recepcion(List<int> clientesSeleccionados, [FromForm] ReservaModel reservaModel)
        {
            Reserva reserva = await guardarReserva(reservaModel);
            foreach (var c in clientesSeleccionados)
            {
                await guardarClienteReserva(new ClienteReservaModel()
                {
                    IdReserva = reserva.Id,
                    IdCliente = c
                });
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<ActionResult> BuscarClientePorNumero(string numero = "")
        {
            ClienteModel cliente = null;
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            clienteService = new ClienteService.ClienteServiceClient(canal);
            var request = new ClienteNumeroDocumento()
            {
                NumeroDocumento = numero
            };
            var mensajeRespuesta = await clienteService.GetByNumeroDocumentoAsync(request);
            cliente = new ClienteModel()
            {
                Id = mensajeRespuesta.Id,
                primer_nombre = mensajeRespuesta.PrimerNombre,
                segundo_nombre = mensajeRespuesta.SegundoNombre,
                primer_apellido = mensajeRespuesta.PrimerApellido,
                segundo_apellido = mensajeRespuesta.SegundoApellido,
                id_tipo_documento = mensajeRespuesta.IdTipoDocumento,
                numero_documento = mensajeRespuesta.NumeroDocumento,
                telefono = mensajeRespuesta.Telefono,
                email = mensajeRespuesta.Email,
                fecha_nacimiento = mensajeRespuesta.FechaNacimiento?.ToDateTime(),
                id_tipo_nacionalidad = mensajeRespuesta.IdTipoNacionalidad,
                id_tipo_sexo = mensajeRespuesta.IdTipoSexo,
            };
            return Json(cliente);
        }
    }
}
