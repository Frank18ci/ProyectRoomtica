using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class EstadoHabitacionServiceImpl : EstadoHabitacionServices.EstadoHabitacionServicesBase
    {
        private readonly string _cadena;
        private readonly ILogger<EstadoHabitacionServiceImpl> _logger;

        public EstadoHabitacionServiceImpl(IConfiguration configuration, ILogger<EstadoHabitacionServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<EstadoHabitaciones> GetAll(Empty request, ServerCallContext context)
        {
            List<EstadoHabitacion> lista = new List<EstadoHabitacion>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_estado_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new EstadoHabitacion
                    {
                        Id = dr.GetInt32(0),
                        EstadoHabitacion_ = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var estadoHabitaciones = new EstadoHabitaciones();
            estadoHabitaciones.EstadoHabitaciones_.AddRange(lista);
            return Task.FromResult(estadoHabitaciones);
        }

        public override Task<EstadoHabitaciones> GetByEstado(EstadoHabitacionEstado request, ServerCallContext context)
        {
            List<EstadoHabitacion> lista = new List<EstadoHabitacion>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_estado_habitacion_por_estado", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new EstadoHabitacion
                    {
                        Id = dr.GetInt32(0),
                        EstadoHabitacion_ = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var estadoHabitaciones = new EstadoHabitaciones();
            estadoHabitaciones.EstadoHabitaciones_.AddRange(lista);
            return Task.FromResult(estadoHabitaciones);
        }

        public override Task<EstadoHabitacion> GetById(EstadoHabitacionId request, ServerCallContext context)
        {
            EstadoHabitacion? estadoHabitacion = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_estado_habitacion_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    estadoHabitacion = new EstadoHabitacion
                    {
                        Id = dr.GetInt32(0),
                        EstadoHabitacion_ = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(estadoHabitacion);
        }

        public override Task<EstadoHabitacion> Create(EstadoHabitacion request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_estado_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@estado_habitacion", request.EstadoHabitacion_);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<EstadoHabitacion> Update(EstadoHabitacion request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_estado_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@estado_habitacion", request.EstadoHabitacion_);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(EstadoHabitacionId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_estado_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
