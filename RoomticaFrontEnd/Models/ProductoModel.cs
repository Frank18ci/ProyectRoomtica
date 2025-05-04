namespace RoomticaFrontEnd.Models
{
    public class ProductoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdUnidad { get; set; }
        public int IdCategoria { get; set; }
        public int Cantidad { get; set; }
        public double precioU { get; set; }
    }
}