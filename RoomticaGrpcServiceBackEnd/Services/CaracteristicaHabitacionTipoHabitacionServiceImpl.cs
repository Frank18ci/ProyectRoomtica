using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class CaracteristicaHabitacionTipoHabitacionServiceImpl : CaracteristicaHabitacionTipoHabitacionService.CaracteristicaHabitacionTipoHabitacionServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<CaracteristicaHabitacionTipoHabitacionServiceImpl> _logger;

        public CaracteristicaHabitacionTipoHabitacionServiceImpl(IConfiguration configuration, ILogger<CaracteristicaHabitacionTipoHabitacionServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<CaracteristicaHabitacionTipoHabitaciones> GetAll(Empty request, ServerCallContext context)
        {
            List<CaracteristicaHabitacionTipoHabitacion> lista = new List<CaracteristicaHabitacionTipoHabitacion>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_caracteristica_habitacion_tipo_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new CaracteristicaHabitacionTipoHabitacion
                    {
                        IdCaracteristicaHabitacion = dr.GetInt32(0),
                        IdTipoHabitacion = dr.GetInt32(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var response = new CaracteristicaHabitacionTipoHabitaciones();
            response.CaracteristicaHabitacionTipoHabitaciones_.AddRange(lista);
            return Task.FromResult(response);
        }

        public override Task<CaracteristicaHabitacionTipoHabitaciones> GetByCaracteristicaHabitacionId(CaracteristicaHabitacionTipoHabitacionId request, ServerCallContext context)
        {
            List<CaracteristicaHabitacionTipoHabitacion> lista = new List<CaracteristicaHabitacionTipoHabitacion>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_por_id_caracteristica_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_caracteristica_habitacion", request.IdCaracteristicaHabitacion);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new CaracteristicaHabitacionTipoHabitacion
                    {
                        IdCaracteristicaHabitacion = dr.GetInt32(0),
                        IdTipoHabitacion = dr.GetInt32(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var response = new CaracteristicaHabitacionTipoHabitaciones();
            response.CaracteristicaHabitacionTipoHabitaciones_.AddRange(lista);
            return Task.FromResult(response);
        }

        public override Task<CaracteristicaHabitacionTipoHabitacion> GetById(CaracteristicaHabitacionTipoHabitacionId request, ServerCallContext context)
        {
            CaracteristicaHabitacionTipoHabitacion? registro = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_caracteristica_habitacion_tipo_habitacion_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_caracteristica_habitacion", request.IdCaracteristicaHabitacion);
                cmd.Parameters.AddWithValue("@id_tipo_habitacion", request.IdTipoHabitacion);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    registro = new CaracteristicaHabitacionTipoHabitacion
                    {
                        IdCaracteristicaHabitacion = dr.GetInt32(0),
                        IdTipoHabitacion = dr.GetInt32(1),
                        Estado = dr.GetBoolean(2)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(registro);
        }

        public override Task<CaracteristicaHabitacionTipoHabitacion> Create(CaracteristicaHabitacionTipoHabitacion request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_caracteristica_habitacion_tipo_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_caracteristica_habitacion", request.IdCaracteristicaHabitacion);
                cmd.Parameters.AddWithValue("@id_tipo_habitacion", request.IdTipoHabitacion);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<CaracteristicaHabitacionTipoHabitacion> Update(CaracteristicaHabitacionTipoHabitacion request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_caracteristica_habitacion_tipo_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_caracteristica_habitacion", request.IdCaracteristicaHabitacion);
                cmd.Parameters.AddWithValue("@id_tipo_habitacion", request.IdTipoHabitacion);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(CaracteristicaHabitacionTipoHabitacionId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_caracteristica_habitacion_tipo_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_caracteristica_habitacion", request.IdCaracteristicaHabitacion);
                cmd.Parameters.AddWithValue("@id_tipo_habitacion", request.IdTipoHabitacion);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
