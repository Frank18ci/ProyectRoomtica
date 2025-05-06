using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class ClienteServiceImpl : ClienteService.ClienteServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<ClienteServiceImpl> _logger;

        public ClienteServiceImpl(IConfiguration configuration, ILogger<ClienteServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public override Task<Clientes> GetAll(Empty request, ServerCallContext context)
        {
            List<ClienteDTO> lista = new List<ClienteDTO>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_clientes", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new ClienteDTO
                    {
                        Id = dr.GetInt32(0),
                        PrimerNombre = dr.GetString(1),
                        SegundoNombre = dr.GetString(2),
                        PrimerApellido = dr.GetString(3),
                        SegundoApellido = dr.GetString(4),
                        IdTipoDocumento = dr.GetString(5),
                        NumeroDocumento = dr.GetString(6),
                        Telefono = dr.GetString(7),
                        Email = dr.GetString(8),
                        FechaNacimiento = Timestamp.FromDateTime(dr.GetDateTime(9).ToUniversalTime()),
                        IdTipoNacionalidad = dr.GetString(10),
                        IdTipoSexo = dr.GetString(11),
                        Estado = dr.GetBoolean(12)
                    });
                }
                dr.Close();
            }
            var clientes = new Clientes();
            clientes.Clientes_.AddRange(lista);
            return Task.FromResult(clientes);
        }

        public override Task<ClienteDTO> GetByIdDTO(ClienteId request, ServerCallContext context)
        {
            ClienteDTO? clienteDTO = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_clienteDTO_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    clienteDTO = new ClienteDTO
                    {
                        Id = dr.GetInt32(0),
                        PrimerNombre = dr.GetString(1),
                        SegundoNombre = dr.GetString(2),
                        PrimerApellido = dr.GetString(3),
                        SegundoApellido = dr.GetString(4),
                        IdTipoDocumento = dr.GetString(5),
                        NumeroDocumento = dr.GetString(6),
                        Telefono = dr.GetString(7),
                        Email = dr.GetString(8),
                        FechaNacimiento = Timestamp.FromDateTime(dr.GetDateTime(9).ToUniversalTime()),
                        IdTipoNacionalidad = dr.GetString(10),
                        IdTipoSexo = dr.GetString(11),
                        Estado = dr.GetBoolean(12)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(clienteDTO);
        }

        public override Task<Cliente> GetById(ClienteId request, ServerCallContext context)
        {
            Cliente? cliente = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_cliente_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    cliente = new Cliente
                    {
                        Id = dr.GetInt32(0),
                        PrimerNombre = dr.GetString(1),
                        SegundoNombre = dr.GetString(2),
                        PrimerApellido = dr.GetString(3),
                        SegundoApellido = dr.GetString(4),
                        IdTipoDocumento = dr.GetInt32(5),
                        NumeroDocumento = dr.GetString(6),
                        Telefono = dr.GetString(7),
                        Email = dr.GetString(8),
                        FechaNacimiento = Timestamp.FromDateTime(dr.GetDateTime(9).ToUniversalTime()),
                        IdTipoNacionalidad = dr.GetInt32(10),
                        IdTipoSexo = dr.GetInt32(11),
                        Estado = dr.GetBoolean(12)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(cliente);
        }

        public override Task<Cliente> Create(Cliente request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_crear_cliente", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@primer_nombre", request.PrimerNombre);
                cmd.Parameters.AddWithValue("@segundo_nombre", request.SegundoNombre);
                cmd.Parameters.AddWithValue("@primer_apellido", request.PrimerApellido);
                cmd.Parameters.AddWithValue("@segundo_apellido", request.SegundoApellido);
                cmd.Parameters.AddWithValue("@id_tipo_documento", request.IdTipoDocumento);
                cmd.Parameters.AddWithValue("@numero_documento", request.NumeroDocumento);
                cmd.Parameters.AddWithValue("@telefono", request.Telefono);
                cmd.Parameters.AddWithValue("@email", request.Email);
                cmd.Parameters.AddWithValue("@fecha_nacimiento", request.FechaNacimiento.ToDateTime());
                cmd.Parameters.AddWithValue("@id_tipo_nacionalidad", request.IdTipoNacionalidad);
                cmd.Parameters.AddWithValue("@id_tipo_sexo", request.IdTipoSexo);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }

        public override Task<Cliente> Update(Cliente request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_cliente", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@primer_nombre", request.PrimerNombre);
                cmd.Parameters.AddWithValue("@segundo_nombre", request.SegundoNombre);
                cmd.Parameters.AddWithValue("@primer_apellido", request.PrimerApellido);
                cmd.Parameters.AddWithValue("@segundo_apellido", request.SegundoApellido);
                cmd.Parameters.AddWithValue("@id_tipo_documento", request.IdTipoDocumento);
                cmd.Parameters.AddWithValue("@numero_documento", request.NumeroDocumento);
                cmd.Parameters.AddWithValue("@telefono", request.Telefono);
                cmd.Parameters.AddWithValue("@email", request.Email);
                cmd.Parameters.AddWithValue("@fecha_nacimiento", request.FechaNacimiento.ToDateTime());
                cmd.Parameters.AddWithValue("@id_tipo_nacionalidad", request.IdTipoNacionalidad);
                cmd.Parameters.AddWithValue("@id_tipo_sexo", request.IdTipoSexo);
                cmd.Parameters.AddWithValue("@estado", request.Estado);

                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }

        public override Task<Empty> Delete(ClienteId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_cliente", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
