﻿using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class UnidadMedidaProductoImpl : UnidadMedidaProductoService.UnidadMedidaProductoServiceBase
    {
        private readonly ILogger<UnidadMedidaProductoImpl> _logger;
        private readonly string cadena ;
        private List<UnidadMedidaProducto> unidadMedidaProductos;

        public UnidadMedidaProductoImpl(ILogger<UnidadMedidaProductoImpl> logger, IConfiguration configuration)
        {
            _logger = logger;
            cadena = configuration.GetConnectionString("DefaultConnection");
            unidadMedidaProductos = ListarUnidadMedidaProductos();
            
        }

        List<UnidadMedidaProducto> ListarUnidadMedidaProductos()
        {
            List<UnidadMedidaProducto> temporal = new List<UnidadMedidaProducto>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_unidad_medida_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new UnidadMedidaProducto()
                    {
                        Id = dr.GetInt32(0),
                        Unidad = dr.GetString(1),
                        //Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        public override Task<UnidadMedidaProductos> GetAll(Empty request, ServerCallContext context)
        {
            UnidadMedidaProductos result = new UnidadMedidaProductos();
            result.UnidadMedidaProductos_.AddRange(unidadMedidaProductos);
            return Task.FromResult(result);
        }

        public override Task<UnidadMedidaProducto> GetById(UnidadMedidaProductoId request, ServerCallContext context)
        {
            UnidadMedidaProducto producto = unidadMedidaProductos.FirstOrDefault(p => p.Id == request.Id);
            return Task.FromResult(producto ?? new UnidadMedidaProducto());
        }

        public override Task<UnidadMedidaProductos> GetByUnidad(UnidadMedidaProductoUnidad request, ServerCallContext context)
        {
            var filtered = unidadMedidaProductos.Where(p => p.Unidad.ToLower() == request.Unidad.ToLower()).ToList();
            UnidadMedidaProductos result = new UnidadMedidaProductos();
            result.UnidadMedidaProductos_.AddRange(filtered);
            return Task.FromResult(result);
        }

        public override Task<UnidadMedidaProducto> Create(UnidadMedidaProducto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_unidad_medida_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@unidad", request.Unidad);
                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
                unidadMedidaProductos.Add(request);
            }
            return Task.FromResult(request);
        }

        public override Task<UnidadMedidaProducto> Update(UnidadMedidaProducto request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_unidad_medida_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@unidad", request.Unidad);
                cmd.ExecuteNonQuery();

                var index = unidadMedidaProductos.FindIndex(p => p.Id == request.Id);
                if (index >= 0)
                {
                    unidadMedidaProductos[index] = request;
                }
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(UnidadMedidaProductoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_unidad_medida_producto", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();

                unidadMedidaProductos.RemoveAll(p => p.Id == request.Id);
            }
            return Task.FromResult(new Empty());
        }
    }
}
