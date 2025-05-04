using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class CategoriaProductoImpl : CategoriaProductoService.CategoriaProductoServiceBase
    {
        private readonly ILogger<CategoriaProductoImpl> _logger;
        private readonly string cadena;
        private List<CategoriaProducto> categoriaProductos;

        public CategoriaProductoImpl(ILogger<CategoriaProductoImpl> logger, IConfiguration configuration)
        {
            _logger = logger;
            cadena = configuration.GetConnectionString("DefaultConnection");
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
                    temporal.Add(new CategoriaProducto
                    {
                        Id = dr.GetInt32(0),
                        Categoria = dr.GetString(1),
                        //Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        public override Task<CategoriaProductos> GetAll(Empty request, ServerCallContext context)
        {
            CategoriaProductos result = new CategoriaProductos();
            result.CategoriaProductos_.AddRange(categoriaProductos);
            return Task.FromResult(result);
        }

        public override Task<CategoriaProducto> GetById(CategoriaProductoId request, ServerCallContext context)
        {
            var categoria = categoriaProductos.FirstOrDefault(c => c.Id == request.Id);
            return Task.FromResult(categoria ?? new CategoriaProducto());
        }

        public override Task<CategoriaProductos> GetByCategoria(CategoriaProductoCategoria request, ServerCallContext context)
        {
            var filtered = categoriaProductos.Where(c => c.Categoria.ToLower() == request.Categoria.ToLower()).ToList();
            CategoriaProductos result = new CategoriaProductos();
            result.CategoriaProductos_.AddRange(filtered);
            return Task.FromResult(result);
        }

        public override Task<CategoriaProducto> Create(CategoriaProducto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_categoria_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@categoria", request.Categoria);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
                categoriaProductos.Add(request);
            }
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

                var index = categoriaProductos.FindIndex(c => c.Id == request.Id);
                if (index >= 0)
                {
                    categoriaProductos[index] = request;
                }
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

                categoriaProductos.RemoveAll(c => c.Id == request.Id);
            }
            return Task.FromResult(new Empty());
        }
    }
}
