using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class TipoNacionalidadImpl : TipoNacionalidadService.TipoNacionalidadServiceBase
    {
        private readonly ILogger<TipoNacionalidadImpl> _logger;
        private readonly string cadena = "server=.;database=db_roomtica; trusted_connection=true; MultipleActiveResultSets=true; TrustServerCertificate=false; Encrypt=false";

        public TipoNacionalidadImpl(ILogger<TipoNacionalidadImpl> logger)
        {
            _logger = logger;
        }

        public override Task<TipoNacionalidades> GetAll(Empty request, ServerCallContext context)
        {
            TipoNacionalidades result = new TipoNacionalidades();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_nacionalidad", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result.TipoNacionalidades_.Add(new TipoNacionalidad
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }

            return Task.FromResult(result);
        }

        public override Task<TipoNacionalidades> GetByTipo(TipoNacionalidadTipo request, ServerCallContext context)
        {
            TipoNacionalidades result = new TipoNacionalidades();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_nacionalidades_por_tipo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result.TipoNacionalidades_.Add(new TipoNacionalidad
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }

            return Task.FromResult(result);
        }

        public override Task<TipoNacionalidad> GetById(TipoNacionalidadId request, ServerCallContext context)
        {
            TipoNacionalidad entity = new TipoNacionalidad();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_tipo_nacionalidad_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    entity.Id = dr.GetInt32(0);
                    entity.Tipo = dr.GetString(1);
                    entity.Estado = dr.GetBoolean(2);
                }
                dr.Close();
            }

            return Task.FromResult(entity);
        }

        public override Task<TipoNacionalidad> Create(TipoNacionalidad request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_tipo_nacionalidad", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                request.Id = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return Task.FromResult(request);
        }

        public override Task<TipoNacionalidad> Update(TipoNacionalidad request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_tipo_nacionalidad", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                cmd.ExecuteNonQuery();
            }

            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(TipoNacionalidadId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_tipo_nacionalidad", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }

            return Task.FromResult(new Empty());
        }
    }
}

