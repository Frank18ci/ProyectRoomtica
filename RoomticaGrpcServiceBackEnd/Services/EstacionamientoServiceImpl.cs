using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class EstacionamientoServiceImpl : EstacionamientoService.EstacionamientoServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<EstacionamientoServiceImpl> _logger;

        public EstacionamientoServiceImpl(IConfiguration configuration, ILogger<EstacionamientoServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<Estacionamientos> GetAll(Empty request, ServerCallContext context)
        {
            List<EstacionamientoDTO> lista = new List<EstacionamientoDTO>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_estacionamientos", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new EstacionamientoDTO
                    {
                        Id = dr.GetInt32(0),
                        Lugar = dr.GetString(1),
                        Largo = dr.GetString(2),
                        Alto = dr.GetString(3),
                        Ancho = dr.GetString(4),
                        IdTipoEstacionamiento = dr.GetString(5),
                        Estado = dr.GetBoolean(6)
                    });
                }
                dr.Close();
            }
            var estacionamientos = new Estacionamientos();
            estacionamientos.Estacionamientos_.AddRange(lista);
            return Task.FromResult(estacionamientos);
        }

        public override Task<EstacionamientoDTO> GetByIdDTO(EstacionamientoId request, ServerCallContext context)
        {
            EstacionamientoDTO? estacionamientoDTO = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_estacionamientoDTO_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    estacionamientoDTO = new EstacionamientoDTO
                    {
                        Id = dr.GetInt32(0),
                        Lugar = dr.GetString(1),
                        Largo = dr.GetString(2),
                        Alto = dr.GetString(3),
                        Ancho = dr.GetString(4),
                        IdTipoEstacionamiento = dr.GetString(5),
                        Estado = dr.GetBoolean(6)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(estacionamientoDTO);
        }

        public override Task<Estacionamiento> GetById(EstacionamientoId request, ServerCallContext context)
        {
            Estacionamiento? estacionamiento = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_estacionamiento_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    estacionamiento = new Estacionamiento
                    {
                        Id = dr.GetInt32(0),
                        Lugar = dr.GetString(1),
                        Largo = dr.GetString(2),
                        Alto = dr.GetString(3),
                        Ancho = dr.GetString(4),
                        IdTipoEstacionamiento = dr.GetInt32(5),
                        Estado = dr.GetBoolean(6)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(estacionamiento);
        }

        public override Task<Estacionamiento> Create(Estacionamiento request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_estacionamiento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@lugar", request.Lugar);
                cmd.Parameters.AddWithValue("@largo", request.Largo);
                cmd.Parameters.AddWithValue("@alto", request.Alto);
                cmd.Parameters.AddWithValue("@ancho", request.Ancho);
                cmd.Parameters.AddWithValue("@id_tipo_estacionamiento", request.IdTipoEstacionamiento);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<Estacionamiento> Update(Estacionamiento request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_estacionamiento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@lugar", request.Lugar);
                cmd.Parameters.AddWithValue("@largo", request.Largo);
                cmd.Parameters.AddWithValue("@alto", request.Alto);
                cmd.Parameters.AddWithValue("@ancho", request.Ancho);
                cmd.Parameters.AddWithValue("@id_tipo_estacionamiento", request.IdTipoEstacionamiento);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(EstacionamientoId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_estacionamiento", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
