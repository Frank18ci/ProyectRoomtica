using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class RolTrabajadorImpl : RolTrabajadorService.RolTrabajadorServiceBase
    {
        private readonly ILogger<RolTrabajadorImpl> _logger;
        private readonly string cadena = "server=LAPTOP-H88MA8IF\\SQLEXPRESS;database=db_roomtica; trusted_connection=true; MultipleActiveResultSets=true; TrustServerCertificate=false; Encrypt=false";

        public RolTrabajadorImpl(ILogger<RolTrabajadorImpl> logger)
        {
            _logger = logger;
        }

        public override Task<RolTrabajadores> GetAll(Empty request, ServerCallContext context)
        {
            RolTrabajadores rolTrabajadores = new RolTrabajadores();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_roles_trabajador", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    rolTrabajadores.RolTrabajadores_.Add(new RolTrabajador
                    {
                        Id = dr.GetInt32(0),
                        Rol = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }

            return Task.FromResult(rolTrabajadores);
        }

        public override Task<RolTrabajadores> GetByRol(RolTrabajadorRol request, ServerCallContext context)
        {
            RolTrabajadores rolTrabajadores = new RolTrabajadores();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_roles_trabajador_por_rol", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@rol", request.Rol);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    rolTrabajadores.RolTrabajadores_.Add(new RolTrabajador
                    {
                        Id = dr.GetInt32(0),
                        Rol = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }

            return Task.FromResult(rolTrabajadores);
        }

        public override Task<RolTrabajador> GetById(RolTrabajadorId request, ServerCallContext context)
        {
            RolTrabajador rolTrabajador = new RolTrabajador();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_rol_trabajador_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    rolTrabajador.Id = dr.GetInt32(0);
                    rolTrabajador.Rol = dr.GetString(1);
                    rolTrabajador.Estado = dr.GetBoolean(2);
                }
                dr.Close();
            }

            return Task.FromResult(rolTrabajador);
        }

        public override Task<RolTrabajador> Create(RolTrabajador request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_rol_trabajador", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@rol", request.Rol);
                cmd.Parameters.AddWithValue("@estado", request.Estado);
                request.Id = Convert.ToInt32(cmd.ExecuteScalar()); // suponiendo que SP devuelve ID generado
            }

            return Task.FromResult(request);
        }

        public override Task<RolTrabajador> Update(RolTrabajador request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
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
            using (SqlConnection cn = new SqlConnection(cadena))
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
