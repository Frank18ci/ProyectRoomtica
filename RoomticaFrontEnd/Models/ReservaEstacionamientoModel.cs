namespace RoomticaFrontEnd.Models
{
    public class ReservaEstacionamientoModel
    {
        public int? id_reserva { get; set; }
        public int? id_estacionamiento { get; set; }
        public int? cantidad { get; set; }
        public double? precio_estacionamiento { get; set; }
    }

    public class ReservaEstacionamientoDTOModel
    {
        public string? id_reserva { get; set; }
        public string? id_estacionamiento { get; set; }
        public int? cantidad { get; set; }
        public double? precio_estacionamiento { get; set; }
    }
}
