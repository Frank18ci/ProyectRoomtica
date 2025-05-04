using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class RolTrabajadorServiceImpl : RolTrabajadorService.RolTrabajadorServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<RolTrabajadorServiceImpl> _logger;

        public RolTrabajadorServiceImpl(IConfiguration configuration, ILogger<RolTrabajadorServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<RolTrabajadores> GetAll(Empty request, ServerCallContext context)
        {
            List<RolTrabajador> lista = new List<RolTrabajador>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_rol_trabajadores", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new RolTrabajador
                    {
                        Id = dr.GetInt32(0),
                        Rol = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var rolTrabajadores = new RolTrabajadores();
            rolTrabajadores.RolTrabajadores_.AddRange(lista);
            return Task.FromResult(rolTrabajadores);
        }

        public override Task<RolTrabajadores> GetByRol(RolTrabajadorRol request, ServerCallContext context)
        {
            List<RolTrabajador> lista = new List<RolTrabajador>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_rol_trabajadores_por_rol", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@rol", request.Rol);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new RolTrabajador
                    {
                        Id = dr.GetInt32(0),
                        Rol = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            var rolTrabajadores = new RolTrabajadores();
            rolTrabajadores.RolTrabajadores_.AddRange(lista);
            return Task.FromResult(rolTrabajadores);
        }

        public override Task<RolTrabajador> GetById(RolTrabajadorId request, ServerCallContext context)
        {
            RolTrabajador? rolTrabajador = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_rol_trabajador_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    rolTrabajador = new RolTrabajador
                    {
                        Id = dr.GetInt32(0),
                        Rol = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(rolTrabajador);
        }

        public override Task<RolTrabajador> Create(RolTrabajador request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_rol_trabajador", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@rol", request.Rol);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<RolTrabajador> Update(RolTrabajador request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_rol_trabajador", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@rol", request.Rol);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(RolTrabajadorId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_rol_trabajador", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
