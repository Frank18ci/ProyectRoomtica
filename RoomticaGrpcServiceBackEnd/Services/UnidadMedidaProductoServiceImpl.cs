using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class UnidadMedidaProductoServiceImpl : UnidadMedidaProductoService.UnidadMedidaProductoServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<UnidadMedidaProductoServiceImpl> _logger;

        public UnidadMedidaProductoServiceImpl(IConfiguration configuration, ILogger<UnidadMedidaProductoServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<UnidadMedidaProductos> GetAll(Empty request, ServerCallContext context)
        {
            List<UnidadMedidaProducto> lista = new List<UnidadMedidaProducto>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_unidad_medida_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new UnidadMedidaProducto
                    {
                        Id = dr.GetInt32(0),
                        Unidad = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }

            var unidadMedidaProductos = new UnidadMedidaProductos();
            unidadMedidaProductos.UnidadMedidaProductos_.AddRange(lista);
            return Task.FromResult(unidadMedidaProductos);
        }

        public override Task<UnidadMedidaProductos> GetByUnidad(UnidadMedidaProductoUnidad request, ServerCallContext context)
        {
            List<UnidadMedidaProducto> lista = new List<UnidadMedidaProducto>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_unidad_medida_por_unidad", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@unidad", request.Unidad);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new UnidadMedidaProducto
                    {
                        Id = dr.GetInt32(0),
                        Unidad = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }

            var unidadMedidaProductos = new UnidadMedidaProductos();
            unidadMedidaProductos.UnidadMedidaProductos_.AddRange(lista);
            return Task.FromResult(unidadMedidaProductos);
        }

        public override Task<UnidadMedidaProducto> GetById(UnidadMedidaProductoId request, ServerCallContext context)
        {
            UnidadMedidaProducto? unidadMedidaProducto = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_unidad_medida_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    unidadMedidaProducto = new UnidadMedidaProducto
                    {
                        Id = dr.GetInt32(0),
                        Unidad = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(unidadMedidaProducto);
        }

        public override Task<UnidadMedidaProducto> Create(UnidadMedidaProducto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_unidad_medida_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@unidad", request.Unidad);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<UnidadMedidaProducto> Update(UnidadMedidaProducto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_unidad_medida_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@unidad", request.Unidad);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(UnidadMedidaProductoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_unidad_medida_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
