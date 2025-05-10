using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class ClienteReservaServiceImpl : ClienteReservaService.ClienteReservaServiceBase
    {
        private readonly string _cadena;
        private readonly ILogger<ClienteReservaServiceImpl> _logger;

        public ClienteReservaServiceImpl(IConfiguration configuration, ILogger<ClienteReservaServiceImpl> logger)
        {
            _cadena = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }
        public override Task<ClienteReservas> GetAll(Empty request, ServerCallContext context)
        {
            List<ClienteReservaDTO> lista = new List<ClienteReservaDTO>();
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_cliente_reserva", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new ClienteReservaDTO
                    {
                        Id = dr.GetInt32(0),
                        Cliente = dr.GetString(1),
                        Reserva = dr.GetInt32(2).ToString(),
                    });
                }
                dr.Close();
            }
            var clienteReservas = new ClienteReservas();
            clienteReservas.ClienteReservas_.AddRange(lista);
            return Task.FromResult(clienteReservas);
        }
        public override Task<ClienteReserva> GetById(ClienteReservaId request, ServerCallContext context)
        {
            ClienteReserva? cliente = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_cliente_reserva_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    cliente = new ClienteReserva
                    {
                        Id = dr.GetInt32(0),
                        IdCliente = dr.GetInt32(1),
                        IdReserva = dr.GetInt32(2)
                    };
                }
                dr.Close();
            }
            return Task.FromResult(cliente);
        }
        public override Task<ClienteReservaDTO> GetByIdDTO(ClienteReservaId request, ServerCallContext context)
        {
            ClienteReservaDTO? cliente = null;
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_cliente_reservaDTO_por_id", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    cliente = new ClienteReservaDTO
                    {
                        Id = dr.GetInt32(0),
                        Cliente = dr.GetString(1),
                        Reserva = dr.GetInt32(2).ToString()
                    };
                }
                dr.Close();
            }
            return Task.FromResult(cliente);
        }
        public override Task<ClienteReserva> Create(ClienteReserva request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_insertar_cliente_reserva", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_cliente", request.IdCliente);
                cmd.Parameters.AddWithValue("@id_reserva", request.IdReserva);
                var id = Convert.ToInt32(cmd.ExecuteScalar());
                request.Id = id;
            }
            return Task.FromResult(request);
        }
        public override Task<ClienteReserva> Update(ClienteReserva request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_actualizar_cliente_reserva", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.Parameters.AddWithValue("@id_cliente", request.IdCliente);
                cmd.Parameters.AddWithValue("@id_reserva", request.IdReserva);
                
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(request);
        }
        public override Task<Empty> Delete(ClienteReservaId request, ServerCallContext context)
        {
            using (SqlConnection cn = new SqlConnection(_cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_eliminar_cliente_reserva", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", request.Id);
                cmd.ExecuteNonQuery();
            }
            return Task.FromResult(new Empty());
        }
    }
}
