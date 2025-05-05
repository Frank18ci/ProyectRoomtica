using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class TipoEstacionamientoServiceImpl : TipoEstacionamientoService.TipoEstacionamientoServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<TipoEstacionamientoServiceImpl> _logger;

        public TipoEstacionamientoServiceImpl(IConfiguration configuration, ILogger<TipoEstacionamientoServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<TipoEstacionamientos> GetAll(Empty request, ServerCallContext context)
        {
            List<TipoEstacionamiento> lista = new List<TipoEstacionamiento>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_estacionamiento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoEstacionamiento
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Costo = dr.GetDouble(2),
                        Estado = dr.GetBoolean(3)
                    });
                }
                dr.Close();
            }
            var tipoEstacionamientos = new TipoEstacionamientos();
            tipoEstacionamientos.TipoEstacionamientos_.AddRange(lista);
            return Task.FromResult(tipoEstacionamientos);
        }

        public override Task<TipoEstacionamientos> GetByTipo(TipoEstacionamientoTipo request, ServerCallContext context)
        {
            List<TipoEstacionamiento> lista = new List<TipoEstacionamiento>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_estacionamiento_por_tipo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoEstacionamiento
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Costo = dr.GetDouble(2),
                        Estado = dr.GetBoolean(3)
                    });
                }
                dr.Close();
            }
            var tipoEstacionamientos = new TipoEstacionamientos();
            tipoEstacionamientos.TipoEstacionamientos_.AddRange(lista);
            return Task.FromResult(tipoEstacionamientos);
        }

        public override Task<TipoEstacionamiento> GetById(TipoEstacionamientoId request, ServerCallContext context)
        {
            TipoEstacionamiento tipoEstacionamiento = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_tipo_estacionamiento_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    tipoEstacionamiento = new TipoEstacionamiento
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Costo = dr.GetDouble(2),
                        Estado = dr.GetBoolean(3)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(tipoEstacionamiento);
        }

        public override Task<TipoEstacionamiento> Create(TipoEstacionamiento request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_tipo_estacionamiento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                cmd.Parameters.AddWithValue("@costo", request.Costo);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<TipoEstacionamiento> Update(TipoEstacionamiento request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_tipo_estacionamiento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@tipo", request.Tipo);
                cmd.Parameters.AddWithValue("@costo", request.Costo);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(TipoEstacionamientoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_tipo_estacionamiento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
