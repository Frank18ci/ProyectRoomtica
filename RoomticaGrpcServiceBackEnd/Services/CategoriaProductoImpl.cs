using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class CategoriaProductoImpl : CategoriaProductoService.CategoriaProductoServiceBase
    {
        private readonly ILogger<CategoriaProductoImpl> _logger;
        private readonly string cadena = "server=.;database=db_roomtica; trusted_connection=true; MultipleActiveResultSets=true; TrustServerCertificate=false; Encrypt=false";
        private List<CategoriaProducto> categoriaProductos;

        public CategoriaProductoImpl(ILogger<CategoriaProductoImpl> logger)
        {
            _logger = logger;
            categoriaProductos = ListarCategoriaProductos();
        }

        private List<CategoriaProducto> ListarCategoriaProductos()
        {
            List<CategoriaProducto> temporal = new List<CategoriaProducto>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_categoria_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new CategoriaProducto()
                    {
                        Id = dr.GetInt32(0),
                        Categoria = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        public override Task<CategoriaProductos> GetAll(Empty request, ServerCallContext context)
        {
            CategoriaProductos response = new CategoriaProductos();
            response.CategoriaProductos_.AddRange(categoriaProductos);
            return Task.FromResult(response);
        }

        public override Task<CategoriaProducto> GetById(CategoriaProductoId request, ServerCallContext context)
        {
            CategoriaProducto producto = categoriaProductos.FirstOrDefault(p => p.Id == request.Id);
            if (producto == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Producto con ID {request.Id} no encontrado."));
            }
            return Task.FromResult(producto);
        }

        public override Task<CategoriaProductos> GetByCategoria(CategoriaProductoCategoria request, ServerCallContext context)
        {
            var filtrados = categoriaProductos.Where(p => p.Categoria.Equals(request.Categoria, StringComparison.OrdinalIgnoreCase)).ToList();
            CategoriaProductos response = new CategoriaProductos();
            response.CategoriaProductos_.AddRange(filtrados);
            return Task.FromResult(response);
        }

        public override Task<CategoriaProducto> Create(CategoriaProducto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_insertar_categoria_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@categoria", request.Categoria);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                int idGenerado = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = idGenerado;
            }
            categoriaProductos.Add(request);
            return Task.FromResult(request);
        }

        public override Task<CategoriaProducto> Update(CategoriaProducto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_categoria_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@categoria", request.Categoria);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                cmd.ExecuteNonQuery();
            }

            var index = categoriaProductos.FindIndex(p => p.Id == request.Id);
            if (index != -1)
            {
                categoriaProductos[index] = request;
            }

            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(CategoriaProductoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_categoria_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }

            categoriaProductos.RemoveAll(p => p.Id == request.Id);

            return Task.FromResult(new Empty());
        }
    }
}
