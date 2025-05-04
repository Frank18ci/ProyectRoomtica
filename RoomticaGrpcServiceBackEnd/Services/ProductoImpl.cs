using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class ProductoServiceImpl : ProductoService.ProductoServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<ProductoServiceImpl> _logger;

        public ProductoServiceImpl(IConfiguration configuration, ILogger<ProductoServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }
        public override Task<Productos> GetAll(Empty request, ServerCallContext context)
        {
            List<ProductoDTO> lista = new List<ProductoDTO>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_productos", cn); 
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new ProductoDTO
                    {
                        Id = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        UnidadMedidaProducto = dr.GetString(2),
                        CategoriaProducto = dr.GetString(3),
                        PrecioUnico = dr.GetDouble(4),
                        Cantidad = dr.GetInt32(5),
                        Estado = dr.GetBoolean(6)
                    });
                }
                dr.Close();
            }
            var productos = new Productos();
            productos.Productos_.AddRange(lista);
            return Task.FromResult(productos);
        }
        public override Task<ProductoDTO> GetByIdDTO(ProductoId request, ServerCallContext context)
        {
            ProductoDTO productoDTO = new ProductoDTO();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_productoDTO_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    productoDTO = new ProductoDTO
                    {
                        Id = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        UnidadMedidaProducto = dr.GetString(2),
                        CategoriaProducto = dr.GetString(3),
                        PrecioUnico = dr.GetDouble(4),
                        Cantidad = dr.GetInt32(5),
                        Estado = dr.GetBoolean(6)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(productoDTO ?? new ProductoDTO());
        }
        public override Task<Producto> GetById(ProductoId request, ServerCallContext context)
        {
            Producto producto = new Producto();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_producto_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    producto = new Producto
                    {
                        Id = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        IdUnidadMedidaProducto = dr.GetInt32(2),
                        IdCategoriaProducto = dr.GetInt32(3),
                        PrecioUnico = dr.GetDouble(4),
                        Cantidad = dr.GetInt32(5),
                        Estado = dr.GetBoolean(6)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(producto ?? new Producto());
        }
        public override Task<Producto> Create(Producto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", request.Nombre);
                cmd.Parameters.AddWithValue("@id_unidad_medida_producto", request.IdUnidadMedidaProducto);
                cmd.Parameters.AddWithValue("@id_categoria_producto", request.IdCategoriaProducto);
                cmd.Parameters.AddWithValue("@precio_unico", request.PrecioUnico);
                cmd.Parameters.AddWithValue("@cantidad", request.Cantidad);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }
        public override Task<Producto> Update(Producto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@nombre", request.Nombre);
                cmd.Parameters.AddWithValue("@id_unidad_medida_producto", request.IdUnidadMedidaProducto);
                cmd.Parameters.AddWithValue("@id_categoria_producto", request.IdCategoriaProducto);
                cmd.Parameters.AddWithValue("@precio_unico", request.PrecioUnico);
                cmd.Parameters.AddWithValue("@cantidad", request.Cantidad);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }
        public override Task<Empty> Delete(ProductoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
