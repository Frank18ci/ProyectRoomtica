using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class TipoNacionalidadServiceImpl : TipoNacionalidadService.TipoNacionalidadServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<TipoNacionalidadServiceImpl> _logger;

        public TipoNacionalidadServiceImpl(IConfiguration configuration, ILogger<TipoNacionalidadServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        // GetAll: Obtiene todos los tipos de nacionalidad
        public override Task<TipoNacionalidades> GetAll(Empty request, ServerCallContext context)
        {
            List<TipoNacionalidad> lista = new List<TipoNacionalidad>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_nacionalidades", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoNacionalidad
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var tipoNacionalidades = new TipoNacionalidades();
            tipoNacionalidades.TipoNacionalidades_.AddRange(lista);
            return Task.FromResult(tipoNacionalidades);
        }

        // GetByTipo: Filtra los tipos de nacionalidad por tipo
        public override Task<TipoNacionalidades> GetByTipo(TipoNacionalidadTipo request, ServerCallContext context)
        {
            List<TipoNacionalidad> lista = new List<TipoNacionalidad>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_tipo_nacionalidades_por_tipo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoNacionalidad
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var tipoNacionalidades = new TipoNacionalidades();
            tipoNacionalidades.TipoNacionalidades_.AddRange(lista);
            return Task.FromResult(tipoNacionalidades);
        }

        // GetById: Obtiene un tipo de nacionalidad por id
        public override Task<TipoNacionalidad> GetById(TipoNacionalidadId request, ServerCallContext context)
        {
            TipoNacionalidad tipoNacionalidad = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_tipo_nacionalidad_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    tipoNacionalidad = new TipoNacionalidad
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(tipoNacionalidad);
        }

        // Create: Crea un nuevo tipo de nacionalidad
        public override Task<TipoNacionalidad> Create(TipoNacionalidad request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_tipo_nacionalidad", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        // Update: Actualiza un tipo de nacionalidad existente
        public override Task<TipoNacionalidad> Update(TipoNacionalidad request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
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

        // Delete: Elimina un tipo de nacionalidad por id
        public override Task<Empty> Delete(TipoNacionalidadId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
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
