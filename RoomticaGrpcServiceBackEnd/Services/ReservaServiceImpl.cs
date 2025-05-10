using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class ReservaServiceImpl : ReservaService.ReservaServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<ReservaServiceImpl> _logger;

        public ReservaServiceImpl(IConfiguration configuration, ILogger<ReservaServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<Reservas> GetAll(Empty request, ServerCallContext context)
        {
            List<ReservaDTO> lista = new List<ReservaDTO>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_reservas", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new ReservaDTO
                    {
                        Id = dr.GetInt32(0),
                        IdHabitacion = dr.GetString(1),
                        IdCliente = dr.GetString(2),
                        IdTrabajador = dr.GetString(3),
                        IdTipoReserva = dr.GetString(4),
                        FechaIngreso = Timestamp.FromDateTime(dr.GetDateTime(5).ToUniversalTime()),
                        FechaSalida = Timestamp.FromDateTime(dr.GetDateTime(6).ToUniversalTime()),
                        CostoAlojamiento = Double.Parse(dr.GetDecimal(7).ToString()),
                        Estado = dr.GetBoolean(8)
                    });
                }
                dr.Close();
            }
            var reservas = new Reservas();
            reservas.Reservas_.AddRange(lista);
            return Task.FromResult(reservas);
        }

        public override Task<ReservaDTO> GetByIdDTO(ReservaId request, ServerCallContext context)
        {
            ReservaDTO? reservaDTO = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_reservaDTO_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    reservaDTO = new ReservaDTO
                    {
                        Id = dr.GetInt32(0),
                        IdHabitacion = dr.GetString(1),
                        IdCliente = dr.GetString(2),
                        IdTrabajador = dr.GetString(3),
                        IdTipoReserva = dr.GetString(4),
                        FechaIngreso = Timestamp.FromDateTime(dr.GetDateTime(5).ToUniversalTime()),
                        FechaSalida = Timestamp.FromDateTime(dr.GetDateTime(6).ToUniversalTime()),
                        CostoAlojamiento = Double.Parse(dr.GetDecimal(7).ToString()),
                        Estado = dr.GetBoolean(8)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(reservaDTO);
        }

        public override Task<Reserva> GetById(ReservaId request, ServerCallContext context)
        {
            Reserva? reserva = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_reserva_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    reserva = new Reserva
                    {
                        Id = dr.GetInt32(0),
                        IdHabitacion = dr.GetInt32(1),
                        IdCliente = dr.GetInt32(2),
                        IdTrabajador = dr.GetInt32(3),
                        IdTipoReserva = dr.GetInt32(4),
                        FechaIngreso = Timestamp.FromDateTime(dr.GetDateTime(5).ToUniversalTime()),
                        FechaSalida = Timestamp.FromDateTime(dr.GetDateTime(5).ToUniversalTime()),
                        CostoAlojamiento = Double.Parse(dr.GetDecimal(7).ToString()),
                        Estado = dr.GetBoolean(8)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(reserva);
        }

        public override Task<Reserva> Create(Reserva request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_reserva", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_habitacion", request.IdHabitacion);
                cmd.Parameters.AddWithValue("@id_cliente", request.IdCliente);
                cmd.Parameters.AddWithValue("@id_trabajador", request.IdTrabajador);
                cmd.Parameters.AddWithValue("@id_tipo_reserva", request.IdTipoReserva);
                cmd.Parameters.AddWithValue("@fecha_ingreso", request.FechaIngreso.ToDateTime());
                cmd.Parameters.AddWithValue("@fecha_salida", request.FechaSalida.ToDateTime());
                cmd.Parameters.AddWithValue("@costo_alojamiento", request.CostoAlojamiento);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<Reserva> Update(Reserva request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_reserva", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@id_habitacion", request.IdHabitacion);
                cmd.Parameters.AddWithValue("@id_cliente", request.IdCliente);
                cmd.Parameters.AddWithValue("@id_trabajador", request.IdTrabajador);
                cmd.Parameters.AddWithValue("@id_tipo_reserva", request.IdTipoReserva);
                cmd.Parameters.AddWithValue("@fecha_ingreso", request.FechaIngreso.ToDateTime());
                cmd.Parameters.AddWithValue("@fecha_salida", request.FechaSalida.ToDateTime());
                cmd.Parameters.AddWithValue("@costo_alojamiento", request.CostoAlojamiento);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(ReservaId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_reserva", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
