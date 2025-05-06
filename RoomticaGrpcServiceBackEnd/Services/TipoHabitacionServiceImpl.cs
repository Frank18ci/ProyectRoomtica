using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class TipoHabitacionServiceImpl : TipoHabitacionService.TipoHabitacionServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<TipoHabitacionServiceImpl> _logger;

        public TipoHabitacionServiceImpl(IConfiguration configuration, ILogger<TipoHabitacionServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<TipoHabitaciones> GetAll(Empty request, ServerCallContext context)
        {
            List<TipoHabitacion> lista = new List<TipoHabitacion>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoHabitacion
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Descripccion = dr.GetString(2),
                        Estado = dr.GetBoolean(3)
                    });
                }
                dr.Close();
            }

            var tipoHabitaciones = new TipoHabitaciones();
            tipoHabitaciones.TipoHabitaciones_.AddRange(lista);
            return Task.FromResult(tipoHabitaciones);
        }

        public override Task<TipoHabitaciones> GetByTipo(TipoHabitacionTipo request, ServerCallContext context)
        {
            List<TipoHabitacion> lista = new List<TipoHabitacion>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_habitacion_por_tipo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoHabitacion
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Descripccion = dr.GetString(2),
                        Estado = dr.GetBoolean(3)
                    });
                }
                dr.Close();
            }

            var tipoHabitaciones = new TipoHabitaciones();
            tipoHabitaciones.TipoHabitaciones_.AddRange(lista);
            return Task.FromResult(tipoHabitaciones);
        }

        public override Task<TipoHabitacion> GetById(TipoHabitacionId request, ServerCallContext context)
        {
            TipoHabitacion? tipoHabitacion = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_tipo_habitacion_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    tipoHabitacion = new TipoHabitacion
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Descripccion = dr.GetString(2),
                        Estado = dr.GetBoolean(3)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(tipoHabitacion);
        }

        public override Task<TipoHabitacion> Create(TipoHabitacion request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_tipo_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                cmd.Parameters.AddWithValue("@descripccion", request.Descripccion);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<TipoHabitacion> Update(TipoHabitacion request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_tipo_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                cmd.Parameters.AddWithValue("@descripccion", request.Descripccion);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(TipoHabitacionId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_tipo_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
