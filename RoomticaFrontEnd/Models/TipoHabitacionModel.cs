namespace RoomticaFrontEnd.Models
{
    public class TipoHabitacionModel
    {
        public int ? Id { get; set; }
        public string ? Tipo { get; set; } = string.Empty;
        public string ? descripccion { get; set; } = string.Empty;
        public bool ? estado { get; set; }
    }
}
