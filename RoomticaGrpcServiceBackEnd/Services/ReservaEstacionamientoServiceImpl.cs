using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class ReservaEstacionamientoServiceImpl : ReservaEstacionamientoService.ReservaEstacionamientoServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<ReservaEstacionamientoServiceImpl> _logger;

        public ReservaEstacionamientoServiceImpl(IConfiguration configuration, ILogger<ReservaEstacionamientoServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }
        public override Task<ReservaEstacionamientos> GetAll(Empty request, ServerCallContext context)
        {
            List<ReservaEstacionamientoDTO> lista = new List<ReservaEstacionamientoDTO>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_reserva_estacionamiento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new ReservaEstacionamientoDTO
                    {
                        IdReserva = dr.GetInt32(0).ToString(),
                        IdEstacionamiento = dr.GetInt32(1).ToString(),
                        Cantidad = dr.GetInt32(2),
                        PrecioEstacionamiento = dr.GetDouble(3),
                        Estado = dr.GetBoolean(4)
                    });
                }
                dr.Close();
            }
            var reservas = new ReservaEstacionamientos();
            reservas.ReservaEstacionamientos_.AddRange(lista);
            return Task.FromResult(reservas);
        }


        public override Task<ReservaEstacionamientoDTO> GetByIdDTO(ReservaEstacionamientoId request, ServerCallContext context)
        {
            ReservaEstacionamientoDTO? reservaDTO = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_reserva_estacionamientoDTO_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_reserva", request.IdReserva);
                cmd.Parameters.AddWithValue("@id_estacionamiento", request.IdEstacionamiento);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    reservaDTO = new ReservaEstacionamientoDTO
                    {
                        IdReserva = dr.GetInt32(0).ToString(),
                        IdEstacionamiento = dr.GetInt32(1).ToString(),
                        Cantidad = dr.GetInt32(2),
                        PrecioEstacionamiento = dr.GetDouble(3),
                        Estado = dr.GetBoolean(4)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(reservaDTO);
        }

        public override Task<ReservaEstacionamiento> GetById(ReservaEstacionamientoId request, ServerCallContext context)
        {
            ReservaEstacionamiento? reserva = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_reserva_estacionamiento_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_reserva", request.IdReserva);
                cmd.Parameters.AddWithValue("@id_estacionamiento", request.IdEstacionamiento);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    reserva = new ReservaEstacionamiento
                    {
                        IdReserva = dr.GetInt32(0),
                        IdEstacionamiento = dr.GetInt32(1),
                        Cantidad = dr.GetInt32(2),
                        PrecioEstacionamiento = dr.GetDouble(3),
                        Estado = dr.GetBoolean(4)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(reserva);
        }

        public override Task<ReservaEstacionamiento> Create(ReservaEstacionamiento request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_reserva_estacionamiento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_estacionamiento", request.IdEstacionamiento);
                cmd.Parameters.AddWithValue("@cantidad", request.Cantidad);
                cmd.Parameters.AddWithValue("@precio_estacionamiento", request.PrecioEstacionamiento);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.IdReserva = id;
            }
            return Task.FromResult(request);
        }


        public override Task<ReservaEstacionamiento> Update(ReservaEstacionamiento request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_reserva_estacionamiento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_reserva", request.IdReserva);
                cmd.Parameters.AddWithValue("@id_estacionamiento", request.IdEstacionamiento);
                cmd.Parameters.AddWithValue("@cantidad", request.Cantidad);
                cmd.Parameters.AddWithValue("@precio_estacionamiento", request.PrecioEstacionamiento);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        
        public override Task<Empty> Delete(ReservaEstacionamientoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_reserva_estacionamiento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_reserva", request.IdReserva);
                cmd.Parameters.AddWithValue("@id_estacionamiento", request.IdEstacionamiento);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
