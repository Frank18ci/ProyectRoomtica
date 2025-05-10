using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class TrabajadorServiceImpl : TrabajadorService.TrabajadorServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<TrabajadorServiceImpl> _logger;

        public TrabajadorServiceImpl(IConfiguration configuration, ILogger<TrabajadorServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<Trabajadores> GetAll(Empty request, ServerCallContext context)
        {
            List<TrabajadorDTO> lista = new List<TrabajadorDTO>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_trabajadores", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TrabajadorDTO
                    {
                        Id = dr.GetInt32(0),
                        PrimerNombre = dr.GetString(1),
                        SegundoNombre = dr.GetString(2),
                        PrimerApellido = dr.GetString(3),
                        SegundoApellido = dr.GetString(4),
                        Username = dr.GetString(5),
                        Password = dr.GetString(6),
                        Sueldo = (double)dr.GetDecimal(7),
                        IdTipoDocumento = dr.GetString(8),
                        NumeroDocumento = dr.GetString(9),
                        Telefono = dr.GetString(10),
                        Email = dr.GetString(11),
                        IdRol = dr.GetString(12),
                        Estado = dr.GetBoolean(13)
                    });
                }
                dr.Close();
            }
            var trabajadores = new Trabajadores();
            trabajadores.Trabajadores_.AddRange(lista);
            return Task.FromResult(trabajadores);
        }

        public override Task<TrabajadorDTO> GetByIdDTO(TrabajadorId request, ServerCallContext context)
        {
            TrabajadorDTO? trabajadorDTO = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_trabajadorDTO_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    trabajadorDTO = new TrabajadorDTO
                    {
                        Id = dr.GetInt32(0),
                        PrimerNombre = dr.GetString(1),
                        SegundoNombre = dr.GetString(2),
                        PrimerApellido = dr.GetString(3),
                        SegundoApellido = dr.GetString(4),
                        Username = dr.GetString(5),
                        Password = dr.GetString(6),
                        Sueldo = (double)dr.GetDecimal(7),
                        IdTipoDocumento = dr.GetString(8),
                        NumeroDocumento = dr.GetString(9),
                        Telefono = dr.GetString(10),
                        Email = dr.GetString(11),
                        IdRol = dr.GetString(12),
                        Estado = dr.GetBoolean(13)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(trabajadorDTO);
        }

        public override Task<Trabajador> GetById(TrabajadorId request, ServerCallContext context)
        {
            Trabajador? trabajador = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_trabajador_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    trabajador = new Trabajador
                    {
                        Id = dr.GetInt32(0),
                        PrimerNombre = dr.GetString(1),
                        SegundoNombre = dr.GetString(2),
                        PrimerApellido = dr.GetString(3),
                        SegundoApellido = dr.GetString(4),
                        Username = dr.GetString(5),
                        Password = dr.GetString(6),
                        Sueldo = (double)dr.GetDecimal(7),
                        IdTipoDocumento = dr.GetInt32(8),
                        NumeroDocumento = dr.GetString(9),
                        Telefono = dr.GetString(10),
                        Email = dr.GetString(11),
                        IdRol = dr.GetInt32(12),
                        Estado = dr.GetBoolean(13)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(trabajador);
        }

        public override Task<Trabajador> Create(Trabajador request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_trabajador", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@primer_nombre", request.PrimerNombre);
                cmd.Parameters.AddWithValue("@segundo_nombre", request.SegundoNombre);
                cmd.Parameters.AddWithValue("@primer_apellido", request.PrimerApellido);
                cmd.Parameters.AddWithValue("@segundo_apellido", request.SegundoApellido);
                cmd.Parameters.AddWithValue("@username", request.Username);
                cmd.Parameters.AddWithValue("@password", request.Password);
                cmd.Parameters.AddWithValue("@sueldo", request.Sueldo);
                cmd.Parameters.AddWithValue("@id_tipo_documento", request.IdTipoDocumento);
                cmd.Parameters.AddWithValue("@numero_documento", request.NumeroDocumento);
                cmd.Parameters.AddWithValue("@telefono", request.Telefono);
                cmd.Parameters.AddWithValue("@email", request.Email);
                cmd.Parameters.AddWithValue("@id_rol", request.IdRol);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<Trabajador> Update(Trabajador request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_trabajador", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@primer_nombre", request.PrimerNombre);
                cmd.Parameters.AddWithValue("@segundo_nombre", request.SegundoNombre);
                cmd.Parameters.AddWithValue("@primer_apellido", request.PrimerApellido);
                cmd.Parameters.AddWithValue("@segundo_apellido", request.SegundoApellido);
                cmd.Parameters.AddWithValue("@username", request.Username);
                cmd.Parameters.AddWithValue("@password", request.Password);
                cmd.Parameters.AddWithValue("@sueldo", request.Sueldo);
                cmd.Parameters.AddWithValue("@id_tipo_documento", request.IdTipoDocumento);
                cmd.Parameters.AddWithValue("@numero_documento", request.NumeroDocumento);
                cmd.Parameters.AddWithValue("@telefono", request.Telefono);
                cmd.Parameters.AddWithValue("@email", request.Email);
                cmd.Parameters.AddWithValue("@id_rol", request.IdRol);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(TrabajadorId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_trabajador", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
        public override Task<Trabajador> Login(DatosLoginTrabajador request, ServerCallContext context)
        {
            Trabajador? trabajador = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_login_trabajador", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", request.Username);
                cmd.Parameters.AddWithValue("@password", request.Password);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    trabajador = new Trabajador
                    {
                        Id = dr.GetInt32(0),
                        PrimerNombre = dr.GetString(1),
                        SegundoNombre = dr.GetString(2),
                        PrimerApellido = dr.GetString(3),
                        SegundoApellido = dr.GetString(4),
                        Username = dr.GetString(5),
                        Password = dr.GetString(6),
                        Sueldo = Double.Parse(dr.GetDecimal(7).ToString()),
                        IdTipoDocumento = dr.GetInt32(8),
                        NumeroDocumento = dr.GetString(9),
                        Telefono = dr.GetString(10),
                        Email = dr.GetString(11),
                        IdRol = dr.GetInt32(12),
                        Estado = dr.GetBoolean(13)
                    };
                }
                dr.Close();
            }
            if (trabajador == null)
            {
                return Task.FromResult(new Trabajador()
                {
                    Id = 0,
                });
            }
            return Task.FromResult(trabajador);
        }
    }
}
