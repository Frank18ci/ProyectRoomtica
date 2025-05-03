using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class TipoSexoImpl : TipoSexoService.TipoSexoServiceBase
    {
        private readonly ILogger<TipoSexoImpl> _logger;
        private readonly string cadena = "server=LAPTOP-H88MA8IF\\SQLEXPRESS;database=db_roomtica; trusted_connection=true; MultipleActiveResultSets=true; TrustServerCertificate=false; Encrypt=false";

        public TipoSexoImpl(ILogger<TipoSexoImpl> logger)
        {
            _logger = logger;
        }

        public override Task<TipoSexos> GetAll(Empty request, ServerCallContext context)
        {
            TipoSexos tipoSexos = new TipoSexos();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_sexo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    tipoSexos.TipoSexos_.Add(new TipoSexo
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }

            return Task.FromResult(tipoSexos);
        }

        public override Task<TipoSexos> GetByTipo(TipoSexoTipo request, ServerCallContext context)
        {
            TipoSexos tipoSexos = new TipoSexos();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_tipo_sexo_por_tipo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    tipoSexos.TipoSexos_.Add(new TipoSexo
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }

            return Task.FromResult(tipoSexos);
        }

        public override Task<TipoSexo> GetById(TipoSexoId request, ServerCallContext context)
        {
            TipoSexo tipoSexo = new TipoSexo();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_tipo_sexo_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    tipoSexo.Id = dr.GetInt32(0);
                    tipoSexo.Tipo = dr.GetString(1);
                    tipoSexo.Estado = dr.GetBoolean(2);
                }
                dr.Close();
            }

            return Task.FromResult(tipoSexo);
        }

        public override Task<TipoSexo> Create(TipoSexo request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_tipo_sexo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                request.Id = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return Task.FromResult(request);
        }

        public override Task<TipoSexo> Update(TipoSexo request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_tipo_sexo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                cmd.ExecuteNonQuery();
            }

            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(TipoSexoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_tipo_sexo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }

            return Task.FromResult(new Empty());
        }
    }
}

