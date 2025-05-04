using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class ProductoImpl : ProductoService.ProductoServiceBase
    {
        private readonly ILogger<ProductoImpl> _logger;
        private readonly string cadena;

        public ProductoImpl(ILogger<ProductoImpl> logger, IConfiguration configuration)
        {
            _logger = logger;
            cadena = configuration.GetConnectionString("DefaultConnection");
        }

        public override Task<Productos> GetAll(Empty request, ServerCallContext context)
        {
            Productos productos = new Productos();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_productos", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    productos.Productos_.Add(new ProductoDTO
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

            return Task.FromResult(productos);
        }

        public override Task<ProductoDTO> GetByIdDTO(ProductoId request, ServerCallContext context)
        {
            ProductoDTO productoDTO = new ProductoDTO();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_producto_dto_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    productoDTO.Id = dr.GetInt32(0);
                    productoDTO.Nombre = dr.GetString(1);
                    productoDTO.UnidadMedidaProducto = dr.GetString(2);
                    productoDTO.CategoriaProducto = dr.GetString(3);
                    productoDTO.PrecioUnico = dr.GetDouble(4);
                    productoDTO.Cantidad = dr.GetInt32(5);
                    productoDTO.Estado = dr.GetBoolean(6);
                }
                dr.Close();
            }

            return Task.FromResult(productoDTO);
        }

        public override Task<Producto> GetById(ProductoId request, ServerCallContext context)
        {
            Producto producto = new Producto();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_producto_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    producto.Id = dr.GetInt32(0);
                    producto.Nombre = dr.GetString(1);
                    producto.IdUnidadMedidaProducto = dr.GetInt32(2);
                    producto.IdCategoriaProducto = dr.GetInt32(3);
                    producto.PrecioUnico = dr.GetDouble(4);
                    producto.Cantidad = dr.GetInt32(5);
                    producto.Estado = dr.GetBoolean(6);
                }
                dr.Close();
            }

            return Task.FromResult(producto);
        }

        public override Task<Producto> Create(Producto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
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
                request.Id = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return Task.FromResult(request);
        }

        public override Task<Producto> Update(Producto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
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
            using (SqlConnection cn = new SqlConnection(cadena))
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
