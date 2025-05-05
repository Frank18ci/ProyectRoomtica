namespace RoomticaFrontEnd.Models
{
    public class HabitacionModel
    {
        public int ? id { get; set; }
        public string ? numero { get; set; }
        public string ? piso { get; set; }
        public double ? precio_diario { get; set; }
        public int ? id_tipo { get; set; }
        public int ? id_estado { get; set; }
    }

    public class HabitacionDTOModel
    {
        public int? id { get; set; }
        public string? numero { get; set; }
        public string? piso { get; set; }
        public double? precio_diario { get; set; }
        public string? id_tipo { get; set; }
        public string? id_estado { get; set; }
    }
}
