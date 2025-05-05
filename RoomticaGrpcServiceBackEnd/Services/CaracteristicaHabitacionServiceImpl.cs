using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class CaracteristicaHabitacionServiceImpl : CaracteristicaHabitacionService.CaracteristicaHabitacionServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<CaracteristicaHabitacionServiceImpl> _logger;

        public CaracteristicaHabitacionServiceImpl(IConfiguration configuration, ILogger<CaracteristicaHabitacionServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<CaracteristicaHabitaciones> GetAll(Empty request, ServerCallContext context)
        {
            var lista = new List<CaracteristicaHabitacion>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_caracteristica_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new CaracteristicaHabitacion
                    {
                        Id = dr.GetInt32(0),
                        Caracteristica = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var response = new CaracteristicaHabitaciones();
            response.CaracteristicaHabitaciones_.AddRange(lista);
            return Task.FromResult(response);
        }

        public override Task<CaracteristicaHabitaciones> GetByCaracteristica(CaracteristicaHabitacionCaracteristica request, ServerCallContext context)
        {
            var lista = new List<CaracteristicaHabitacion>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_caracteristica_habitacion_por_caracteristica", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@caracteristica", request.Caracteristica);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new CaracteristicaHabitacion
                    {
                        Id = dr.GetInt32(0),
                        Caracteristica = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var response = new CaracteristicaHabitaciones();
            response.CaracteristicaHabitaciones_.AddRange(lista);
            return Task.FromResult(response);
        }

        public override Task<CaracteristicaHabitacion> GetById(CaracteristicaHabitacionId request, ServerCallContext context)
        {
            CaracteristicaHabitacion? caracteristicaHabitacion = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_caracteristica_habitacion_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    caracteristicaHabitacion = new CaracteristicaHabitacion
                    {
                        Id = dr.GetInt32(0),
                        Caracteristica = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(caracteristicaHabitacion ?? new CaracteristicaHabitacion());
        }

        public override Task<CaracteristicaHabitacion> Create(CaracteristicaHabitacion request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_caracteristica_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@caracteristica", request.Caracteristica);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<CaracteristicaHabitacion> Update(CaracteristicaHabitacion request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_caracteristica_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@caracteristica", request.Caracteristica);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(CaracteristicaHabitacionId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_caracteristica_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
