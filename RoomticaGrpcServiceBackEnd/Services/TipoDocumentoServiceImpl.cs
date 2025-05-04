using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class TipoDocumentoServiceImpl : TipoDocumentoService.TipoDocumentoServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<TipoDocumentoServiceImpl> _logger;

        public TipoDocumentoServiceImpl(IConfiguration configuration, ILogger<TipoDocumentoServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<TipoDocumentos> GetAll(Empty request, ServerCallContext context)
        {
            List<TipoDocumento> lista = new List<TipoDocumento>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_documentos", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoDocumento
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var tipoDocumentos = new TipoDocumentos();
            tipoDocumentos.TipoDocumentos_.AddRange(lista);
            return Task.FromResult(tipoDocumentos);
        }

        public override Task<TipoDocumentos> GetByTipo(TipoDocumentoTipo request, ServerCallContext context)
        {
            List<TipoDocumento> lista = new List<TipoDocumento>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_documentos_por_tipo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoDocumento
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var tipoDocumentos = new TipoDocumentos();
            tipoDocumentos.TipoDocumentos_.AddRange(lista);
            return Task.FromResult(tipoDocumentos);
        }

        public override Task<TipoDocumento> GetById(TipoDocumentoId request, ServerCallContext context)
        {
            TipoDocumento tipoDocumento = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_tipo_documento_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    tipoDocumento = new TipoDocumento
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(tipoDocumento);
        }

        public override Task<TipoDocumento> Create(TipoDocumento request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_tipo_documento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<TipoDocumento> Update(TipoDocumento request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_tipo_documento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(TipoDocumentoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_tipo_documento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
