using System;

namespace RoomticaFrontEnd.Models
{
    public class PagoModel
    {
        public int ? id { get; set; }
        public int ? id_reserva {  get; set; }
        public int ? id_tipo_comprobante { get; set; }
        public double ? igv { get; set; }
        public double ? total_pago { get; set; }
        public string ? fecha_emision {  get; set; }
        public string ? fecha_pago { get; set; }
    }

    public class PagoDTOModel
    {
        public int? id { get; set; }
        public string? id_reserva { get; set; }
        public string? id_tipo_comprobante { get; set; }
        public double? igv { get; set; }
        public double? total_pago { get; set; }
        public string? fecha_emision { get; set; }
        public string? fecha_pago { get; set; }
    }
}
