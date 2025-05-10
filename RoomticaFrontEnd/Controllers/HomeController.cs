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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            var canal = GrpcChannel.ForAddress("http://localhost:5225");
            habitacionService = new HabitacionServices.HabitacionServicesClient(canal);
            tipoReservaService = new TipoReservaService.TipoReservaServiceClient(canal);
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
        [HttpPost]
        public async Task<IActionResult> Recepcion(List<int> ClientesId, [FromForm] ReservaModel reservaModel)
        {
            
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
