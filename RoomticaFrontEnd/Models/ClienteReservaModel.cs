using System.ComponentModel.DataAnnotations;

namespace RoomticaFrontEnd.Models
{
    public class ClienteReservaModel
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Id Cliente")]
        public int IdCliente { get; set; }
        [Required]
        [Display(Name = "Id Reserva")]
        public int IdReserva { get; set; }
    }
    public class ClienteReservaDTOModel
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Cliente")]
        public string Cliente { get; set; }
        [Required]
        [Display(Name = "Reserva")]
        public string Reserva { get; set; }
    }
}
