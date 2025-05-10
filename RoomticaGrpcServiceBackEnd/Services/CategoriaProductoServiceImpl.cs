using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class CategoriaProductoServiceImpl : CategoriaProductoService.CategoriaProductoServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<CategoriaProductoServiceImpl> _logger;

        public CategoriaProductoServiceImpl(IConfiguration configuration, ILogger<CategoriaProductoServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<CategoriaProductos> GetAll(Empty request, ServerCallContext context)
        {
            List<CategoriaProducto> lista = new List<CategoriaProducto>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_categorias_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new CategoriaProducto
                    {
                        Id = dr.GetInt32(0),
                        Categoria = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var categorias = new CategoriaProductos();
            categorias.CategoriaProductos_.AddRange(lista);
            return Task.FromResult(categorias);
        }

        public override Task<CategoriaProductos> GetByCategoria(CategoriaProductoCategoria request, ServerCallContext context)
        {
            List<CategoriaProducto> lista = new List<CategoriaProducto>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_categoria_producto_por_categoria", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@categoria", request.Categoria);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new CategoriaProducto
                    {
                        Id = dr.GetInt32(0),
                        Categoria = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var categorias = new CategoriaProductos();
            categorias.CategoriaProductos_.AddRange(lista);
            return Task.FromResult(categorias);
        }

        public override Task<CategoriaProducto> GetById(CategoriaProductoId request, ServerCallContext context)
        {
            CategoriaProducto? categoria = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_categoria_producto_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    categoria = new CategoriaProducto
                    {
                        Id = dr.GetInt32(0),
                        Categoria = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(categoria);
        }

        public override Task<CategoriaProducto> Create(CategoriaProducto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_categoria_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@categoria", request.Categoria);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<CategoriaProducto> Update(CategoriaProducto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_categoria_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@categoria", request.Categoria);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(CategoriaProductoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_categoria_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
