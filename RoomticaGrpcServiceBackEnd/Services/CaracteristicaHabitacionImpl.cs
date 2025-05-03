using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using RoomticaGrpcServiceBackEnd;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class CaracteristicaHabitacionImpl : CaracteristicaHabitacionService.CaracteristicaHabitacionServiceBase

    {
        private readonly ILogger<CaracteristicaHabitacionImpl> _logger;
        private readonly string cadena = "server=.;database=db_roomtica; trusted_connection=true; MultipleActiveResultSets=true; TrustServerCertificate=false; Encrypt=false";
        private List<CaracteristicaHabitacion> caracteristicaHabitacions;
        public CaracteristicaHabitacionImpl(ILogger<CaracteristicaHabitacionImpl> logger)
        {
            _logger = logger;
            caracteristicaHabitacions = listarCaracteristicaHabitacions();
        }
        List<CaracteristicaHabitacion> listarCaracteristicaHabitacions()
        {
            List<CaracteristicaHabitacion> temporal = new List<CaracteristicaHabitacion>();
            using(SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_caracteristica_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new CaracteristicaHabitacion()
                    {
                        Id = dr.GetInt32(0),
                        Caracteristica = dr.GetString(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            return temporal;
        }
        public override Task<CaracteristicaHabitaciones> GetAll(Empty request, ServerCallContext context)
        {
            CaracteristicaHabitaciones caracteristicaHabitaciones = new CaracteristicaHabitaciones();
            caracteristicaHabitaciones.CaracteristicaHabitaciones_.AddRange(caracteristicaHabitacions);
            return Task.FromResult(caracteristicaHabitaciones);
        }
    }
}
