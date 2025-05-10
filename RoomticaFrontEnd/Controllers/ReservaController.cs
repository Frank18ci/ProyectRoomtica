using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaFrontEnd.Controllers
{
    public class ReservaController : Controller
    {
        private ReservaService.ReservaServiceClient? reservaService;
        private HabitacionServices.HabitacionServicesClient? habitacionService;
        private ClienteService.ClienteServiceClient? clienteService;
        private TrabajadorService.TrabajadorServiceClient? trabajadorService;
        private TipoReservaService.TipoReservaServiceClient? tipoReservaService;
        private GrpcChannel? chanal;

        //CONTROLLER
        public ReservaController()
        {
            chanal = GrpcChannel.ForAddress("http://localhost:5225");
            reservaService = new ReservaService.ReservaServiceClient(chanal);
            habitacionService = new HabitacionServices.HabitacionServicesClient(chanal);
            clienteService = new ClienteService.ClienteServiceClient(chanal);
            trabajadorService = new TrabajadorService.TrabajadorServiceClient(chanal);
            tipoReservaService = new TipoReservaService.TipoReservaServiceClient(chanal);
        }

        //LISTAR
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

        async Task<IEnumerable<ClienteDTOModel>> listarCliente()
        {
            List<ClienteDTOModel> clienteModel = new List<ClienteDTOModel>();
            var request = new Empty();
            var mensaje = await clienteService.GetAllAsync(request);
            foreach (var item in mensaje.Clientes_)
            {
                clienteModel.Add(new ClienteDTOModel()
                {
                    Id = item.Id,
                    primer_nombre = item.PrimerNombre,
                    segundo_nombre = item.SegundoNombre,
                    primer_apellido = item.PrimerApellido,
                    segundo_apellido = item.SegundoApellido,
                    tipo_documento = item.IdTipoDocumento,
                    numero_documento = item.NumeroDocumento,
                    telefono = item.Telefono,
                    email = item.Email,
                    fecha_nacimiento = item.FechaNacimiento.ToDateTime(),
                    tipo_nacionalidad = item.IdTipoNacionalidad,
                    tipo_sexo = item.IdTipoSexo
                });
            }
            return clienteModel;
        }

        async Task<IEnumerable<TrabajadorDTOModel>> listarTrabajador()
        {
            List<TrabajadorDTOModel> trabajadorModel = new List<TrabajadorDTOModel>();
            var request = new Empty();
            var mensaje = await trabajadorService.GetAllAsync(request);
            foreach (var item in mensaje.Trabajadores_)
            {
                trabajadorModel.Add(new TrabajadorDTOModel()
                {
                    id = item.Id,
                    primer_nombre = item.PrimerNombre,
                    segundo_nombre = item.SegundoNombre,
                    primer_apellido = item.PrimerApellido,
                    segundo_apellido = item.SegundoApellido,
                    username = item.Username,
                    password = item.Password,
                    sueldo = item.Sueldo,
                    tipo_documento = item.IdTipoDocumento,
                    numero_documento = item.NumeroDocumento,
                    telefono = item.Telefono,
                    email = item.Email,
                    rol = item.IdRol
                });
            }
            return trabajadorModel;
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

        async Task<IEnumerable<ReservaDTOModel>> listarReserva()
        {
            List<ReservaDTOModel> reservaDTOModel = new List<ReservaDTOModel>();
            var request = new Empty();
            var mensaje = await reservaService.GetAllAsync(request);
            foreach (var item in mensaje.Reservas_)
            {
                reservaDTOModel.Add(new ReservaDTOModel()
                {
                    id = item.Id,
                    id_habitacion = item.IdHabitacion,
                    id_trabajador = item.IdTrabajador,
                    id_tipo_reserva = item.IdTipoReserva,
                    fecha_ingreso = item.FechaIngreso.ToDateTime(),
                    fecha_salida = item.FechaSalida.ToDateTime(),
                    costo_alojamiento = item.CostoAlojamiento,
                });
            }
            return reservaDTOModel;
        }

        public async Task<ActionResult> Listar(int p = 0, string nombre = "", string mensaje = "")
        {
            IEnumerable<ReservaDTOModel> temporal = await listarReserva();
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                temporal = temporal.Where(c =>
                    (c.id_habitacion)
                    .ToString()
                    .ToLower()
                    .Contains(nombre.ToLower()));
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
        async Task<string> guardarReserva(ReservaModel reserva)
        {
            string mensaje = string.Empty;
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
                mensaje = $"{mensajeRespuesta} Estacionamiento Agregado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.habitacion = new SelectList(await listarHabitacion(), "id", "numero");
            ViewBag.cliente = new SelectList(await listarCliente(), "Id", "primer_nombre");
            ViewBag.trabajador = new SelectList(await listarTrabajador(), "id", "primer_nombre");
            ViewBag.tipo_reserva = new SelectList(await listarTipoReserva(), "id", "tipo");
            return View(new ReservaModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(ReservaModel reserva)
        {
            ViewBag.habitacion = new SelectList(await listarHabitacion(), "id", "numero", reserva.id_habitacion);
            ViewBag.trabajador = new SelectList(await listarTrabajador(), "id", "primer_nombre", reserva.id_trabajador);
            ViewBag.tipo_reserva = new SelectList(await listarTipoReserva(), "id", "tipo", reserva.id_tipo_reserva);
            ViewBag.mensaje = await guardarReserva(reserva);
            return View(reserva);
        }

        //DETAIL
        public async Task<ActionResult> Details(int id = 0)
        {
            ReservaDTOModel reserva = await buscarReservaDTOPorId(id);
            return View(reserva);
        }

        //EDIT
        async Task<ReservaDTOModel> buscarReservaDTOPorId(int id)
        {
            ReservaDTOModel reserva = null;
            try
            {
                var request = new ReservaId()
                {
                    Id = id
                };
                var mensajeRespuesta = await reservaService.GetByIdDTOAsync(request);
                reserva = new ReservaDTOModel()
                {
                    id = mensajeRespuesta.Id,
                    id_habitacion = mensajeRespuesta.IdHabitacion,
                    id_trabajador = mensajeRespuesta.IdTrabajador,
                    id_tipo_reserva = mensajeRespuesta.IdTipoReserva,
                    fecha_ingreso = mensajeRespuesta.FechaIngreso.ToDateTime(),
                    fecha_salida = mensajeRespuesta.FechaSalida.ToDateTime(),
                    costo_alojamiento = mensajeRespuesta.CostoAlojamiento
                };
            }
            catch (Exception ex) { throw ex; }
            return reserva;
        }

        async Task<ReservaModel> buscarReservaPorId(int id)
        {
            ReservaModel reserva = null;
            try
            {
                var request = new ReservaId()
                {
                    Id = id
                };
                var mensajeRespuesta = await reservaService.GetByIdAsync(request);
                reserva = new ReservaModel()
                {
                    id = mensajeRespuesta.Id,
                    id_habitacion = mensajeRespuesta.IdHabitacion,
                    id_trabajador = mensajeRespuesta.IdTrabajador,
                    id_tipo_reserva = mensajeRespuesta.IdTipoReserva,
                    fecha_ingreso = mensajeRespuesta.FechaIngreso.ToDateTime(),
                    fecha_salida = mensajeRespuesta.FechaSalida.ToDateTime(),
                    costo_alojamiento = mensajeRespuesta.CostoAlojamiento
                };
            }
            catch (Exception ex) { throw ex; }
            return reserva;
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            ReservaModel reserva = await buscarReservaPorId(id);
            ViewBag.habitacion = new SelectList(await listarHabitacion(), "id", "numero", reserva.id_habitacion);
            ViewBag.trabajador = new SelectList(await listarTrabajador(), "id", "primer_nombre", reserva.id_trabajador);
            ViewBag.tipo_reserva = new SelectList(await listarTipoReserva(), "id", "tipo", reserva.id_tipo_reserva);
            return View(reserva);
        }

        //-------------------------------

        async Task<string> actualizarReserva(ReservaModel reserva)
        {
            string mensaje = string.Empty;
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
                var mensajeRespuesta = await reservaService.UpdateAsync(request);
                mensaje = $"{mensajeRespuesta} Estacionamiento Actualizado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ReservaModel reserva)
        {
            ViewBag.mensaje = await actualizarReserva(reserva);
            ViewBag.habitacion = new SelectList(await listarHabitacion(), "id", "numero", reserva.id_habitacion);
            ViewBag.trabajador = new SelectList(await listarTrabajador(), "id", "primer_nombre", reserva.id_trabajador);
            ViewBag.tipo_reserva = new SelectList(await listarTipoReserva(), "id", "tipo", reserva.id_tipo_reserva);
            return View(reserva);
        }

        //DELETE
        async Task<string> eliminarReserva(int id)
        {
            string mensaje = string.Empty;
            try
            {
                var request = new ReservaId()
                {
                    Id = id
                };
                var mensajeRespuesta = await reservaService.DeleteAsync(request);
                mensaje = $"{mensajeRespuesta} Estacionamiento Eliminado";
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public async Task<ActionResult> Delete(int id = 0)
        {
            string mensajeRespuesta = await eliminarReserva(id);
            return RedirectToAction("Listar", new { mensaje = mensajeRespuesta });
        }
    }
}
