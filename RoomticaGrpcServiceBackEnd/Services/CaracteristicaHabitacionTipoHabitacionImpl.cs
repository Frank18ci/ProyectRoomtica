using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RoomticaGrpcServiceBackEnd.Services
{
    public class CaracteristicaHabitacionTipoHabitacionImpl : CaracteristicaHabitacionTipoHabitacionService.CaracteristicaHabitacionTipoHabitacionServiceBase
    {
        private readonly ILogger<CaracteristicaHabitacionTipoHabitacionImpl> _logger;
        private readonly string cadena;
        private List<CaracteristicaHabitacionTipoHabitacion> caracteristicaHabitacionTipoHabitaciones;
        public CaracteristicaHabitacionTipoHabitacionImpl(ILogger<CaracteristicaHabitacionTipoHabitacionImpl> logger, IConfiguration configuration)
        {
            _logger = logger;
            cadena = configuration.GetConnectionString("DefaultConnection");
            caracteristicaHabitacionTipoHabitaciones = listarCaracteristicaHabitacionTipoHabitaciones();
        }
        List<CaracteristicaHabitacionTipoHabitacion> listarCaracteristicaHabitacionTipoHabitaciones()
        {
            List<CaracteristicaHabitacionTipoHabitacion> temporal = new List<CaracteristicaHabitacionTipoHabitacion>();
            using(SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_caracteristica_habitacion_tipo_habitacion", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new CaracteristicaHabitacionTipoHabitacion()
                    {
                        IdCaracteristicaHabitacion = dr.GetInt32(0),
                        IdTipoHabitacion = dr.GetInt32(1),
                        Estado = dr.GetBoolean(2)
                    });
                }
                dr.Close();
            }
            return temporal;
        }
        public override Task<CaracteristicaHabitacionTipoHabitaciones> GetAll(Empty request, ServerCallContext context)
        {
            CaracteristicaHabitacionTipoHabitaciones caracteristicaHabitacionTipoHabitaciones = new CaracteristicaHabitacionTipoHabitaciones();
            caracteristicaHabitacionTipoHabitaciones.CaracteristicaHabitacionTipoHabitaciones_.AddRange(this.caracteristicaHabitacionTipoHabitaciones);
            return Task.FromResult(caracteristicaHabitacionTipoHabitaciones);
        }
    }
}
