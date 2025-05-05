using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class HabitacionServiceImpl : HabitacionServices.HabitacionServicesBase
    {
        private readonly string _cadena;
        private readonly ILogger<HabitacionServiceImpl> _logger;

        public HabitacionServiceImpl(IConfiguration configuration, ILogger<HabitacionServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<Habitaciones> GetAll(Empty request, ServerCallContext context)
        {
            List<HabitacionDTO> lista = new List<HabitacionDTO>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_habitaciones", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new HabitacionDTO
                    {
                        Id = dr.GetInt32(0),
                        Numero = dr.GetString(1),
                        Piso = dr.GetString(2),
                        PrecioDiario = Double.Parse(dr.GetDecimal(3).ToString()),
                        IdTipo = dr.GetString(4), 
                        IdEstado = dr.GetString(5), 
                        Estado = dr.GetBoolean(6)
                    });
                }
                dr.Close();
            }
            var habitaciones = new Habitaciones();
            habitaciones.Habitaciones_.AddRange(lista);
            return Task.FromResult(habitaciones);
        }

        public override Task<HabitacionDTO> GetByIdDTO(HabitacionId request, ServerCallContext context)
        {
            HabitacionDTO? habitacionDTO = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_habitacionDTO_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    habitacionDTO = new HabitacionDTO
                    {
                        Id = dr.GetInt32(0),
                        Numero = dr.GetString(1),
                        Piso = dr.GetString(2),
                        PrecioDiario = dr.GetDouble(3),
                        IdTipo = dr.GetString(4), 
                        IdEstado = dr.GetString(5), 
                        Estado = dr.GetBoolean(6)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(habitacionDTO);
        }

        public override Task<Habitacion> GetById(HabitacionId request, ServerCallContext context)
        {
            Habitacion? habitacion = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_habitacion_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    habitacion = new Habitacion
                    {
                        Id = dr.GetInt32(0),
                        Numero = dr.GetString(1),
                        Piso = dr.GetString(2),
                        PrecioDiario = dr.GetDouble(3),
                        IdTipo = dr.GetInt32(4),
                        IdEstado = dr.GetInt32(5),
                        Estado = dr.GetBoolean(6)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(habitacion);
        }

        public override Task<Habitacion> Create(Habitacion request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@numero", request.Numero);
                cmd.Parameters.AddWithValue("@piso", request.Piso);
                cmd.Parameters.AddWithValue("@precio_diario", request.PrecioDiario);
                cmd.Parameters.AddWithValue("@id_tipo", request.IdTipo);
                cmd.Parameters.AddWithValue("@id_estado", request.IdEstado);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<Habitacion> Update(Habitacion request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@numero", request.Numero);
                cmd.Parameters.AddWithValue("@piso", request.Piso);
                cmd.Parameters.AddWithValue("@precio_diario", request.PrecioDiario);
                cmd.Parameters.AddWithValue("@id_tipo", request.IdTipo);
                cmd.Parameters.AddWithValue("@id_estado", request.IdEstado);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(HabitacionId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
