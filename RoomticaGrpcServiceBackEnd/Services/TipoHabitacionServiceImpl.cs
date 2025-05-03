using Grpc.Core;
using Microsoft.Data.SqlClient;
using Google.Protobuf.WellKnownTypes;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class TipoHabitacionServiceImpl : TipoHabitacionService.TipoHabitacionServiceBase
    {
        private readonly ILogger<TipoHabitacionServiceImpl> logger;
        private List<TipoHabitacion> ListtipoHabitaciones;
        private readonly string cadena = "server=LAPTOP-H88MA8IF\\SQLEXPRESS; trusted_connection=true; database=db_roomtica; MultipleActiveResultSets=true; TrustServerCertificate=false; Encrypt=false";
        public TipoHabitacionServiceImpl(ILogger<TipoHabitacionServiceImpl> logger)
        {
            this.logger = logger;
            ListtipoHabitaciones = listarTipoHabitacions();
        }
        List<TipoHabitacion> listarTipoHabitacions()
        {
            List<TipoHabitacion> temporal = new List<TipoHabitacion>();
            using(SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_tipo_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new TipoHabitacion()
                    {
                        Id = dr.GetInt32(0),
                        Tipo = dr.GetString(1),
                        Descripccion = dr.GetString(2),
                        Estado = dr.GetBoolean(3),
                    });
                }
            }
            return temporal;
        }
        public override Task<TipoHabitaciones> GetAll(Empty request, ServerCallContext context)
        {
            TipoHabitaciones tipoHabitaciones = new TipoHabitaciones();
            tipoHabitaciones.TipoHabitaciones_.AddRange(ListtipoHabitaciones);
            return Task.FromResult(tipoHabitaciones);
        }
    }
}
