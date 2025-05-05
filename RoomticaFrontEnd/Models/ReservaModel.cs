using System;

namespace RoomticaFrontEnd.Models
{
    public class ReservaModel
    {
        public int ? id { get; set; }
        public int ? id_habitacion { get; set; }
        public int ? id_cliente { get; set; }
        public int ? id_trabajador { get; set; }
        public int ? id_tipo_reserva { get; set; }
        public string ? fecha_ingreso { get; set; }
        public string ? fecha_salida { get; set; }
        public double ? costo_alojamiento {  get; set; }
    }

    public class ReservaDTOModel
    {
        public int? id { get; set; }
        public string? id_habitacion { get; set; }
        public string? id_cliente { get; set; }
        public string? id_trabajador { get; set; }
        public string? id_tipo_reserva { get; set; }
        public string? fecha_ingreso { get; set; }
        public string? fecha_salida { get; set; }
        public double? costo_alojamiento { get; set; }
    }
}
