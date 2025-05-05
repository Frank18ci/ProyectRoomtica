using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class ConsumoServiceImpl : ConsumoService.ConsumoServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<ConsumoServiceImpl> _logger;

        public ConsumoServiceImpl(IConfiguration configuration, ILogger<ConsumoServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<Consumos> GetAll(Empty request, ServerCallContext context)
        {
            List<ConsumoDTO> lista = new List<ConsumoDTO>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_consumos", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new ConsumoDTO
                    {
                        Id = dr.GetInt32(0),
                        IdReserva = dr.GetString(1),
                        IdProducto = dr.GetString(2),
                        Cantidad = dr.GetInt32(3),
                        PrecioVenta = dr.GetDouble(4),
                        Estado = dr.GetBoolean(5)
                    });
                }
                dr.Close();
            }
            var consumos = new Consumos();
            consumos.Consumo.AddRange(lista);
            return Task.FromResult(consumos);
        }
        public override Task<ConsumoDTO> GetByIdDTO(ConsumoId request, ServerCallContext context)
        {
            ConsumoDTO? consumoDTO = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_consumoDTO_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    consumoDTO = new ConsumoDTO
                    {
                        Id = dr.GetInt32(0),
                        IdReserva = dr.GetString(1),
                        IdProducto = dr.GetString(2),
                        Cantidad = dr.GetInt32(3),
                        PrecioVenta = dr.GetDouble(4),
                        Estado = dr.GetBoolean(5)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(consumoDTO);
        }
        public override Task<Consumo> GetById(ConsumoId request, ServerCallContext context)
        {
            Consumo? consumo = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_consumo_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    consumo = new Consumo
                    {
                        Id = dr.GetInt32(0),
                        IdReserva = dr.GetInt32(1),
                        IdProducto = dr.GetInt32(2),
                        Cantidad = dr.GetInt32(3),
                        PrecioVenta = dr.GetDouble(4),
                        Estado = dr.GetBoolean(5)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(consumo);
        }

        public override Task<Consumo> Create(Consumo request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_consumo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_reserva", request.IdReserva);
                cmd.Parameters.AddWithValue("@id_producto", request.IdProducto);
                cmd.Parameters.AddWithValue("@cantidad", request.Cantidad);
                cmd.Parameters.AddWithValue("@precio_venta", request.PrecioVenta);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<Consumo> Update(Consumo request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_consumo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@id_reserva", request.IdReserva);
                cmd.Parameters.AddWithValue("@id_producto", request.IdProducto);
                cmd.Parameters.AddWithValue("@cantidad", request.Cantidad);
                cmd.Parameters.AddWithValue("@precio_venta", request.PrecioVenta);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(ConsumoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_consumo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}

