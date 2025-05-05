namespace RoomticaFrontEnd.Models
{
    public class EstacionamientoModel
    {
        public int ? id { get; set; }
        public string ? lugar { get; set; }
        public string ? largo { get; set; }
        public string ? alto { get; set; }
        public string ? ancho { get; set; }
        public int ? id_tipo_estacionamiento { get; set; }
    }

    public class EstacionamientoDTOModel
    {
        public int? id { get; set; }
        public string? lugar { get; set; }
        public string? largo { get; set; }
        public string? alto { get; set; }
        public string? ancho { get; set; }
        public string? id_tipo_estacionamiento { get; set; }
    }
}
